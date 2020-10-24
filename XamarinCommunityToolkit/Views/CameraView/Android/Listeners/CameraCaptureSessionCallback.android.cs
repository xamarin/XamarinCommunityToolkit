using System;
using System.Collections.Generic;
using System.Text;
using Android.Hardware.Camera2;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraCaptureSessionCallback : CameraCaptureSession.StateCallback
	{
		readonly CameraDroid owner;

		public CameraCaptureSessionCallback(CameraDroid cameraFragment) =>
			owner = cameraFragment ?? throw new ArgumentNullException(nameof(cameraFragment));

		public override void OnConfigured(CameraCaptureSession session)
		{
			if (owner.mCameraDevice == null)
			{
				return;
			}

			// When the session is ready, we start displaying the preview.
			owner.mCaptureSession = session;
			try
			{
				// Auto focus should be continuous for camera preview.
				//owner.mPreviewRequestBuilder.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.ContinuousPicture);
				// Flash is automatically enabled when necessary.
			//	owner.SetAutoFlash(owner.mPreviewRequestBuilder);

				// Finally, we start displaying the camera preview.
			//	owner.mPreviewRequest = owner.mPreviewRequestBuilder.Build();
			//	owner.mCaptureSession.SetRepeatingRequest(owner.mPreviewRequest,
			//			owner.mCaptureCallback, owner.mBackgroundHandler);
			}
			catch (CameraAccessException e)
			{
				e.PrintStackTrace();
			}
		}

		public override void OnConfigureFailed(CameraCaptureSession session) => Console.WriteLine("oi"); //owner.ShowToast("Failed");
	}
}
