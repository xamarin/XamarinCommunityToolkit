using System;
using System.Collections.Generic;
using System.Text;
using AContent = Android.Content;
using AUtil = Android.Util;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Java.Lang;
using Android.Hardware.Camera2.Params;
using Android.Media;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraDroid : Fragment, TextureView.ISurfaceTextureListener
	{
		HandlerThread backgroundThread;
		Handler backgroundHandler;
		CameraManager manager;
		string cameraId;
		CameraStateListener cameraStateListener;
		AUtil.Size[] supportedJpegSizes;
		AUtil.Size idealPhotoSize = new AUtil.Size(480, 640);
		ImageReader imageReader;
		bool flashSupported;
		private AUtil.Size previewSize;

		public event EventHandler<byte[]> Photo;

		public bool OpeningCamera { private get; set; }

		public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
		{
			StartBackgroundThread();
			OpenCamera(width, height);
		}

		public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
		{
			StopBackgroundThread();
			return true;
		}

		public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
		{
		}

		public void OnSurfaceTextureUpdated(SurfaceTexture surface)
		{
		}

		void StartBackgroundThread()
		{
			backgroundThread = new HandlerThread("CameraBackground");
			backgroundThread.Start();
			backgroundHandler = new Handler(backgroundThread.Looper);
		}

		void OpenCamera(int width, int height)
		{
			if (Context == null || OpeningCamera)
				return;

			OpeningCamera = true;

			SetUpCameraOutputs(width, height);

			manager.OpenCamera(cameraId, cameraStateListener, null);
		}

		void SetUpCameraOutputs(int width, int height)
		{
			manager = (CameraManager)Context.GetSystemService(AContent.Context.CameraService);

			var cameraIds = manager.GetCameraIdList();

			cameraId = cameraIds[0];

			for (var i = 0; i < cameraIds.Length; i++)
			{
				var chararc = manager.GetCameraCharacteristics(cameraIds[i]);

				var facing = (Integer)chararc.Get(CameraCharacteristics.LensFacing);
				if (facing != null && facing == Integer.ValueOf((int)LensFacing.Back))
				{
					cameraId = cameraIds[i];

					// Phones like Galaxy S10 have 2 or 3 frontal cameras usually the one with flash is the one
					// that should be chosen, if not It will select the first one and that can be the fish
					// eye camera
					if (HasFLash(chararc))
						break;
				}
			}

			var characteristics = manager.GetCameraCharacteristics(cameraId);
			var map = (StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap);

			if (supportedJpegSizes == null && characteristics != null)
			{
				supportedJpegSizes = ((StreamConfigurationMap)characteristics.Get(CameraCharacteristics.ScalerStreamConfigurationMap)).GetOutputSizes((int)ImageFormatType.Jpeg);
			}

			if (supportedJpegSizes != null && supportedJpegSizes.Length > 0)
			{
				idealPhotoSize = GetOptimalSize(supportedJpegSizes, 1050, 1400); // MAGIC NUMBER WHICH HAS PROVEN TO BE THE BEST
			}

			imageReader = ImageReader.NewInstance(idealPhotoSize.Width, idealPhotoSize.Height, ImageFormatType.Jpeg, 1);

			var readerListener = new ImageAvailableListener();

			readerListener.Photo += (sender, buffer) =>
			{
				Photo?.Invoke(this, buffer);
			};

			flashSupported = HasFLash(characteristics);

			imageReader.SetOnImageAvailableListener(readerListener, backgroundHandler);

			previewSize = GetOptimalSize(map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture))), width, height);
		}

		bool HasFLash(CameraCharacteristics characteristics)
		{
			var available = (Java.Lang.Boolean)characteristics.Get(CameraCharacteristics.FlashInfoAvailable);
			if (available == null)
				return false;
			else
				return (bool)available;
		}

		AUtil.Size GetOptimalSize(IList<AUtil.Size> sizes, int h, int w)
		{
			var aspectTolerance = 0.1;
			var targetRatio = (double)w / h;

			if (sizes == null)
			{
				return null;
			}

			AUtil.Size optimalSize = null;
			var minDiff = double.MaxValue;
			var targetHeight = h;

			while (optimalSize == null)
			{
				foreach (var size in sizes)
				{
					var ratio = (double)size.Width / size.Height;

					if (System.Math.Abs(ratio - targetRatio) > aspectTolerance)
						continue;
					if (System.Math.Abs(size.Height - targetHeight) < minDiff)
					{
						optimalSize = size;
						minDiff = System.Math.Abs(size.Height - targetHeight);
					}
				}

				if (optimalSize == null)
					aspectTolerance += 0.1f;
			}

			return optimalSize;
		}

		void StopBackgroundThread()
		{
			backgroundThread.QuitSafely();
			try
			{
				backgroundThread.Join();
				backgroundThread = null;
				backgroundHandler = null;
			}
			catch (InterruptedException e)
			{
				e.PrintStackTrace();
			}
		}
	}
}
