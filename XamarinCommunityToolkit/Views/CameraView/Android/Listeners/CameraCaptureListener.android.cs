using System;
using Android.Hardware.Camera2;
using Java.Lang;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraCaptureListener : CameraCaptureSession.CaptureCallback
	{
		readonly CameraDroid cameraDroid;

		public CameraCaptureListener(CameraDroid camera) =>
			cameraDroid = camera ?? throw new ArgumentNullException(nameof(camera));

		public Action<TotalCaptureResult> OnCompleted { get; set; }

		public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
			=> Process(result);

		public override void OnCaptureProgressed(CameraCaptureSession session, CaptureRequest request, CaptureResult partialResult) =>
			Process(partialResult);

		void Process(CaptureResult result)
		{
			switch (cameraDroid.mState)
			{
				case CameraDroid.STATE_WAITING_LOCK:
					StateWaitingLock(result, cameraDroid);
					break;
				case CameraDroid.STATE_WAITING_PRECAPTURE:
					StateWaitingPrecapture(result, cameraDroid);
					break;
				case CameraDroid.STATE_WAITING_NON_PRECAPTURE:
					StateWaitingNonPrecapture(result, cameraDroid);
					break;
			}

			static void StateWaitingLock(CaptureResult result, CameraDroid cameraFragment)
			{
				var afState = (Integer)result.Get(CaptureResult.ControlAfState);
				if (afState == null)
				{
					cameraFragment.mState = CameraFragment.StatePictureTaken;
					cameraFragment.CaptureStillPicture();
				}
				else if ((afState.IntValue() == ((int)ControlAFState.FocusedLocked))
						|| (afState.IntValue() == ((int)ControlAFState.NotFocusedLocked)))
				{
					var aeState = (Integer)result.Get(CaptureResult.ControlAeState);

					if (aeState == null || aeState.IntValue() == ((int)ControlAEState.Converged))
					{
						cameraFragment.mState = CameraFragment.StatePictureTaken;
						cameraFragment.CaptureStillPicture();
					}
					else
						cameraFragment.RunPrecaptureSequence();
				}
			}
			static void StateWaitingPrecapture(CaptureResult result, CameraDroid cameraFragment)
			{
				var aeState = (Integer)result.Get(CaptureResult.ControlAeState);
				if (aeState == null
					|| aeState.IntValue() == ((int)ControlAEState.Precapture)
					|| aeState.IntValue() == ((int)ControlAEState.FlashRequired))
				{
					cameraFragment.mState = CameraFragment.StateWaitingNonPrecapture;
				}
			}
			static void StateWaitingNonPrecapture(CaptureResult result, CameraDroid cameraFragment)
			{
				var aeState = (Integer)result.Get(CaptureResult.ControlAeState);
				if (aeState == null || aeState.IntValue() != ((int)ControlAEState.Precapture))
				{
					cameraFragment.mState = CameraFragment.StatePictureTaken;
					cameraFragment.CaptureStillPicture();
				}
			}
		}
	}
}