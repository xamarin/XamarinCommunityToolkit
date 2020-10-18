using System;
using Android.Hardware.Camera2;
using Android.Util;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraCaptureStillPictureSessionCallback : CameraCaptureSession.CaptureCallback
	{
		static readonly string TAG = "CameraCaptureStillPictureSessionCallback";
		readonly CameraFragment owner;

		public CameraCaptureStillPictureSessionCallback(CameraFragment cameraFragment) =>
			owner = cameraFragment ?? throw new ArgumentNullException(nameof(cameraFragment));

		public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
		{
			// If something goes wrong with the save (or the handler isn't even 
			// registered, this code will toast a success message regardless...)
			owner.ShowToast("Saved: " + owner.mFile);
			Log.Debug(TAG, owner.mFile.ToString());
			owner.UnlockFocus();
		}
	}
}
