using System;
using System.Collections.Generic;
using System.Text;
using AContent = Android.Content;
using AUtil = Android.Util;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.OS;
using Android.Views;
using Java.Lang;
using Android.Hardware.Camera2.Params;
using Android.Media;
using Java.Util.Concurrent;
using Android;
using Android.Content.PM;
using System.Threading.Tasks;
using Android.Util;
using Java.Util;
using Env = Android.OS.Environment;
using AOrientation = Android.Content.Res.Orientation;
using Android.Provider;
using System.IO;
#if __ANDROID_29__
using AndroidX.Core.Content;
using AndroidX.Fragment.App;
#else
using Android.Support.V4.Content;
using Android.Support.V4.App;
#endif

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraDroid : Fragment
	{
		public const int STATE_PREVIEW = 0;

		// Camera state: Waiting for the focus to be locked.
		public const int STATE_WAITING_LOCK = 1;

		// Camera state: Waiting for the exposure to be precapture state.
		public const int STATE_WAITING_PRECAPTURE = 2;

		// Camera state: Waiting for the exposure state to be something other than precapture.
		public const int STATE_WAITING_NON_PRECAPTURE = 3;

		// Camera state: Picture was taken.
		public const int STATE_PICTURE_TAKEN = 4;


		// Max preview width that is guaranteed by Camera2 API
		const int MAX_PREVIEW_WIDTH = 1920;

		// Max preview height that is guaranteed by Camera2 API
		const int MAX_PREVIEW_HEIGHT = 1080;
		const string cameraBackground = "CameraBackground";

		static readonly SparseIntArray ORIENTATIONS = new SparseIntArray();
		public AutoFitTextureView texture;
		CameraStateListener mStateCallback;
		HandlerThread mBackgroundThread;
		public Handler mBackgroundHandler;
		CameraSurfaceTextureListener surfaceTextureListener;
		public CameraDevice mCameraDevice;
		ImageReader mImageReader;
		public CameraCaptureListener mCaptureCallback;

		public CameraCaptureSession mCaptureSession;
		public Semaphore mCameraOpenCloseLock = new Semaphore(1);
		ImageAvailableListener mOnImageAvailableListener;
		int mSensorOrientation;
		bool mFlashSupported;
		string mCameraId;
		Size previewSize;
		public CaptureRequest mPreviewRequest;

		public CaptureRequest.Builder mPreviewRequestBuilder;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) =>
			inflater.Inflate(Resource.Layout.CameraFragment, null);

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			texture = view.FindViewById<AutoFitTextureView>(Resource.Id.cameratexture);
			surfaceTextureListener = new CameraSurfaceTextureListener(this);
			mStateCallback = new CameraStateListener(this);
			mCaptureCallback = new CameraCaptureListener(this);
			mOnImageAvailableListener = new ImageAvailableListener
			{
				OnPhotoReady = OnPhotoReady
			};

			// fill ORIENTATIONS list
			ORIENTATIONS.Append((int)SurfaceOrientation.Rotation0, 180);
			ORIENTATIONS.Append((int)SurfaceOrientation.Rotation90, 270);
			ORIENTATIONS.Append((int)SurfaceOrientation.Rotation180, 0);
			ORIENTATIONS.Append((int)SurfaceOrientation.Rotation270, 90);
		}

		async void OnPhotoReady(byte[] bytes)
		{
			var filePath = ConstructMediaFilename(null, extension: "jpg");
			File.WriteAllBytes(filePath, bytes);
			await FixOrientation(filePath);
			Element.RaiseMediaCaptured(new MediaCapturedEventArgs(filePath, bytes));

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

			static Task FixOrientation(string filePath)
			{
				if (string.IsNullOrEmpty(filePath))
					return Task.CompletedTask;

				var originalMetadata = new ExifInterface(filePath);

				return Task.Run(() =>
				{
					var rotation = GetRotation(originalMetadata);
					if (rotation == 0)
						return;

					var originalImage = BitmapFactory.DecodeFile(filePath);
					var matrix = new Matrix();
					matrix.PostRotate(rotation);
					using var rotatedImage = Bitmap.CreateBitmap(originalImage);
					using var stream = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite);
					rotatedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);

					stream.Close();
					rotatedImage.Recycle();
					originalMetadata.SetAttribute(ExifInterface.TagOrientation, Java.Lang.Integer.ToString((int)Orientation.Normal));

					GC.Collect();
				});
			}
		}

		static int GetRotation(ExifInterface exif)
		{
			if (exif == null)
				return 0;
			try
			{
				var orientation = (Orientation)exif.GetAttributeInt(ExifInterface.TagOrientation, (int)Orientation.Normal);

				return orientation switch
				{
					Orientation.Rotate90 => 90,
					Orientation.Rotate180 => 180,
					Orientation.Rotate270 => 270,
					_ => 0,
				};
			}
			catch (System.Exception ex)
			{
#if DEBUG
				throw ex;
#else
                return 0;
#endif
			}
		}

		public override void OnResume()
		{
			base.OnResume();
			StartBackgroundThread();

			if (texture.IsAvailable)
				OpenCamera(texture.Width, texture.Height);
			else
				texture.SurfaceTextureListener = surfaceTextureListener;
		}

		public override void OnPause()
		{
			StopBackgroundThread();
			CloseCamera();
			base.OnPause();
		}

		public void CloseCamera()
		{
			try
			{
				mCameraOpenCloseLock.Acquire();
				if (mCaptureSession != null)
				{
					mCaptureSession.Close();
					mCaptureSession = null;
				}
				if (mCameraDevice != null)
				{
					mCameraDevice.Close();
					mCameraDevice = null;
				}
				if (mImageReader != null)
				{
					mImageReader.Close();
					mImageReader = null;
				}
			}
			catch (InterruptedException e)
			{
				e.PrintStackTrace();
			}
			finally
			{
				mCameraOpenCloseLock.Release();
			}
		}

		void StopBackgroundThread()
		{
			if (mBackgroundThread == null)
				return;

			mBackgroundThread.QuitSafely();
			try
			{
				mBackgroundThread.Join();
				mBackgroundThread = null;
				mBackgroundHandler = null;
			}
			catch (InterruptedException e)
			{
				e.PrintStackTrace();
			}
		}

		public async void OpenCamera(int width, int height)
		{
			if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.Camera) != Permission.Granted)
			{
				await RequestCameraPermissions().ConfigureAwait(false);
				return;
			}

			SetUpCameraOutputs(width, height);
			ConfigureTransform(width, height);
			var activity = Activity;
			var manager = (CameraManager)activity.GetSystemService(AContent.Context.CameraService);
			try
			{
				if (!mCameraOpenCloseLock.TryAcquire(2500, TimeUnit.Milliseconds))
					throw new RuntimeException("Time out waiting to lock camera opening.");

				manager.OpenCamera(mCameraId, mStateCallback, mBackgroundHandler);
			}
			catch (CameraAccessException e)
			{
				e.PrintStackTrace();
			}
			catch (InterruptedException e)
			{
				throw new RuntimeException("Interrupted while trying to lock camera opening.", e);
			}
		}

		public void ConfigureTransform(int viewWidth, int viewHeight)
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

		public void SetUpCameraOutputs(int width, int height)
		{
			var activity = Activity;
			var manager = (CameraManager)activity.GetSystemService(AContent.Context.CameraService);
			try
			{
				var cameraList = manager.GetCameraIdList();
				var cameraIdListCount = cameraList.Length;

				for (var i = 0; i < cameraIdListCount; i++)
				{
					var cameraId = cameraList[i];
					var characteristics = manager.GetCameraCharacteristics(cameraId);

					// We don't use a front facing camera in this sample.
					var facing = (Integer)characteristics.Get(CameraCharacteristics.LensFacing);
					if (facing != null && facing == Integer.ValueOf((int)LensFacing.Front))
						continue;

					var map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
					if (map == null)
						continue;

					// For still image captures, we use the largest available size.
					var largest = (Size)Collections.Max(Arrays.AsList(map.GetOutputSizes((int)ImageFormatType.Jpeg)), new CompareSizesByArea());
					mImageReader = ImageReader.NewInstance(largest.Width, largest.Height, ImageFormatType.Jpeg, 2);
					mImageReader.SetOnImageAvailableListener(mOnImageAvailableListener, mBackgroundHandler);

					// Find out if we need to swap dimension to get the preview size relative to sensor
					// coordinate.
					var displayRotation = activity.WindowManager.DefaultDisplay.Rotation;

					// noinspection ConstantConditions
					mSensorOrientation = (int)characteristics.Get(CameraCharacteristics.SensorOrientation);
					var swappedDimensions = false;
					switch (displayRotation)
					{
						case SurfaceOrientation.Rotation0:
						case SurfaceOrientation.Rotation180:
							if (mSensorOrientation == 90 || mSensorOrientation == 270)
								swappedDimensions = true;
							break;
						case SurfaceOrientation.Rotation90:
						case SurfaceOrientation.Rotation270:
							if (mSensorOrientation == 0 || mSensorOrientation == 180)
								swappedDimensions = true;
							break;
						default:
							// Log.Error(TAG, "Display rotation is invalid: " + displayRotation);
							break;
					}

					var displaySize = new Point();
					activity.WindowManager.DefaultDisplay.GetSize(displaySize);
					var rotatedPreviewWidth = width;
					var rotatedPreviewHeight = height;
					var maxPreviewWidth = displaySize.X;
					var maxPreviewHeight = displaySize.Y;

					if (swappedDimensions)
					{
						rotatedPreviewWidth = height;
						rotatedPreviewHeight = width;
						maxPreviewWidth = displaySize.Y;
						maxPreviewHeight = displaySize.X;
					}

					if (maxPreviewWidth > MAX_PREVIEW_WIDTH)
						maxPreviewWidth = MAX_PREVIEW_WIDTH;

					if (maxPreviewHeight > MAX_PREVIEW_HEIGHT)
						maxPreviewHeight = MAX_PREVIEW_HEIGHT;

					// Danger, W.R.! Attempting to use too large a preview size could  exceed the camera
					// bus' bandwidth limitation, resulting in gorgeous previews but the storage of
					// garbage capture data.
					previewSize = ChooseOptimalSize(map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture))),
						rotatedPreviewWidth, rotatedPreviewHeight, maxPreviewWidth,
						maxPreviewHeight, largest);

					// We fit the aspect ratio of TextureView to the size of preview we picked.
					var orientation = Resources.Configuration.Orientation;
					if (orientation == AOrientation.Landscape)
						texture.SetAspectRatio(previewSize.Width, previewSize.Height);
					else
						texture.SetAspectRatio(previewSize.Height, previewSize.Width);

					// Check if the flash is supported.
					var available = (Java.Lang.Boolean)characteristics.Get(CameraCharacteristics.FlashInfoAvailable);

					mFlashSupported = available != null && (bool)available;

					mCameraId = cameraId;
					return;
				}
			}
			catch (CameraAccessException e)
			{
				e.PrintStackTrace();
			}
			catch (NullPointerException e)
			{
				// Currently an NPE is thrown when the Camera2API is used but not supported on the
				// device this code runs.
				e.PrintStackTrace();
			}
		}

		Size ChooseOptimalSize(Size[] choices, int textureViewWidth, int textureViewHeight, int maxWidth, int maxHeight, Size aspectRatio)
		{
			var choicesLength = choices.Length;

			// Collect the supported resolutions that are at least as big as the preview Surface
			var bigEnough = new List<Size>(choicesLength);

			// Collect the supported resolutions that are smaller than the preview Surface
			var notBigEnough = new List<Size>(choicesLength);
			var w = aspectRatio.Width;
			var h = aspectRatio.Height;

			for (var i = 0; i < choicesLength; i++)
			{
				var option = choices[i];
				if ((option.Width <= maxWidth) && (option.Height <= maxHeight) &&
					   option.Height == option.Width * h / w)
				{
					if (option.Width >= textureViewWidth &&
						option.Height >= textureViewHeight)
					{
						bigEnough.Add(option);
					}
					else
						notBigEnough.Add(option);
				}
			}

			// Pick the smallest of those big enough. If there is no one big enough, pick the
			// largest of those not big enough.
			if (bigEnough.Count > 0)
				return (Size)Collections.Min(bigEnough, new CompareSizesByArea());
			if (notBigEnough.Count > 0)
				return (Size)Collections.Max(notBigEnough, new CompareSizesByArea());
			else
				return choices[0]; // Couldn't find any suitable preview size
		}

		TaskCompletionSource<bool> permissionsRequested;

		bool audioPermissionsGranted;
		bool cameraPermissionsGranted;
		public int mState;

		public CameraView Element { get; set; }

		async Task RequestCameraPermissions()
		{
			if (permissionsRequested != null)
				await permissionsRequested.Task;

			var permissionsToRequest = new List<string>();
			cameraPermissionsGranted = ContextCompat.CheckSelfPermission(Context, Manifest.Permission.Camera) == Permission.Granted;
			if (!cameraPermissionsGranted)
				permissionsToRequest.Add(Manifest.Permission.Camera);
			if (Element.CaptureOptions == CameraCaptureOptions.Video)
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

		void StartBackgroundThread()
		{
			mBackgroundThread = new HandlerThread(cameraBackground);
			mBackgroundThread.Start();
			mBackgroundHandler = new Handler(mBackgroundThread.Looper);
		}

		public void CreateCameraPreviewSession()
		{
			try
			{
				var surfaceTexture = texture.SurfaceTexture;
				if (surfaceTexture == null)
				{
					throw new IllegalStateException("texture is null");
				}

				// We configure the size of default buffer to be the size of camera preview we want.
				surfaceTexture.SetDefaultBufferSize(previewSize.Width, previewSize.Height);

				// This is the output Surface we need to start preview.
				var surface = new Surface(surfaceTexture);

				// We set up a CaptureRequest.Builder with the output Surface.
				mPreviewRequestBuilder = mCameraDevice.CreateCaptureRequest(CameraTemplate.Preview);
				mPreviewRequestBuilder.AddTarget(surface);

				// Here, we create a CameraCaptureSession for camera preview.
				var surfaces = new List<Surface>
				{
					surface,
					mImageReader.Surface
				};

				mCameraDevice.CreateCaptureSession(surfaces, new CameraCaptureSessionCallback(this), null);
			}
			catch (CameraAccessException e)
			{
				e.PrintStackTrace();
			}
		}

		public void SetAutoFlash(CaptureRequest.Builder builder)
		{
			if (mFlashSupported)
				builder.Set(CaptureRequest.ControlAeMode, (int)ControlAEMode.OnAutoFlash);
		}

		public void TakePhoto()
		{
			try
			{
				mPreviewRequestBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Start);

				mState = STATE_WAITING_LOCK;
				mCaptureSession.Capture(mPreviewRequestBuilder.Build(), mCaptureCallback, mBackgroundHandler);
			}
			catch (CameraAccessException e)
			{
				e.PrintStackTrace();
			}
		}

		CaptureRequest.Builder stillCaptureBuilder;

		public void CaptureStillPicture()
		{
			try
			{
				var activity = Activity;

				if (activity == null || mCameraDevice == null)
					return;

				if (stillCaptureBuilder == null)
					stillCaptureBuilder = mCameraDevice.CreateCaptureRequest(CameraTemplate.StillCapture);

				stillCaptureBuilder.AddTarget(mImageReader.Surface);

				stillCaptureBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.ContinuousPicture);
				SetAutoFlash(stillCaptureBuilder);

				var rotation = (int)activity.WindowManager.DefaultDisplay.Rotation;
				stillCaptureBuilder.Set(CaptureRequest.JpegOrientation, GetOrientation(rotation));

				mCaptureSession.StopRepeating();
				mCaptureSession.Capture(stillCaptureBuilder.Build(), new CameraCaptureStillPictureSessionCallback(this), null);
			}
			catch (CameraAccessException e)
			{
				e.PrintStackTrace();
			}
		}

		int GetOrientation(int rotation)
		{
			// Sensor orientation is 90 for most devices, or 270 for some devices (eg. Nexus 5X)
			// We have to take that into account and rotate JPEG properly.
			// For devices with orientation of 90, we simply return our mapping from ORIENTATIONS.
			// For devices with orientation of 270, we need to rotate the JPEG 180 degrees.
			return (ORIENTATIONS.Get(rotation) + mSensorOrientation + 270) % 360;
		}

		public void UnlockFocus()
		{
			try
			{
				mPreviewRequestBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Cancel);
				SetAutoFlash(mPreviewRequestBuilder);
				mCaptureSession.Capture(mPreviewRequestBuilder.Build(), mCaptureCallback, mBackgroundHandler);

				mState = STATE_PREVIEW;
				mCaptureSession.SetRepeatingRequest(mPreviewRequest, mCaptureCallback, mBackgroundHandler);
			}
			catch (CameraAccessException e)
			{
				e.PrintStackTrace();
			}
		}

		public void RunPrecaptureSequence()
		{
			try
			{
				mPreviewRequestBuilder.Set(CaptureRequest.ControlAePrecaptureTrigger, (int)ControlAEPrecaptureTrigger.Start);

				mState = STATE_WAITING_PRECAPTURE;
				mCaptureSession.Capture(mPreviewRequestBuilder.Build(), mCaptureCallback, mBackgroundHandler);
			}
			catch (CameraAccessException e)
			{
				e.PrintStackTrace();
			}
		}

		class CompareSizesByArea : Java.Lang.Object, IComparator
		{
			public int Compare(Java.Lang.Object lhs, Java.Lang.Object rhs)
			{
				var lhsSize = (Size)lhs;
				var rhsSize = (Size)rhs;

				// We cast here to ensure the multiplications won't overflow
				return Long.Signum(((long)lhsSize.Width * lhsSize.Height) - ((long)rhsSize.Width * rhsSize.Height));
			}
		}
	}
}
