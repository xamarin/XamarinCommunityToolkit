using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xunit;

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
		public void TestOnShutterClicked()
		{
			var camera = new CameraView();

			var fired = false;
			camera.ShutterClicked += (sender, e) => fired = true;
			camera.Shutter();

			Assert.True(fired);
		}

		[Fact]
		public void TestShutterCommand()
		{
			var x = 0;
			var camera = new CameraView
			{
				ShutterCommand = new Command(ShutterCommand)
			};

			camera.ShutterCommand.Execute(null);

			Assert.Equal(1, x);

			void ShutterCommand()
			{
				x++;
			}
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestShutterCommandCanExecute(bool canExecute)
		{
			var x = 0;
			var camera = new CameraView
			{
				ShutterCommand = new Command(ShutterCommand, CanExecute)
			};

			if (camera.ShutterCommand.CanExecute(null))
				camera.ShutterCommand.Execute(null);

			var expected = canExecute ? 1 : 0;
			Assert.Equal(expected, x);

			void ShutterCommand()
			{
				x++;
			}

			bool CanExecute()
			{
				return canExecute;
			}
		}

		[Fact]
		public void TestShutterCommandParameter()
		{
			var x = 0;
			var camera = new CameraView
			{
				ShutterCommand = new Command<int>(ShutterCommand),
				ShutterCommandParameter = 5
			};

			Assert.NotNull(camera.ShutterCommand);

			camera.ShutterCommand.Execute(camera.ShutterCommandParameter);

			camera.Shutter();

			Assert.Equal(10, x);

			void ShutterCommand(int value)
			{
				x += value;
			}
		}
	}
}