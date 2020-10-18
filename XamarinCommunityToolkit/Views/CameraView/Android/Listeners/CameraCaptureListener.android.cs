using System;
using Android.Hardware.Camera2;
using Java.Lang;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraCaptureListener : CameraCaptureSession.CaptureCallback
	{
		readonly CameraFragment cameraFragment;

		public CameraCaptureListener(CameraFragment camera) =>
			cameraFragment = camera ?? throw new ArgumentNullException(nameof(camera));

		public Action<TotalCaptureResult> OnCompleted { get; set; }

		public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
			=> Process(result);

		public override void OnCaptureProgressed(CameraCaptureSession session, CaptureRequest request, CaptureResult partialResult) =>
			Process(partialResult);

		void Process(CaptureResult result)
		{
			switch (cameraFragment.CurrentState)
			{
				case CameraFragment.StateWaitingLock:
					{
						var afState = (Integer)result.Get(CaptureResult.ControlAfState);
						if (afState == null)
						{
							cameraFragment.CurrentState = CameraFragment.StatePictureTaken;
							cameraFragment.TakePhoto();
						}
						else if ((afState.IntValue() == ((int)ControlAFState.FocusedLocked)) ||
								   (afState.IntValue() == ((int)ControlAFState.NotFocusedLocked)))
						{
							// ControlAeState can be null on some devices
							var aeState = (Integer)result.Get(CaptureResult.ControlAeState);

							if (aeState == null || aeState.IntValue() == ((int)ControlAEState.Converged))
							{
								cameraFragment.CurrentState = CameraFragment.StatePictureTaken;
								cameraFragment.TakePhoto();
							}
							else
							{
								cameraFragment.RunPrecaptureSequence();
							}
						}
						break;
					}
				case CameraFragment.StateWaitingPrecapture:
					{
						// ControlAeState can be null on some devices
						var aeState = (Integer)result.Get(CaptureResult.ControlAeState);
						if (aeState == null ||
								aeState.IntValue() == ((int)ControlAEState.Precapture) ||
								aeState.IntValue() == ((int)ControlAEState.FlashRequired))
						{
							cameraFragment.CurrentState = CameraFragment.StateWaitingNonPrecapture;
						}
						break;
					}
				case CameraFragment.StateWaitingNonPrecapture:
					{
						// ControlAeState can be null on some devices
						var aeState = (Integer)result.Get(CaptureResult.ControlAeState);
						if (aeState == null || aeState.IntValue() != ((int)ControlAEState.Precapture))
						{
							cameraFragment.CurrentState = CameraFragment.StatePictureTaken;
							cameraFragment.TakePhoto();
						}
						break;
					}
			}
		}
	}
}