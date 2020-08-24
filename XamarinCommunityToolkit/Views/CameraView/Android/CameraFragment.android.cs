using System;
using System.Collections.Generic;
using System.IO;

#if __ANDROID_29__
using AndroidX.Core.Content;
using AndroidX.Fragment.App;
#else
using Android.Support.V4.Content;
using Android.Support.V4.App;
#endif

using Android;
using Android.Hardware.Camera2.Params;
using Android.Graphics;
using Android.Content;
using Android.Content.PM;
using Android.Hardware.Camera2;
using Android.Media;
using Android.Views;
using Android.Runtime;
using Android.OS;
using AOrientation = Android.Content.Res.Orientation;
using AVideoSource = Android.Media.VideoSource;
using AView = Android.Views.View;
using ASize = Android.Util.Size;
using App = Android.App.Application;
using Env = Android.OS.Environment;

using Java.Lang;
using Java.Util.Concurrent;

using Xamarin.Forms.Internals;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using System.Reflection;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraFragment : Fragment, TextureView.ISurfaceTextureListener
	{
		CameraDevice device;
		CaptureRequest.Builder sessionBuilder;
		CameraCaptureSession session;

		AutoFitTextureView texture;
		ImageReader photoReader;
		MediaRecorder mediaRecorder;
		bool audioPermissionsGranted;
		bool cameraPermissionsGranted;
		ASize previewSize, videoSize, photoSize;
		int sensorOrientation;
		LensFacing cameraType;

		bool busy;
		bool flashSupported;
		bool stabilizationSupported;
		bool repeatingIsRunning;
		FlashMode flashMode;
		string cameraId;
		string videoFile;
		Java.Util.Concurrent.Semaphore captureSessionOpenCloseLock = new Java.Util.Concurrent.Semaphore(1);
		CameraTemplate cameraTemplate;
		HandlerThread backgroundThread;
		Handler backgroundHandler = null;

		float zoom = 1;
		bool ZoomSupported => maxDigitalZoom != 0;
		float maxDigitalZoom;
		Rect activeRect;

		public bool IsRecordingVideo { get; set; }

		bool UseSystemSound { get; set; }

		CameraManager manager;
		CameraManager Manager => manager ??= (CameraManager)Context.GetSystemService(Context.CameraService);

		MediaActionSound mediaSound;
		MediaActionSound MediaSound => mediaSound ??= new MediaActionSound();

		TaskCompletionSource<CameraDevice> initTaskSource;
		TaskCompletionSource<bool> permissionsRequested;

		bool IsBusy
		{
			get => device == null || busy;
			set
			{
				busy = value;
				if (Element != null)
					Element.IsBusy = value;
			}
		}

		bool Available
		{
			get => Element?.IsAvailable ?? false;
			set
			{
				if (Element?.IsAvailable != value)
					Element.IsAvailable = value;
			}
		}

		public CameraView Element { get; set; }

		public override AView OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return inflater.Inflate(Resource.Layout.CameraFragment, null);
		}

		public override void OnViewCreated(AView view, Bundle savedInstanceState)
		{
			texture = view.FindViewById<AutoFitTextureView>(Resource.Id.cameratexture);
		}

		public override async void OnResume()
		{
			base.OnResume();
			StartBackgroundThread();
			if (texture is null)
				return;
			if (texture.IsAvailable)
			{
				UpdateBackgroundColor();
				UpdateCaptureOptions();
				await RetrieveCameraDevice(force: true);
			}
			else
			{
				texture.SurfaceTextureListener = this;
			}
		}

		public override void OnPause()
		{
			CloseSession();
			StopBackgroundThread();
			base.OnPause();
		}

		void StartBackgroundThread()
		{
			backgroundThread = new HandlerThread("CameraBackground");
			backgroundThread.Start();
			backgroundHandler = new Handler(backgroundThread.Looper);
		}

		void StopBackgroundThread()
		{
			if (backgroundThread == null)
				return;

			backgroundThread.QuitSafely();
			try
			{
				backgroundThread.Join();
				backgroundThread = null;
				backgroundHandler = null;
			}
			catch (InterruptedException e)
			{
				LogError("BackgroundThread stoping error", e);
			}
		}

		public async Task RetrieveCameraDevice(bool force = false)
		{
			if (Context == null || (!force && initTaskSource != null))
				return;

			if (device != null)
				CloseDevice();

			await RequestCameraPermissions();
			if (!cameraPermissionsGranted)
				return;

			if (!captureSessionOpenCloseLock.TryAcquire(2500, TimeUnit.Milliseconds))
				throw new RuntimeException("Time out waiting to lock camera opening.");

			IsBusy = true;
			cameraId = GetCameraId();

			if (string.IsNullOrEmpty(cameraId))
			{
				IsBusy = false;
				captureSessionOpenCloseLock.Release();
				//_texture.ClearCanvas(Element.BackgroundColor.ToAndroid()); // HANG after select valid camera...
				Element.RaiseMediaCaptureFailed($"No {Element.CameraOptions} camera found");
			}
			else
			{
				try
				{
					var characteristics = Manager.GetCameraCharacteristics(cameraId);
					var map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);

					flashSupported = characteristics.Get(CameraCharacteristics.FlashInfoAvailable) == Java.Lang.Boolean.True;
					stabilizationSupported = false;
					var stabilizationModes = characteristics.Get(CameraCharacteristics.ControlAvailableVideoStabilizationModes);
					if (stabilizationModes != null)
					{
						var modes = (int[])stabilizationModes;
						foreach (var mode in modes)
						{
							if (mode == (int)ControlVideoStabilizationMode.On)
								stabilizationSupported = true;
						}
					}
					Element.MaxZoom = maxDigitalZoom = (float)characteristics.Get(CameraCharacteristics.ScalerAvailableMaxDigitalZoom);
					activeRect = (Rect)characteristics.Get(CameraCharacteristics.SensorInfoActiveArraySize);
					photoSize = GetMaxSize(map.GetOutputSizes((int)ImageFormatType.Jpeg));
					videoSize = GetMaxSize(map.GetOutputSizes(Class.FromType(typeof(MediaRecorder))));
					previewSize = ChooseOptimalSize(
						map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture))),
						texture.Width,
						texture.Height,
						cameraTemplate == CameraTemplate.Record ? videoSize : photoSize);
					sensorOrientation = (int)characteristics.Get(CameraCharacteristics.SensorOrientation);
					cameraType = (LensFacing)(int)characteristics.Get(CameraCharacteristics.LensFacing);

					if (Resources.Configuration.Orientation == AOrientation.Landscape)
						texture.SetAspectRatio(previewSize.Width, previewSize.Height);
					else
						texture.SetAspectRatio(previewSize.Height, previewSize.Width);

					initTaskSource = new TaskCompletionSource<CameraDevice>();

					Manager.OpenCamera(
						cameraId,
						new CameraStateListener
						{
							OnOpenedAction = device =>
							{
								initTaskSource?.TrySetResult(device);
							},
							OnDisconnectedAction = device =>
							{
								initTaskSource?.TrySetResult(null);
								CloseDevice(device);
							},
							OnErrorAction = (device, error) =>
							{
								initTaskSource?.TrySetResult(device);
								Element?.RaiseMediaCaptureFailed($"Camera device error: {error}");
								CloseDevice(device);
							},
							OnClosedAction = device =>
							{
								initTaskSource?.TrySetResult(null);
								CloseDevice(device);
							}
						},
						backgroundHandler);

					captureSessionOpenCloseLock.Release();
					device = await initTaskSource.Task;
					initTaskSource = null;
					if (device != null)
					{
						await PrepareSession();
					}
				}
				catch (Java.Lang.Exception error)
				{
					LogError("Failed to open camera", error);
					Available = false;
				}
				finally
				{
					IsBusy = false;
				}
			}
		}

		public void UpdateCaptureOptions()
		{
			switch (Element.CaptureOptions)
			{
				default:
				case CameraCaptureOptions.Photo:
					cameraTemplate = CameraTemplate.Preview;
					break;
				case CameraCaptureOptions.Video:
					cameraTemplate = CameraTemplate.Record;
					break;
			}
		}

		public void TakePhoto()
		{
			if (IsBusy || cameraTemplate != CameraTemplate.Preview)
				return;

			try
			{
				if (device != null)
				{
					session.StopRepeating();
					repeatingIsRunning = false;
					sessionBuilder.AddTarget(photoReader.Surface);
					sessionBuilder.Set(CaptureRequest.FlashMode, (int)flashMode);
					session.Capture(sessionBuilder.Build(), null, null);
					sessionBuilder.RemoveTarget(photoReader.Surface);
					UpdateRepeatingRequest();
				}
			}
			catch (Java.Lang.Exception error)
			{
				LogError("Failed to take photo", error);
			}
		}

		void OnPhoto(object sender, byte[] data)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Element?.RaiseMediaCaptured(new MediaCapturedEventArgs()
				{
					Data = data,
					Image = ImageSource.FromStream(() => new MemoryStream(data))
				});
			});
		}

		void OnVideo(object sender, string data)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Element?.RaiseMediaCaptured(new MediaCapturedEventArgs()
				{
					Video = MediaSource.FromFile(data)
				});
			});
		}

		void SetupImageReader()
		{
			DisposeImageReader();

			photoReader = ImageReader.NewInstance(640, 480, ImageFormatType.Jpeg, maxImages: 1);

			var readerListener = new ImageAvailableListener();
			readerListener.Photo += (_, bytes) =>
			{
				if (Element.SavePhotoToFile)
					File.WriteAllBytes(ConstructMediaFilename(null, "jpg"), bytes);
				Sound(MediaActionSoundType.ShutterClick);
				OnPhoto(this, bytes);
			};

			photoReader.SetOnImageAvailableListener(readerListener, backgroundHandler);
		}

		void SetupMediaRecorder(Surface previewSurface)
		{
			DisposeMediaRecorder();

			mediaRecorder = new MediaRecorder();
			mediaRecorder.SetPreviewDisplay(previewSurface);
			if (audioPermissionsGranted)
				mediaRecorder.SetAudioSource(AudioSource.Camcorder);
			mediaRecorder.SetVideoSource(AVideoSource.Surface);

			var profile = GetCamcoderProfile();
			if (profile != null)
			{
				mediaRecorder.SetProfile(profile);
			}
			else
			{
				mediaRecorder.SetOutputFormat(OutputFormat.Mpeg4);
				mediaRecorder.SetVideoEncodingBitRate(10000000);
				mediaRecorder.SetVideoFrameRate(30);
				mediaRecorder.SetVideoSize(videoSize.Width, videoSize.Height);
				mediaRecorder.SetVideoEncoder(VideoEncoder.H264);
				if (audioPermissionsGranted)
				{
					mediaRecorder.SetAudioEncoder(AudioEncoder.Default);
				}
			}

			videoFile = ConstructMediaFilename("VID", "mp4");

			mediaRecorder.SetOutputFile(videoFile);
			mediaRecorder.SetOrientationHint(GetCaptureOrientation());
			mediaRecorder.Prepare();
		}

		CamcorderProfile GetCamcoderProfile()
		{
			var cameraId = Convert.ToInt32(this.cameraId);
			if (CamcorderProfile.HasProfile(cameraId, CamcorderQuality.HighSpeed1080p))
				return CamcorderProfile.Get(cameraId, CamcorderQuality.HighSpeed1080p);
			else if (CamcorderProfile.HasProfile(cameraId, CamcorderQuality.HighSpeed720p))
				return CamcorderProfile.Get(cameraId, CamcorderQuality.HighSpeed720p);
			else if (CamcorderProfile.HasProfile(cameraId, CamcorderQuality.HighSpeed480p))
				return CamcorderProfile.Get(cameraId, CamcorderQuality.HighSpeed480p);
			else if (CamcorderProfile.HasProfile(cameraId, CamcorderQuality.High))
				return CamcorderProfile.Get(cameraId, CamcorderQuality.High);
			else if (CamcorderProfile.HasProfile(cameraId, CamcorderQuality.Low))
				return CamcorderProfile.Get(cameraId, CamcorderQuality.Low);

			return null;
		}

		public void StartRecord()
		{
			if (IsBusy)
			{
				return;
			}
			else if (IsRecordingVideo)
			{
				Element?.RaiseMediaCaptureFailed("Video already recording.");
				return;
			}
			else if (cameraTemplate != CameraTemplate.Record)
			{
				Element?.RaiseMediaCaptureFailed($"Unexpected error: Camera {cameraTemplate} not configured to record video.");
				return;
			}
			else if (mediaRecorder == null)
			{
				Element?.RaiseMediaCaptureFailed($"Unexpected error: MediaRecorder is not initialized.");
				IsRecordingVideo = false;
				return;
			}

			try
			{
				Sound(MediaActionSoundType.StartVideoRecording);
				mediaRecorder.Start();
				IsRecordingVideo = true;
			}
			catch (Java.Lang.Exception error)
			{
				LogError("Failed to take video", error);
				Element?.RaiseMediaCaptureFailed($"Failed to take video: {error}");
				DisposeMediaRecorder();
			}
		}

		public async void StopRecord()
		{
			if (IsBusy || !IsRecordingVideo || session == null || mediaRecorder == null)
				return;

			try
			{
				DisposeMediaRecorder();
				await PrepareSession();
			}
			catch (Java.Lang.Exception ex)
			{
				LogError("Stop record exception", ex);
			}
			finally
			{
				IsRecordingVideo = false;
			}

			Sound(MediaActionSoundType.StopVideoRecording);
			OnVideo(this, videoFile);
		}

		async Task PrepareSession()
		{
			IsBusy = true;
			try
			{
				CloseSession();

				sessionBuilder = device.CreateCaptureRequest(cameraTemplate);

				SetFlash();
				SetVideoStabilization();
				ApplyZoom();

				var surfaces = new List<Surface>();

				// preview texture
				if (texture.IsAvailable && previewSize != null)
				{
					var texture = this.texture.SurfaceTexture;
					texture.SetDefaultBufferSize(previewSize.Width, previewSize.Height);
					var previewSurface = new Surface(texture);
					surfaces.Add(previewSurface);
					sessionBuilder.AddTarget(previewSurface);

					// video mode
					if (cameraTemplate == CameraTemplate.Record)
					{
						SetupMediaRecorder(previewSurface);
						var _mediaSurface = mediaRecorder.Surface;
						surfaces.Add(_mediaSurface);
						sessionBuilder.AddTarget(_mediaSurface);
					}
					// photo mode
					else
					{
						SetupImageReader();
						surfaces.Add(photoReader.Surface);
					}
				}

				var tcs = new TaskCompletionSource<CameraCaptureSession>();

				device.CreateCaptureSession(
					surfaces,
					new CameraCaptureStateListener()
					{
						OnConfigureFailedAction = session =>
						{
							tcs.SetResult(null);
							Element.RaiseMediaCaptureFailed("Failed to create captire sesstion");
						},
						OnConfiguredAction = session =>
						{
							tcs.SetResult(session);
						}
					},
					null);

				session = await tcs.Task;
				if (session != null)
				{
					UpdateRepeatingRequest();
				}
			}
			catch (Java.Lang.Exception error)
			{
				Available = false;
				LogError("Capture", error);
			}
			finally
			{
				Available = session != null;
				IsBusy = false;
			}
		}

		void CloseSession()
		{
			repeatingIsRunning = false;

			if (session == null)
				return;

			try
			{
				session.StopRepeating();
				session.AbortCaptures();
				session.Close();
				session.Dispose();
				session = null;
			}
			catch (CameraAccessException e)
			{
				LogError("Error camera access", e);
			}
			catch (Java.Lang.Exception e)
			{
				LogError("Error close device", e);
			}
		}

		public void UpdateRepeatingRequest()
		{
			if (session == null || sessionBuilder == null)
				return;

			IsBusy = true;
			try
			{
				if (repeatingIsRunning)
					session.StopRepeating();

				sessionBuilder.Set(CaptureRequest.ControlMode, (int)ControlMode.Auto);
				sessionBuilder.Set(CaptureRequest.ControlAeMode, (int)ControlAEMode.On);
				if (cameraTemplate == CameraTemplate.Record)
					sessionBuilder.Set(CaptureRequest.FlashMode, (int)flashMode);

				session.SetRepeatingRequest(sessionBuilder.Build(), null, backgroundHandler);
				repeatingIsRunning = true;
			}
			catch (Java.Lang.Exception error)
			{
				LogError("Update preview exception.", error);
			}
			finally
			{
				IsBusy = false;
			}
		}

		void CloseDevice(CameraDevice inputDevice)
		{
			if (inputDevice == device)
				CloseDevice();
		}

		public void CloseDevice()
		{
			try
			{
				DisposeMediaRecorder();
			}
			catch
			{
			}
			CloseSession();
			try
			{
				if (sessionBuilder != null)
				{
					sessionBuilder.Dispose();
					sessionBuilder = null;
				}

				if (device != null)
				{
					device.Close();
					device = null;
				}

				DisposeImageReader();
			}
			catch (Java.Lang.Exception e)
			{
				LogError("Error close device", e);
			}
		}

		void UpdateBackgroundColor()
		{
			View?.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
		}

		public void SetFlash()
		{
			if (!flashSupported)
				return;

			switch (Element.FlashMode)
			{
				default:
				case CameraFlashMode.On:
				case CameraFlashMode.Auto:
					flashMode = FlashMode.Single;
					break;
				case CameraFlashMode.Off:
					flashMode = FlashMode.Off;
					break;
				case CameraFlashMode.Torch:
					flashMode = FlashMode.Torch;
					break;
			}
		}

		public void SetVideoStabilization()
		{
			if (sessionBuilder == null || !stabilizationSupported)
			{
				sessionBuilder.Set(CaptureRequest.ControlVideoStabilizationMode,
					(int)(Element.VideoStabilization ? ControlVideoStabilizationMode.On : ControlVideoStabilizationMode.Off));
			}
		}

		public void ApplyZoom()
		{
			zoom = System.Math.Max(1f, System.Math.Min(Element.Zoom, maxDigitalZoom));
			if (ZoomSupported)
				sessionBuilder?.Set(CaptureRequest.ScalerCropRegion, GetZoomRect());
		}

		string GetCameraId()
		{
			var cameraIdList = Manager.GetCameraIdList();
			if (cameraIdList.Length == 0)
				return null;

			string FilterCameraByLens(LensFacing lensFacing)
			{
				foreach (var id in cameraIdList)
				{
					var characteristics = Manager.GetCameraCharacteristics(id);
					if (lensFacing == (LensFacing)(int)characteristics.Get(CameraCharacteristics.LensFacing))
						return id;
				}
				return null;
			}

			switch (Element.CameraOptions)
			{
				default:
				case CameraOptions.Default:
					return cameraIdList.Length != 0 ? cameraIdList[0] : null;
				case CameraOptions.Front:
					return FilterCameraByLens(LensFacing.Front);
				case CameraOptions.Back:
					return FilterCameraByLens(LensFacing.Back);
				case CameraOptions.External:
					return FilterCameraByLens(LensFacing.External);
			}
		}

		#region TextureView.ISurfaceTextureListener
		async void TextureView.ISurfaceTextureListener.OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
		{
			UpdateBackgroundColor();
			UpdateCaptureOptions();
			await RetrieveCameraDevice();
		}

		void TextureView.ISurfaceTextureListener.OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
		{
			ConfigureTransform(width, height);
		}

		bool TextureView.ISurfaceTextureListener.OnSurfaceTextureDestroyed(SurfaceTexture surface)
		{
			CloseDevice();
			return true;
		}

		void TextureView.ISurfaceTextureListener.OnSurfaceTextureUpdated(SurfaceTexture surface)
		{
		}
		#endregion

		#region Permissions
		async Task RequestCameraPermissions()
		{
			if (permissionsRequested != null)
				await permissionsRequested.Task;

			cameraPermissionsGranted = ContextCompat.CheckSelfPermission(Context, Manifest.Permission.Camera) == Permission.Granted;
			audioPermissionsGranted = ContextCompat.CheckSelfPermission(Context, Manifest.Permission.RecordAudio) == Permission.Granted;
			if (!cameraPermissionsGranted || !audioPermissionsGranted)
			{
				permissionsRequested = new TaskCompletionSource<bool>();
				RequestPermissions(new[] { Manifest.Permission.Camera, Manifest.Permission.RecordAudio }, requestCode: 1);
				await permissionsRequested.Task;
				permissionsRequested = null;
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			if (requestCode != 1)
			{
				base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
				return;
			}
			for (var i = 0; i < permissions.Length; i++)
			{
				if (permissions[i] == Manifest.Permission.Camera)
				{
					cameraPermissionsGranted = grantResults[i] == Permission.Granted;
					if (!cameraPermissionsGranted)
						Element.RaiseMediaCaptureFailed($"No permission to use the camera.");
				}
				else if (permissions[i] == Manifest.Permission.RecordAudio)
				{
					audioPermissionsGranted = grantResults[i] == Permission.Granted;
					if (!audioPermissionsGranted)
					{
						Element.RaiseMediaCaptureFailed($"No permission to record audio.");
					}
				}
			}
			permissionsRequested?.TrySetResult(true);
		}
		#endregion

		#region Helpers
		void LogError(string desc, Java.Lang.Exception ex = null)
		{
			var newLine = System.Environment.NewLine;
			var sb = new StringBuilder(desc);
			if (ex != null)
			{
				sb.Append($"{newLine}ErrorMessage: {ex.Message}{newLine}Stacktrace: {ex.StackTrace}");
				ex.PrintStackTrace();
			}
			Log.Warning("CameraView", sb.ToString());
		}

		void DisposeMediaRecorder()
		{
			if (mediaRecorder != null)
			{
				if (IsRecordingVideo)
				{
					mediaRecorder.Stop();
					mediaRecorder.Reset();
				}
				mediaRecorder.Release();
				mediaRecorder.Dispose();
				mediaRecorder = null;
			}
			IsRecordingVideo = false;
		}

		void DisposeImageReader()
		{
			if (photoReader != null)
			{
				photoReader.Close();
				photoReader.Dispose();
				photoReader = null;
			}
		}

		protected override void Dispose(bool disposing)
		{
			CloseDevice();
			base.Dispose(disposing);
		}

		string ConstructMediaFilename(string prefix, string extension)
		{
			// "To improve user privacy, direct access to shared/external storage devices is deprecated"
			// Env.GetExternalStoragePublicDirectory(Env.DirectoryDcim).AbsolutePath
			var path = Context.GetExternalFilesDir(Env.DirectoryDcim).AbsolutePath;
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			var fileName = DateTime.Now.ToString("yyyyddMM_HHmmss");
			if (!string.IsNullOrEmpty(prefix))
				fileName = $"{prefix}_{fileName}";
			return System.IO.Path.Combine(path, $"{fileName}.{extension}");
		}

		Rect GetZoomRect()
		{
			if (activeRect == null)
				return null;
			var width = activeRect.Width();
			var heigth = activeRect.Height();
			var newWidth = (int)(width / zoom);
			var newHeight = (int)(heigth / zoom);
			var x = (width - newWidth) / 2;
			var y = (heigth - newHeight) / 2;
			return new Rect(x, y, x + newWidth, y + newHeight);
		}

		SurfaceOrientation GetDisplayRotation()
			=> App.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>().DefaultDisplay.Rotation;

		int GetDisplayRotationiDegress()
		{
			switch (GetDisplayRotation())
			{
				case SurfaceOrientation.Rotation90:
					return 90;
				case SurfaceOrientation.Rotation180:
					return 180;
				case SurfaceOrientation.Rotation270:
					return 270;
				default:
					return 0;
			}
		}

		public bool KeepScreenOn
		{
			get => texture.KeepScreenOn;
			set => texture.KeepScreenOn = value;
		}

		int GetPreviewOrientation()
		{
			switch (GetDisplayRotation())
			{
				case SurfaceOrientation.Rotation90:
					return 270;
				case SurfaceOrientation.Rotation180:
					return 180;
				case SurfaceOrientation.Rotation270:
					return 90;
				default:
					return 0;
			}
		}

		public void ConfigureTransform()
		{
			ConfigureTransform(texture.Width, texture.Height);
		}

		void ConfigureTransform(int viewWidth, int viewHeight)
		{
			if (texture == null || previewSize == null || previewSize.Width == 0 || previewSize.Height == 0)
				return;

			var matrix = new Matrix();
			var viewRect = new RectF(0, 0, viewWidth, viewHeight);
			var bufferRect = new RectF(0, 0, previewSize.Height, previewSize.Width);
			var centerX = viewRect.CenterX();
			var centerY = viewRect.CenterY();
			bufferRect.Offset(centerX - bufferRect.CenterX(), centerY - bufferRect.CenterY());

			var mirror = true; // Element.CameraOptions == CameraOptions.Front && Element.OnThisPlatform().GetMirrorFrontPreview();
			matrix.SetRectToRect(viewRect, bufferRect, Matrix.ScaleToFit.Fill);
			float scaleHH() => (float)viewHeight / previewSize.Height;
			float scaleHW() => (float)viewHeight / previewSize.Width;
			float scaleWW() => (float)viewWidth / previewSize.Width;
			float scaleWH() => (float)viewWidth / previewSize.Height;
			float sx, sy;

			switch (Element.PreviewAspect)
			{
				default:
				case Aspect.AspectFit:
					sx = sy = System.Math.Min(scaleHH(), scaleHW());
					break;
				case Aspect.AspectFill:
					sx = sy = System.Math.Max(scaleHH(), scaleHW());
					break;
				case Aspect.Fill:
					if (Resources.Configuration.Orientation == AOrientation.Landscape)
					{
						sx = scaleWW();
						sy = scaleHH();
					}
					else
					{
						sx = scaleWH();
						sy = scaleHW();
					}
					break;
			}

			matrix.PostScale(mirror ? -sx : sx, sy, centerX, centerY);
			matrix.PostRotate(GetCaptureOrientation(), centerX, centerY);
			texture.SetTransform(matrix);
		}

		int GetCaptureOrientation()
		{
			var frontOffset = cameraType == LensFacing.Front ? 90 : -90;
			return (360 + sensorOrientation - GetDisplayRotationiDegress() + frontOffset) % 360;
		}

		void Sound(MediaActionSoundType soundType)
		{
			if (UseSystemSound)
				mediaSound.Play(soundType);
		}

		ASize GetMaxSize(ASize[] ImageSizes)
		{
			ASize maxSize = null;
			long maxPixels = 0;
			for (var i = 0; i < ImageSizes.Length; i++)
			{
				long currentPixels = ImageSizes[i].Width * ImageSizes[i].Height;
				if (currentPixels > maxPixels)
				{
					maxSize = ImageSizes[i];
					maxPixels = currentPixels;
				}
			}
			return maxSize;
		}

		// chooses the smallest one whose width and height are at least as large as the respective requested values
		ASize ChooseOptimalSize(ASize[] choices, int width, int height, ASize aspectRatio)
		{
			var bigEnough = new List<ASize>();
			var w = aspectRatio.Width;
			var h = aspectRatio.Height;
			foreach (var option in choices)
			{
				if (option.Height == option.Width * h / w &&
						option.Width >= width && option.Height >= height)
				{
					bigEnough.Add(option);
				}
			}
			// Pick the smallest of those, assuming we found any
			if (bigEnough.Count > 0)
			{
				var minArea = bigEnough.Min(s => s.Width * s.Height);
				return bigEnough.First(s => s.Width * s.Height == minArea);
			}
			else
			{
				LogError("Couldn't find any suitable preview size");
				return choices[0];
			}
		}
		#endregion
	}
}