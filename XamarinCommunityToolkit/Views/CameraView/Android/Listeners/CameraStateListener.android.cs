using System;
using System.Collections.Generic;
using Android.Hardware.Camera2;
using Android.Runtime;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class CameraStateListener : CameraDevice.StateCallback
	{
		readonly CameraDroid owner;

		public CameraStateListener(CameraDroid cameraFragment) =>
			owner = cameraFragment ?? throw new ArgumentNullException(nameof(cameraFragment));

		public override void OnDisconnected(CameraDevice camera)
		{
			owner.mCameraOpenCloseLock.Release();
		//	cameraDevice.Close();
			owner.mCameraDevice = null;
		}

		public override void OnError(CameraDevice camera, [GeneratedEnum] CameraError error)
		{
			owner.mCameraOpenCloseLock.Release();
		//	cameraDevice.Close();
			owner.mCameraDevice = null;
			if (owner == null)
				return;
			var activity = owner.Activity;

			if (activity != null)
				activity.Finish();
		}

		public override void OnOpened(CameraDevice camera)
		{
			owner.mCameraOpenCloseLock.Release();
		//	owner.mCameraDevice = cameraDevice;
		//	owner.CreateCameraPreviewSession();
		}
	}
}