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
using Camera = Android.Hardware.Camera;
using Math = System.Math;
using Rect = Android.Graphics.Rect;
using APoint = Android.Graphics.Point;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraFragment : Fragment, TextureView.ISurfaceTextureListener
	{
		// Max preview width that is guaranteed by Camera2 API
		const int MAX_PREVIEW_HEIGHT = 1080;
		// Max preview height that is guaranteed by Camera2 API
		const int MAX_PREVIEW_WIDTH = 1920;

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

		public CameraFragment()
		{
		}

		public CameraFragment(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
		}

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

		public override AView OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) =>
			inflater.Inflate(Resource.Layout.CameraFragment, null);

		public override void OnViewCreated(AView view, Bundle savedInstanceState) =>
			texture = view.FindViewById<AutoFitTextureView>(Resource.Id.cameratexture);

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
				texture.SurfaceTextureListener = this;
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

				// _texture.ClearCanvas(Element.BackgroundColor.ToAndroid()); // HANG after select valid camera...
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
					sensorOrientation = (int)characteristics.Get(CameraCharacteristics.SensorOrientation);

					var displaySize = new APoint();
					Activity.WindowManager.DefaultDisplay.GetSize(displaySize);
					var rotatedViewWidth = texture.Width;
					var rotatedViewHeight = texture.Height;
					var maxPreviewWidth = displaySize.X;
					var maxPreviewHeight = displaySize.Y;

					if (sensorOrientation == 90 || sensorOrientation == 270)
					{
						rotatedViewWidth = texture.Height;
						rotatedViewHeight = texture.Width;
						maxPreviewWidth = displaySize.Y;
						maxPreviewHeight = displaySize.X;
					}

					if (maxPreviewHeight > MAX_PREVIEW_HEIGHT)
					{
						maxPreviewHeight = MAX_PREVIEW_HEIGHT;
					}

					if (maxPreviewWidth > MAX_PREVIEW_WIDTH)
					{
						maxPreviewWidth = MAX_PREVIEW_WIDTH;
					}

					photoSize = GetMaxSize(map.GetOutputSizes((int)ImageFormatType.Jpeg));
					videoSize = GetMaxSize(map.GetOutputSizes(Class.FromType(typeof(MediaRecorder))));
					previewSize = ChooseOptimalSize(
						map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture))),
						rotatedViewWidth,
						rotatedViewHeight,
						maxPreviewWidth,
						maxPreviewHeight,
						cameraTemplate == CameraTemplate.Record ? videoSize : photoSize);
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
							OnOpenedAction = device => initTaskSource?.TrySetResult(device),
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
						await PrepareSession();
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
			switch (Element.CaptureMode)
			{
				default:
				case CameraCaptureMode.Photo:
					cameraTemplate = CameraTemplate.Preview;
					break;
				case CameraCaptureMode.Video:
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
					/*sessionBuilder.Set(CaptureRequest.JpegOrientation, GetJpegOrientation());*/
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

		void OnPhoto(object sender, Tuple<string, byte[], int> tuple) =>
			Device.BeginInvokeOnMainThread(() =>
				Element?.RaiseMediaCaptured(new MediaCapturedEventArgs(tuple.Item1, tuple.Item2, tuple.Item3)));

		void OnVideo(object sender, string path) =>
			Device.BeginInvokeOnMainThread(() =>
				Element?.RaiseMediaCaptured(new MediaCapturedEventArgs(path)));

		void SetupImageReader()
		{
			DisposeImageReader();

			photoReader = ImageReader.NewInstance(photoSize.Width, photoSize.Height, ImageFormatType.Jpeg, maxImages: 1);

			var readerListener = new ImageAvailableListener();
			readerListener.Photo += (_, bytes) =>
			{
				string filePath = null;

				// Calculate image rotation based on sensor and device orientation
				var rotation = GetRotationCompensation();

				// See TODO on CameraView.SavePhotoToFile
				// Insert Exif information to jpeg file
				/*if (Element.SavePhotoToFile)
				{
					filePath = ConstructMediaFilename(null, extension: "jpg");
					File.WriteAllBytes(filePath, bytes);
				}
				Sound(MediaActionSoundType.ShutterClick);
				OnPhoto(this, new Tuple<string, byte[]>(filePath, Element.SavePhotoToFile ? null : bytes));*/

				Sound(MediaActionSoundType.ShutterClick);
				OnPhoto(this, new Tuple<string, byte[], int>(filePath, bytes, rotation));
			};

			photoReader.SetOnImageAvailableListener(readerListener, backgroundHandler);
		}

		private int GetRotationCompensation()
		{
			var rotationCompensation = GetDisplayRotationDegrees();
			var c = Manager.GetCameraCharacteristics(cameraId);
			// Get the device's sensor orientation.
			var sensorOrientation = (int)c.Get(CameraCharacteristics.SensorOrientation);

			var facingFront = ((Integer)c.Get(CameraCharacteristics.LensFacing)).IntValue() == (int)LensFacing.Front;
			if (facingFront)
			{
				rotationCompensation = (sensorOrientation + rotationCompensation) % 360;
			}
			else
			{
				rotationCompensation = (sensorOrientation - rotationCompensation + 360) % 360;
			}

			return rotationCompensation;
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
				mediaRecorder.SetProfile(profile);
			else
			{
				mediaRecorder.SetOutputFormat(OutputFormat.Mpeg4);
				mediaRecorder.SetVideoEncodingBitRate(10000000);
				mediaRecorder.SetVideoFrameRate(30);
				mediaRecorder.SetVideoSize(videoSize.Width, videoSize.Height);
				mediaRecorder.SetVideoEncoder(VideoEncoder.H264);

				if (audioPermissionsGranted)
					mediaRecorder.SetAudioEncoder(AudioEncoder.Default);
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
				return;
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
						var mediaSurface = mediaRecorder.Surface;
						surfaces.Add(mediaSurface);
						sessionBuilder.AddTarget(mediaSurface);
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
						OnConfigureFailedAction = captureSession =>
						{
							tcs.SetResult(null);
							Element.RaiseMediaCaptureFailed("Failed to create captire sesstion");
						},
						OnConfiguredAction = captureSession => tcs.SetResult(captureSession)
					},
					null);

				session = await tcs.Task;
				if (session != null)
					UpdateRepeatingRequest();
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

				session.SetRepeatingRequest(sessionBuilder.Build(), listener: null, backgroundHandler);
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
			catch (Java.Lang.Exception e)
			{
				LogError("Error close device", e);
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

		void UpdateBackgroundColor() =>
			View?.SetBackgroundColor(Element.BackgroundColor.ToAndroid());

		public void SetFlash()
		{
			if (!flashSupported)
				return;

			flashMode = Element.FlashMode switch
			{
				CameraFlashMode.Off => FlashMode.Off,
				CameraFlashMode.Torch => FlashMode.Torch,
				_ => FlashMode.Single,
			};
		}

		public void SetVideoStabilization()
		{
			if (sessionBuilder == null || !stabilizationSupported)
				return;
			sessionBuilder.Set(CaptureRequest.ControlVideoStabilizationMode,
				(int)(Element.VideoStabilization ? ControlVideoStabilizationMode.On : ControlVideoStabilizationMode.Off));
		}

		public void ApplyZoom()
		{
			zoom = (float)System.Math.Max(1f, System.Math.Min(Element.Zoom, maxDigitalZoom));
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

			return Element.CameraOptions switch
			{
				CameraOptions.Front => FilterCameraByLens(LensFacing.Front),
				CameraOptions.Back => FilterCameraByLens(LensFacing.Back),
				CameraOptions.External => FilterCameraByLens(LensFacing.External),
				_ => cameraIdList.Length != 0 ? cameraIdList[0] : null,
			};
		}

		#region TextureView.ISurfaceTextureListener
		async void TextureView.ISurfaceTextureListener.OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
		{
			UpdateBackgroundColor();
			UpdateCaptureOptions();
			await RetrieveCameraDevice();
		}

		void TextureView.ISurfaceTextureListener.OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height) =>
			ConfigureTransform(width, height);

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

			var permissionsToRequest = new List<string>();
			cameraPermissionsGranted = ContextCompat.CheckSelfPermission(Context, Manifest.Permission.Camera) == Permission.Granted;
			if (!cameraPermissionsGranted)
				permissionsToRequest.Add(Manifest.Permission.Camera);
			if (Element.CaptureMode == CameraCaptureMode.Video)
			{
				audioPermissionsGranted = ContextCompat.CheckSelfPermission(Context, Manifest.Permission.RecordAudio) == Permission.Granted;
				if (!audioPermissionsGranted)
					permissionsToRequest.Add(Manifest.Permission.RecordAudio);
			}
			if (permissionsToRequest.Count > 0)
			{
				permissionsRequested = new TaskCompletionSource<bool>();
				RequestPermissions(permissionsToRequest.ToArray(), requestCode: 1);
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

		int GetDisplayRotationDegrees() =>
			GetDisplayRotation() switch
			{
				SurfaceOrientation.Rotation90 => 90,
				SurfaceOrientation.Rotation180 => 180,
				SurfaceOrientation.Rotation270 => 270,
				_ => 0,
			};

		int GetJpegRotationDegrees() =>
			GetDisplayRotation() switch
			{
				SurfaceOrientation.Rotation90 => 0,
				SurfaceOrientation.Rotation180 => 270,
				SurfaceOrientation.Rotation270 => 180,
				_ => 90,
			};

		int GetPreviewOrientation() =>
			GetDisplayRotation() switch
			{
				SurfaceOrientation.Rotation90 => 270,
				SurfaceOrientation.Rotation180 => 180,
				SurfaceOrientation.Rotation270 => 90,
				_ => 0,
			};

		public void ConfigureTransform() =>
			ConfigureTransform(texture.Width, texture.Height);

		void ConfigureTransform(int viewWidth, int viewHeight)
		{
			var activity = Activity;

			if (texture == null || previewSize == null || activity == null)
				return;

			var rotation = (int)activity.WindowManager.DefaultDisplay.Rotation;
			var matrix = new Matrix();
			var viewRect = new RectF(0, 0, viewWidth, viewHeight);
			var bufferRect = new RectF(0, 0, previewSize.Height, previewSize.Width);
			var centerX = viewRect.CenterX();
			var centerY = viewRect.CenterY();

			if (rotation == (int)SurfaceOrientation.Rotation90 || rotation == (int)SurfaceOrientation.Rotation270)
			{
				bufferRect.Offset(centerX - bufferRect.CenterX(), centerY - bufferRect.CenterY());
				matrix.SetRectToRect(viewRect, bufferRect, Matrix.ScaleToFit.Fill);
				var scale = System.Math.Max((float)viewHeight / previewSize.Height, (float)viewWidth / previewSize.Width);
				matrix.PostScale(scale, scale, centerX, centerY);
				matrix.PostRotate(90 * (rotation - 2), centerX, centerY);
			}
			else if (rotation == (int)SurfaceOrientation.Rotation180)
				matrix.PostRotate(180, centerX, centerY);

			texture.SetTransform(matrix);
		}

		int GetCaptureOrientation()
		{
			var frontOffset = cameraType == LensFacing.Front ? 90 : -90;
			return (360 + sensorOrientation - GetDisplayRotationDegrees() + frontOffset) % 360;
		}

		int GetJpegOrientation() => (sensorOrientation + GetJpegRotationDegrees() + 270) % 360;

		void Sound(MediaActionSoundType soundType)
		{
			if (UseSystemSound)
				mediaSound.Play(soundType);
		}

		ASize GetMaxSize(ASize[] imageSizes)
		{
			ASize maxSize = null;
			long maxPixels = 0;
			for (var i = 0; i < imageSizes.Length; i++)
			{
				long currentPixels = imageSizes[i].Width * imageSizes[i].Height;
				if (currentPixels > maxPixels)
				{
					maxSize = imageSizes[i];
					maxPixels = currentPixels;
				}
			}
			return maxSize;
		}

		// chooses the smallest one whose width and height are at least as large as the respective requested values
		ASize ChooseOptimalSize(ASize[] choices, int width, int height, int maxWidth, int maxHeight, ASize aspectRatio)
		{
			var bigEnough = new List<ASize>();
			var notBigEnough = new List<ASize>();
			var w = aspectRatio.Width;
			var h = aspectRatio.Height;
			foreach (var option in choices)
			{
				if (option.Width <= maxWidth && option.Height <= maxHeight &&
					option.Height == option.Width * h / w)
				{
					if (option.Width >= width && option.Height >= height)
					{
						bigEnough.Add(option);
					}
					else
					{
						notBigEnough.Add(option);
					}
				}
			}

			// Pick the smallest of those, assuming we found any
			if (bigEnough.Count > 0)
			{
				var minArea = bigEnough.Min(s => s.Width * s.Height);
				return bigEnough.First(s => s.Width * s.Height == minArea);
			}

			if (notBigEnough.Count > 0)
			{
				var maxArea = notBigEnough.Max(s => s.Height * s.Width);
				return notBigEnough.First(s => s.Height * s.Width == maxArea);
			}

			LogError("Couldn't find any suitable preview size");
			return choices[0];
		}
		#endregion
	}
}