using System.Collections.Generic;

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
using Android.OS;
using AOrientation = Android.Content.Res.Orientation;
using AView = Android.Views.View;

using Java.Lang;
using Java.Util.Concurrent;
using AGraphics = Android.Graphics;
using System.Threading.Tasks;
using Android.Widget;
using Android.Util;
using Size = Android.Util.Size;
using Java.Util;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraFragment : Fragment
	{
		#region Camera States

		// Camera state: Showing camera preview.
		public const int StatePreview = 0;

		// Camera state: Waiting for the focus to be locked.
		public const int StateWaitingLock = 1;

		// Camera state: Waiting for the exposure to be precapture state.
		public const int StateWaitingPrecapture = 2;

		// Camera state: Waiting for the exposure state to be something other than precapture.
		public const int StateWaitingNonPrecapture = 3;

		// Camera state: Picture was taken.
		public const int StatePictureTaken = 4;

		#endregion

		// The current state of camera state for taking pictures.
		public int CurrentState = StatePreview;

		// Max preview width that is guaranteed by Camera2 API
		static readonly int MAX_PREVIEW_WIDTH = 1920;

		// Max preview height that is guaranteed by Camera2 API
		static readonly int MAX_PREVIEW_HEIGHT = 1080;

		CameraStateListener mStateCallback;
		CameraCaptureListener mCaptureCallback;
		ImageAvailableListener mOnImageAvailableListener;
		public CameraCaptureSession mCaptureSession;
		public CameraDevice mCameraDevice;
		string mCameraId;

		private Size previewSize;

		AutoFitTextureView texture;
		private TextureView.ISurfaceTextureListener mSurfaceTextureListener;
		private Handler mBackgroundHandler;
		private HandlerThread mBackgroundThread;
		private ImageReader mImageReader;
		public Semaphore mCameraOpenCloseLock = new Semaphore(1);
		readonly SparseIntArray orientations = new SparseIntArray();

		bool audioPermissionsGranted;
		bool cameraPermissionsGranted;
		TaskCompletionSource<bool> permissionsRequested;
		public CameraView Element { get; set; }

		private int mSensorOrientation;
		private bool mFlashSupported;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			mStateCallback = new CameraStateListener(null);
			mSurfaceTextureListener = new CameraSurfaceTextureListener(this);

			orientations.Append((int)SurfaceOrientation.Rotation0, 90);
			orientations.Append((int)SurfaceOrientation.Rotation90, 0);
			orientations.Append((int)SurfaceOrientation.Rotation180, 270);
			orientations.Append((int)SurfaceOrientation.Rotation270, 180);
		}

		public override AView OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) =>
			inflater.Inflate(Resource.Layout.CameraFragment, null);

		public override void OnViewCreated(AView view, Bundle savedInstanceState) =>
			texture = view.FindViewById<AutoFitTextureView>(Resource.Id.cameratexture);

		public override void OnActivityCreated(Bundle savedInstanceState)
		{
			base.OnActivityCreated(savedInstanceState);
			mCaptureCallback = new CameraCaptureListener(this);
			mOnImageAvailableListener = new ImageAvailableListener();
		}

		public override void OnResume()
		{
			base.OnResume();
			StartBackgroundThread();

			if (texture.IsAvailable)
				OpenCamera(texture.Width, texture.Height);
			else
				texture.SurfaceTextureListener = mSurfaceTextureListener;
		}

		public override void OnPause()
		{
			CloseCamera();
			StopBackgroundThread();
			base.OnPause();
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
			var manager = (CameraManager)activity.GetSystemService(Context.CameraService);
			try
			{
				if (!mCameraOpenCloseLock.TryAcquire(2500, TimeUnit.Milliseconds))
				{
					throw new RuntimeException("Time out waiting to lock camera opening.");
				}
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

		void SetUpCameraOutputs(int width, int height)
		{
			var activity = Activity;
			var manager = (CameraManager)activity.GetSystemService(Context.CameraService);
			try
			{
				for (var i = 0; i < manager.GetCameraIdList().Length; i++)
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
					{
						texture.SetAspectRatio(previewSize.Width, previewSize.Height);
					}
					else
					{
						texture.SetAspectRatio(previewSize.Height, previewSize.Width);
					}

					// Check if the flash is supported.
					var available = (Java.Lang.Boolean)characteristics.Get(CameraCharacteristics.FlashInfoAvailable);
					if (available == null)
					{
						mFlashSupported = false;
					}
					else
					{
						mFlashSupported = (bool)available;
					}

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

		static Size ChooseOptimalSize(Size[] choices, int textureViewWidth,
			int textureViewHeight, int maxWidth, int maxHeight, Size aspectRatio)
		{
			// Collect the supported resolutions that are at least as big as the preview Surface
			var bigEnough = new List<Size>();

			// Collect the supported resolutions that are smaller than the preview Surface
			var notBigEnough = new List<Size>();
			var w = aspectRatio.Width;
			var h = aspectRatio.Height;

			for (var i = 0; i < choices.Length; i++)
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
					{
						notBigEnough.Add(option);
					}
				}
			}

			// Pick the smallest of those big enough. If there is no one big enough, pick the
			// largest of those not big enough.
			if (bigEnough.Count > 0)
			{
				return (Size)Collections.Min(bigEnough, new CompareSizesByArea());
			}
			else if (notBigEnough.Count > 0)
			{
				return (Size)Collections.Max(notBigEnough, new CompareSizesByArea());
			}
			else
			{
				// Log.Error(TAG, "Couldn't find any suitable preview size");
				return choices[0];
			}
		}

		void ConfigureTransform(int viewWidth, int viewHeight)
		{
			var activity = Activity;
			if (texture == null || previewSize == null || activity == null)
			{
				return;
			}
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
			{
				matrix.PostRotate(180, centerX, centerY);
			}
			texture.SetTransform(matrix);
		}

		void CloseCamera()
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
				throw new RuntimeException("Interrupted while trying to lock camera closing.", e);
			}
			finally
			{
				mCameraOpenCloseLock.Release();
			}
		}

		void StopBackgroundThread()
		{
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

		void StartBackgroundThread()
		{
			mBackgroundThread = new HandlerThread("CameraBackground");
			mBackgroundThread.Start();
			mBackgroundHandler = new Handler(mBackgroundThread.Looper);
		}



		public void ShowToast(string text) =>
			Activity?.RunOnUiThread(() => Toast.MakeText(Context, text, ToastLength.Short).Show());

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

		//public void UnlockFocus()
		//{
		//	try
		//	{
		//		// Reset the auto-focus trigger
		//		mPreviewRequestBuilder.Set(CaptureRequest.ControlAfTrigger, (int)ControlAFTrigger.Cancel);
		//		SetAutoFlash(mPreviewRequestBuilder);
		//		mCaptureSession.Capture(mPreviewRequestBuilder.Build(), mCaptureCallback,
		//				mBackgroundHandler);
		//		// After this, the camera will go back to the normal state of preview.
		//		mState = STATE_PREVIEW;
		//		mCaptureSession.SetRepeatingRequest(mPreviewRequest, mCaptureCallback,
		//				mBackgroundHandler);
		//	}
		//	catch (CameraAccessException e)
		//	{
		//		e.PrintStackTrace();
		//	}
		//}
	}
}