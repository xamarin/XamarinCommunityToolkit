using System;
using Android.Hardware.Camera2;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraCaptureListener : CameraCaptureSession.CaptureCallback
	{
		public Action<TotalCaptureResult> OnCompleted;

		public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
			=> OnCompleted?.Invoke(result);
	}
}
