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

using AOrientation = Android.Content.Res.Orientation;
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
		// Max preview width that is guaranteed by Camera2 API
		const int MAX_PREVIEW_WIDTH = 1920;

		// Max preview height that is guaranteed by Camera2 API
		const int MAX_PREVIEW_HEIGHT = 1080;
		const string cameraBackground = "CameraBackground";

		AutoFitTextureView texture;
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
			inflater.Inflate(Resource.Layout.CameraFragment, container, false);

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			texture = view.FindViewById<AutoFitTextureView>(Resource.Id.cameratexture);
			surfaceTextureListener = new CameraSurfaceTextureListener(this);
			mStateCallback = new CameraStateListener(this);
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

		void SetUpCameraOutputs(int width, int height)
		{
			var activity = Activity;
			var manager = (CameraManager)activity.GetSystemService(AContent.Context.CameraService);
			try
			{
				var cameraIdListCount = manager.GetCameraIdList().Length;
				for (var i = 0; i < cameraIdListCount; i++)
				{
					var cameraId = manager.GetCameraIdList()[i];
					var characteristics = manager.GetCameraCharacteristics(cameraId);

					// We don't use a front facing camera in this sample.
					var facing = (Integer)characteristics.Get(CameraCharacteristics.LensFacing);
					if (facing != null && facing == Integer.ValueOf((int)LensFacing.Front))
					{
						continue;
					}

					var map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);
					if (map == null)
					{
						continue;
					}

					// For still image captures, we use the largest available size.
					var largest = (Size)Collections.Max(Arrays.AsList(map.GetOutputSizes((int)ImageFormatType.Jpeg)),
						new CompareSizesByArea());
					mImageReader = ImageReader.NewInstance(largest.Width, largest.Height, ImageFormatType.Jpeg, /*maxImages*/2);
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
							{
								swappedDimensions = true;
							}
							break;
						case SurfaceOrientation.Rotation90:
						case SurfaceOrientation.Rotation270:
							if (mSensorOrientation == 0 || mSensorOrientation == 180)
							{
								swappedDimensions = true;
							}
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
					{
						maxPreviewWidth = MAX_PREVIEW_WIDTH;
					}

					if (maxPreviewHeight > MAX_PREVIEW_HEIGHT)
					{
						maxPreviewHeight = MAX_PREVIEW_HEIGHT;
					}

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
				// ErrorDialog.NewInstance(GetString(Resource.String.camera_error)).Show(ChildFragmentManager, FRAGMENT_DIALOG);
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

		TaskCompletionSource<bool> permissionsRequested;

		bool audioPermissionsGranted;
		bool cameraPermissionsGranted;

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
	}
}
