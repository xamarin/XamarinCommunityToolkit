using System;
using System.Collections.Generic;
using Android.Hardware.Camera2;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraStateListener : CameraDevice.StateCallback
	{
		public Action<CameraDevice> OnOpenedAction;
		public Action<CameraDevice> OnDisconnectedAction;
		public Action<CameraDevice, CameraError> OnErrorAction;
		public Action<CameraDevice> OnClosedAction;

		public override void OnOpened(CameraDevice camera) => OnOpenedAction?.Invoke(camera);

		public override void OnDisconnected(CameraDevice camera) => OnDisconnectedAction?.Invoke(camera);

		public override void OnError(CameraDevice camera, CameraError error) => OnErrorAction(camera, error);

		public override void OnClosed(CameraDevice camera) => OnClosedAction(camera);
	}
}
