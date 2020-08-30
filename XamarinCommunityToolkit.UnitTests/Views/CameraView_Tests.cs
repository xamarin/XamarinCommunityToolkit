using System;
using Xunit;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class CameraView_Tests
	{
		[Fact]
		public void TestConstructor()
		{
			var camera = new CameraView();

			Assert.False(camera.IsBusy);
			Assert.False(camera.IsAvailable);
			Assert.Equal(CameraOptions.Default, camera.CameraOptions);
			Assert.False(camera.SavePhotoToFile);
			Assert.Equal(CameraCaptureOptions.Default, camera.CaptureOptions);
			Assert.False(camera.KeepScreenOn);
			Assert.Equal(CameraFlashMode.Off, camera.FlashMode);
		}

		[Fact]
		public void TestOnMediaCaptured()
		{
			var camera = new CameraView();

			var fired = false;
			var args = new MediaCapturedEventArgs();
			camera.MediaCaptured += (_, e) => fired = e == args;
			camera.RaiseMediaCaptured(args);

			Assert.True(fired);
		}

		[Fact]
		public void TestOnMediaCapturedFailed()
		{
			var camera = new CameraView();

			var fired = false;
			camera.MediaCaptureFailed += (_, e) => fired = e == "123";
			camera.RaiseMediaCaptureFailed("123");

			Assert.True(fired);
		}

		[Fact]
		public void TestOnShitterClicked()
		{
			var camera = new CameraView();

			var fired = false;
			camera.ShutterClicked += (sender, e) => fired = true;
			camera.Shutter();

			Assert.True(fired);
		}
	}
}