﻿using System;
using NUnit.Framework;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class CameraView_Tests
	{
		[Test]
		public void TestConstructor()
		{
			var camera = new CameraView();

			Assert.False(camera.IsBusy);
			Assert.False(camera.IsAvailable);
			Assert.AreEqual(CameraOptions.Default, camera.CameraOptions);

			// See TODO on CameraView.SavePhotoToFile
			// Assert.False(camera.SavePhotoToFile);

			Assert.AreEqual(CameraCaptureMode.Default, camera.CaptureMode);
			Assert.AreEqual(CameraFlashMode.Off, camera.FlashMode);
		}

		[Test]
		public void TestOnMediaCaptured()
		{
			var camera = new CameraView();

			var fired = false;
			var args = new MediaCapturedEventArgs();
			camera.MediaCaptured += (_, e) => fired = e == args;
			camera.RaiseMediaCaptured(args);

			Assert.IsTrue(fired);
		}

		[Test]
		public void TestOnMediaCapturedFailed()
		{
			var camera = new CameraView();

			var fired = false;
			camera.MediaCaptureFailed += (_, e) => fired = e == "123";
			camera.RaiseMediaCaptureFailed("123");

			Assert.IsTrue(fired);
		}

		[Test]
		public void TestOnShutterClicked()
		{
			var camera = new CameraView();

			var fired = false;
			camera.ShutterClicked += (sender, e) => fired = true;
			camera.Shutter();

			Assert.IsTrue(fired);
		}

		[Test]
		public void TestShutterCommand()
		{
			var camera = new CameraView();
			var trigged = false;

			camera.ShutterClicked += (_, __) =>
			{
				trigged = true;
			};

			camera.ShutterCommand?.Execute(null);

			Assert.IsTrue(trigged);
		}

		[Test]
		public void TestShutterCommandFromVM()
		{
			var vm = new CameraViewModel();
			var camera = new CameraView
			{
				BindingContext = vm
			};

			var trigged = false;

			camera.SetBinding(CameraView.ShutterCommandProperty, nameof(vm.CameraShutterCommand));

			camera.ShutterClicked += (_, __) =>
			{
				trigged = true;
			};

			vm.ShutterCommand.Execute(null);

			Assert.IsTrue(trigged);
		}

		class CameraViewModel
		{
			public Command ShutterCommand { get; }

			// The dev should have the set in this property
			public Command CameraShutterCommand { get; set; }

			public CameraViewModel()
			{
				ShutterCommand = new Command(Shutter);
				CameraShutterCommand = new Command(DoNothing);
			}

			void Shutter() => CameraShutterCommand.Execute(null);

			void DoNothing() =>
				Console.WriteLine("This is just to prove that the user can't override the CameraViewShutterCommand");
		}
	}
}