using System;
using System.IO;
using System.Threading.Tasks;
using AVFoundation;
using CoreGraphics;
using CoreMedia;
using Foundation;
using UIKit;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public sealed class FormsCameraView : UIView, IAVCaptureFileOutputRecordingDelegate
	{
		readonly AVCaptureVideoPreviewLayer previewLayer;
		readonly AVCaptureSession captureSession;
		readonly UIView mainView;
		AVCaptureDeviceInput input;
		AVCaptureStillImageOutput imageOutput;
		AVCapturePhotoOutput photoOutput;
		AVCaptureMovieFileOutput videoOutput;
		AVCaptureConnection captureConnection;
		AVCaptureDevice device;
		bool isBusy;
		bool isAvailable;
		CameraFlashMode flashMode;
		readonly float imgScale = 1f;

		public event EventHandler<bool> Busy;
		public event EventHandler<bool> Available;
		public event EventHandler<Tuple<NSObject, NSError>> FinishCapture;

		public bool VideoRecorded => videoOutput?.Recording == true;

		public FormsCameraView()
		{
			flashMode = CameraFlashMode.Off;
			mainView = new UIView { TranslatesAutoresizingMaskIntoConstraints = false };
			AutoresizingMask = UIViewAutoresizing.FlexibleMargins;

			captureSession = new AVCaptureSession
			{
				SessionPreset = AVCaptureSession.PresetHigh
			};

			previewLayer = new AVCaptureVideoPreviewLayer(captureSession)
			{
				VideoGravity = AVLayerVideoGravity.ResizeAspectFill
			};

			mainView.Layer.AddSublayer(previewLayer);
			RetrieveCameraDevice(CameraOptions.Default);

			Add(mainView);

			AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[mainView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, new NSDictionary("mainView", mainView)));
			AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[mainView]|", NSLayoutFormatOptions.AlignAllTop, null, new NSDictionary("mainView", mainView)));
		}

		void SetStartOrientation()
		{
			var previewLayerFrame = previewLayer.Frame;

			switch (UIApplication.SharedApplication.StatusBarOrientation)
			{
				case UIInterfaceOrientation.Portrait:
				case UIInterfaceOrientation.PortraitUpsideDown:
					previewLayerFrame.Height = UIScreen.MainScreen.Bounds.Height;
					previewLayerFrame.Width = UIScreen.MainScreen.Bounds.Width;
					break;

				case UIInterfaceOrientation.LandscapeLeft:
				case UIInterfaceOrientation.LandscapeRight:
					previewLayerFrame.Width = UIScreen.MainScreen.Bounds.Width;
					previewLayerFrame.Height = UIScreen.MainScreen.Bounds.Height;
					break;
			}

			try
			{
				previewLayer.Frame = previewLayerFrame;
			}
			catch (Exception error)
			{
				LogError("Failed to adjust frame", error);
			}
		}

		void LogError(string message, Exception error = null)
		{
			var errorMessage = error == null
				? string.Empty
				: Environment.NewLine +
					$"ErrorMessage: {Environment.NewLine}" +
					error.Message + Environment.NewLine +
					$"Stacktrace: {Environment.NewLine}" +
					error.StackTrace;

			Forms.Internals.Log.Warning("Camera", $"{message}{errorMessage}");
		}

		bool IsBusy
		{
			get => isBusy;
			set
			{
				if (isBusy != value)
					Busy?.Invoke(this, value);
				isBusy = value;
			}
		}

		UIImage RotateImage(UIImage image)
		{
			var imgRef = image.CGImage;
			var transform = CGAffineTransform.MakeIdentity();

			var imgHeight = imgRef.Height * imgScale;
			var imgWidth = imgRef.Width * imgScale;

			var bounds = new CGRect(0, 0, imgWidth, imgHeight);
			var imageSize = new CGSize(imgWidth, imgHeight);
			var orient = image.Orientation;

			switch (orient)
			{
				case UIImageOrientation.Up:
					transform = CGAffineTransform.MakeIdentity();
					break;
				case UIImageOrientation.Down:
					transform = CGAffineTransform.MakeTranslation(imageSize.Width, imageSize.Height);
					transform = CGAffineTransform.Rotate(transform, (float)Math.PI);
					break;
				case UIImageOrientation.Right:
					bounds.Size = new CGSize(bounds.Size.Height, bounds.Size.Width);
					transform = CGAffineTransform.MakeTranslation(imageSize.Height, 0);
					transform = CGAffineTransform.Rotate(transform, (float)Math.PI / 2.0f);
					break;
				default:
					throw new Exception("Invalid image orientation");
			}

			UIGraphics.BeginImageContext(bounds.Size);
			var context = UIGraphics.GetCurrentContext();

			if (orient == UIImageOrientation.Right)
			{
				context.ScaleCTM(-1, 1);
				context.TranslateCTM(-imgHeight, 0);
			}
			else
			{
				context.ScaleCTM(1, -1);
				context.TranslateCTM(0, -imgHeight);
			}

			context.ConcatCTM(transform);

			context.DrawImage(new CGRect(0, 0, imgWidth, imgHeight), imgRef);
			image = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return image;
		}

		public override void Draw(CGRect rect)
		{
			previewLayer.Frame = rect;
			base.Draw(rect);
		}

		public float Zoom
		{
			get => (float)(device?.VideoZoomFactor ?? 1f);
			set
			{
				if (device == null)
					return;
				_ = device.LockForConfiguration(out _);
				device.VideoZoomFactor = Math.Max(1, Math.Min(value, MaxZoom));
				device.UnlockForConfiguration();
			}
		}

		public float MaxZoom => (float)(device?.ActiveFormat.VideoMaxZoomFactor ?? 1f);

		public async Task TakePhoto()
		{
			if (isBusy || device == null || videoOutput != null)
				return;

			IsBusy = true;
			// iOS >= 10
			if (photoOutput != null)
			{
				var photoOutputConnection = photoOutput.ConnectionFromMediaType(AVMediaType.Video);
				if (photoOutputConnection != null)
					photoOutputConnection.VideoOrientation = previewLayer.Connection.VideoOrientation;

				var photoSettings = AVCapturePhotoSettings.Create();
				photoSettings.FlashMode = (AVCaptureFlashMode)flashMode;
				photoSettings.IsHighResolutionPhotoEnabled = true;

				var photoCaptureDelegate = new PhotoCaptureDelegate
				{
					OnFinishCapture = (data, error) =>
					{
						FinishCapture?.Invoke(this, new Tuple<NSObject, NSError>(data, error));
						IsBusy = false;
					},
					WillCapturePhotoAnimation = () => Animate(0.25, () => previewLayer.Opacity = 1)
				};

				photoOutput.CapturePhoto(photoSettings, photoCaptureDelegate);
				return;
			}
			// iOS < 10
			try
			{
				var connection = imageOutput.Connections[0];
				connection.VideoOrientation = previewLayer.Connection.VideoOrientation;
				var sampleBuffer = await imageOutput.CaptureStillImageTaskAsync(connection);
				var imageData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
				FinishCapture?.Invoke(this, new Tuple<NSObject, NSError>(imageData, null));
			}
			catch (Exception)
			{
				FinishCapture?.Invoke(this, new Tuple<NSObject, NSError>(null, new NSError(new NSString("faled create image"), 0)));
			}
			IsBusy = false;
		}

		string ConstructVideoFilename()
		{
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var library = Path.Combine(documents, "..", "Library");
			var timeStamp = DateTime.Now.ToString("yyyyddMM_HHmmss");
			return Path.Combine(library, $"VID_{timeStamp}.mov");
		}

		public void StartRecord() // TODO audio record
		{
			if (isBusy || device == null || videoOutput?.Recording == true)
				return;

			captureSession.BeginConfiguration();

			videoOutput = new AVCaptureMovieFileOutput();
			if (captureSession.CanAddOutput(videoOutput))
				captureSession.AddOutput(videoOutput);

			captureSession.CommitConfiguration();

			IsBusy = true;
			try
			{
				videoOutput.Connections[0].VideoOrientation = previewLayer.Connection.VideoOrientation;
				var connection = videoOutput.Connections[0];

				if (connection.SupportsVideoOrientation)
					connection.VideoOrientation = previewLayer.Orientation;
				if (connection.SupportsVideoStabilization)
					connection.PreferredVideoStabilizationMode = VideoStabilization ? AVCaptureVideoStabilizationMode.Auto : AVCaptureVideoStabilizationMode.Off;

				var outputFileURL = NSUrl.FromFilename(ConstructVideoFilename());

				videoOutput.StartRecordingToOutputFile(outputFileURL, this);
			}
			catch (Exception error)
			{
				LogError("Error with camera output capture", error);
			}
			IsBusy = false;
		}

		public void StopRecord()
		{
			if (!isBusy && device != null && videoOutput != null && videoOutput.Recording)
			{
				IsBusy = true;
				videoOutput.StopRecording();
			}
		}

		public void FinishedRecording(AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject[] connections, NSError error)
		{
			FinishCapture?.Invoke(this, new Tuple<NSObject, NSError>(outputFileUrl, error));
			captureSession.RemoveOutput(videoOutput);
			videoOutput = null;
			IsBusy = false;
		}

		public void SwitchFlash(CameraFlashMode newFlashMode)
		{
			if (isAvailable && device != null && newFlashMode != flashMode)
			{
				flashMode = newFlashMode;
				SwitchFlash();
			}
		}

		void SwitchFlash()
		{
			try
			{
				device.LockForConfiguration(out var err);

				switch (flashMode)
				{
					default:
					case CameraFlashMode.Off:
						if (device.IsFlashModeSupported(AVCaptureFlashMode.Off))
							device.FlashMode = AVCaptureFlashMode.Off;
						break;
					case CameraFlashMode.On:
						if (device.IsFlashModeSupported(AVCaptureFlashMode.On))
							device.FlashMode = AVCaptureFlashMode.On;
						break;
					case CameraFlashMode.Auto:
						if (device.IsFlashModeSupported(AVCaptureFlashMode.Auto))
							device.FlashMode = AVCaptureFlashMode.Auto;
						break;
					case CameraFlashMode.Torch:
						if (device.IsTorchModeSupported(AVCaptureTorchMode.On))
							device.TorchMode = AVCaptureTorchMode.On;
						break;
				}

				if (flashMode != CameraFlashMode.Torch &&
					device.TorchMode == AVCaptureTorchMode.On &&
					device.IsTorchModeSupported(AVCaptureTorchMode.Off))
					device.TorchMode = AVCaptureTorchMode.Off;

				device.UnlockForConfiguration();
			}
			catch (Exception error)
			{
				LogError("Failed to switch flash on/off", error);
			}
		}

		public bool VideoStabilization { get; set; }

		public void SetBounds(double width, double height)
		{
			mainView.Frame = new CGRect(0, 0, width, height);
			Draw(mainView.Frame);
		}

		public void ChangeFocusPoint(Point point)
		{
			if (!isAvailable && device == null)
				return;

			try
			{
				device.LockForConfiguration(out var err);

				var focus_x = point.X / Bounds.Width;
				var focus_y = point.Y / Bounds.Height;

				if (device.FocusPointOfInterestSupported)
					device.FocusPointOfInterest = new CGPoint(focus_x, focus_y);
				if (device.ExposurePointOfInterestSupported)
					device.ExposurePointOfInterest = new CGPoint(focus_x, focus_y);

				device.UnlockForConfiguration();
			}
			catch (Exception error)
			{
				LogError("Failed to adjust focus", error);
			}
		}

		public void RetrieveCameraDevice(CameraOptions cameraOptions)
		{
			var cameraAccess = false;
			switch (AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video))
			{
				case AVAuthorizationStatus.Authorized:
					cameraAccess = true;
					break;
				case AVAuthorizationStatus.NotDetermined:
					AVCaptureDevice.RequestAccessForMediaType(AVMediaType.Video, granted => cameraAccess = granted);
					break;
			}

			if (!cameraAccess)
				return;

			AVCaptureDevicePosition position;
			switch (cameraOptions)
			{
				default:
				case CameraOptions.Default:
				case CameraOptions.Back:
					position = AVCaptureDevicePosition.Back; break;
				case CameraOptions.Front:
					position = AVCaptureDevicePosition.Front; break;
				case CameraOptions.External:
					position = AVCaptureDevicePosition.Unspecified; break;
			}

			device = null;
			var devs = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
			foreach (var d in devs)
			{
				if (d.Position == position)
					device = d;
			}

			if (device == null)
			{
				ClearCaptureSession();
				isAvailable = false;
				LogError("No device detected");
				return;
			}
			isAvailable = device != null;

			InitializeCamera();
			SwitchFlash();
		}

		void InitializeCamera()
		{
			if (device == null)
			{
				LogError("Camera failed to initialise.");
				return;
			}

			try
			{
				device.LockForConfiguration(out var err);

				if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
					device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;

				device.UnlockForConfiguration();

				ClearCaptureSession();

				input = new AVCaptureDeviceInput(device, out var error);
				if (error != null)
					LogError($"Could not create device input: {error.LocalizedDescription}");

				captureSession.BeginConfiguration();

				if (captureSession.CanAddInput(input))
					captureSession.AddInput(input);
				if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
				{
					photoOutput = new AVCapturePhotoOutput();
					if (captureSession.CanAddOutput(photoOutput))
					{
						captureSession.AddOutput(photoOutput);
						photoOutput.IsHighResolutionCaptureEnabled = true;
						photoOutput.IsLivePhotoCaptureEnabled = photoOutput.IsLivePhotoCaptureSupported;
					}
				}
				else
				{
					imageOutput = new AVCaptureStillImageOutput();
					if (captureSession.CanAddOutput(imageOutput))
						captureSession.AddOutput(imageOutput);
				}

				captureSession.CommitConfiguration();

				InvokeOnMainThread(() =>
				{
					captureConnection = previewLayer.Connection;
					SetStartOrientation();
					captureSession.StartRunning();
				});
			}
			catch (Exception error)
			{
				LogError("Camera failed to initialise", error);
			}

			Draw(mainView.Frame);
			Available?.Invoke(this, isAvailable);
		}

		void ClearCaptureSession()
		{
			if (captureSession != null)
			{
				if (captureSession.Running)
					captureSession.StopRunning();
				if (imageOutput != null)
					captureSession.RemoveOutput(imageOutput);
				if (photoOutput != null)
					captureSession.RemoveOutput(photoOutput);
				if (videoOutput != null)
					captureSession.RemoveOutput(videoOutput);
				if (input != null)
					captureSession.RemoveInput(input);
			}
			input?.Dispose();
			imageOutput?.Dispose();
			photoOutput?.Dispose();
			videoOutput?.Dispose();
		}

		protected override void Dispose(bool disposing)
		{
			if (device?.TorchMode == AVCaptureTorchMode.On)
			{
				flashMode = CameraFlashMode.Off;
				SwitchFlash();
			}

			ClearCaptureSession();
			base.Dispose(disposing);
		}
	}

	class PhotoCaptureDelegate : NSObject, IAVCapturePhotoCaptureDelegate
	{
		public Action<NSData, NSError> OnFinishCapture;
		public Action WillCapturePhotoAnimation;

		NSData photoData;

		[Export("captureOutput:willCapturePhotoForResolvedSettings:")]
		public void WillCapturePhoto(AVCapturePhotoOutput captureOutput, AVCaptureResolvedPhotoSettings resolvedSettings) => WillCapturePhotoAnimation();

		[Export("captureOutput:didFinishProcessingPhotoSampleBuffer:previewPhotoSampleBuffer:resolvedSettings:bracketSettings:error:")]
		public void DidFinishProcessingPhoto(AVCapturePhotoOutput captureOutput, CMSampleBuffer photoSampleBuffer, CMSampleBuffer previewPhotoSampleBuffer, AVCaptureResolvedPhotoSettings resolvedSettings, AVCaptureBracketedStillImageSettings bracketSettings, NSError error)
		{
			if (photoSampleBuffer != null)
				photoData = AVCapturePhotoOutput.GetJpegPhotoDataRepresentation(photoSampleBuffer, previewPhotoSampleBuffer);
			else
				Console.WriteLine($"Error capturing photo: {error.LocalizedDescription}");
		}

		[Export("captureOutput:didFinishCaptureForResolvedSettings:error:")]
		public void DidFinishCapture(AVCapturePhotoOutput captureOutput, AVCaptureResolvedPhotoSettings resolvedSettings, NSError error)
			=> OnFinishCapture(photoData, error);
	}
}