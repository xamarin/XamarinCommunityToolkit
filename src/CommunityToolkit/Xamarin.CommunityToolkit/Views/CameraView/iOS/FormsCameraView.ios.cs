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
		AVCaptureDeviceInput? input;
		AVCaptureStillImageOutput? imageOutput;
		AVCapturePhotoOutput? photoOutput;
		AVCaptureMovieFileOutput? videoOutput;
		AVCaptureDevice? device;
		AVCaptureDevicePosition? lastPosition;
		bool isBusy;
		bool isAvailable;
		bool isDisposed;
		CameraFlashMode flashMode;
		PhotoCaptureDelegate? photoCaptureDelegate;

		public event EventHandler<bool>? Busy;

		public event EventHandler<bool>? Available;

		public event EventHandler<Tuple<NSObject?, NSError?>>? FinishCapture;

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

			Add(mainView);

			AddConstraints(NSLayoutConstraint.FromVisualFormat("V:|[mainView]|", NSLayoutFormatOptions.DirectionLeftToRight, null, new NSDictionary("mainView", mainView)));
			AddConstraints(NSLayoutConstraint.FromVisualFormat("H:|[mainView]|", NSLayoutFormatOptions.AlignAllTop, null, new NSDictionary("mainView", mainView)));
		}

		internal void SetOrientation()
		{
			SetFrameOrientation();
			SetVideoOrientation();
		}

		void SetFrameOrientation()
		{
			try
			{
				var previewLayerFrame = previewLayer.Frame;
				previewLayerFrame.Height = mainView.Bounds.Height;
				previewLayerFrame.Width = mainView.Bounds.Width;
				previewLayer.Frame = previewLayerFrame;
			}
			catch (Exception error)
			{
				LogError("Failed to adjust frame", error);
			}
		}

		void SetVideoOrientation()
		{
			try
			{
				if (previewLayer?.Connection?.SupportsVideoOrientation == true)
					previewLayer.Connection.VideoOrientation = GetVideoOrientation();
			}
			catch (Exception error)
			{
				LogError("Failed to set video orientation", error);
			}
		}

		AVCaptureVideoOrientation GetVideoOrientation()
		{
			switch (UIApplication.SharedApplication.StatusBarOrientation)
			{
				case UIInterfaceOrientation.Portrait:
					return AVCaptureVideoOrientation.Portrait;
				case UIInterfaceOrientation.PortraitUpsideDown:
					return AVCaptureVideoOrientation.PortraitUpsideDown;
				case UIInterfaceOrientation.LandscapeLeft:
					return AVCaptureVideoOrientation.LandscapeLeft;
				case UIInterfaceOrientation.LandscapeRight:
					return AVCaptureVideoOrientation.LandscapeRight;
				default:
					throw new ArgumentOutOfRangeException(nameof(UIApplication.SharedApplication.StatusBarOrientation));
			}
		}

		void LogError(string message, Exception? error = null)
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
			if (isBusy || device == null)
				return;

			IsBusy = true;

			// iOS >= 10
			if (photoOutput != null)
			{
				try
				{
					var photoOutputConnection = photoOutput.ConnectionFromMediaType(AVMediaType.Video);
					if (photoOutputConnection != null)
						photoOutputConnection.VideoOrientation = previewLayer.Connection?.VideoOrientation ?? throw new NullReferenceException();

					photoOutput.CapturePhoto(GetCapturePhotoSettings(), GetPhotoCaptureDelegate());
				}
				catch (Exception)
				{
					FinishCapture?.Invoke(this, new Tuple<NSObject?, NSError?>(null, new NSError(new NSString("Failed to create image"), 0)));
					IsBusy = false;
				}
				return;
			}

			// iOS < 10
			try
			{
				var connection = imageOutput?.Connections[0] ?? throw new NullReferenceException();
				connection.VideoOrientation = previewLayer.Connection?.VideoOrientation ?? throw new NullReferenceException();
				var sampleBuffer = await imageOutput.CaptureStillImageTaskAsync(connection);
				var imageData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
				FinishCapture?.Invoke(this, new Tuple<NSObject?, NSError?>(imageData, null));
			}
			catch (Exception)
			{
				FinishCapture?.Invoke(this, new Tuple<NSObject?, NSError?>(null, new NSError(new NSString("Failed to create image"), 0)));
			}
			finally
			{
				IsBusy = false;
			}
		}

		AVCapturePhotoSettings GetCapturePhotoSettings()
		{
			var photoSettings = AVCapturePhotoSettings.Create();
			photoSettings.FlashMode = GetFlashMode();
			photoSettings.IsHighResolutionPhotoEnabled = true;
			return photoSettings;
		}

		PhotoCaptureDelegate GetPhotoCaptureDelegate()
		{
			if (photoCaptureDelegate == null)
			{
				photoCaptureDelegate = new PhotoCaptureDelegate
				{
					OnFinishCapture = (data, error) =>
					{
						FinishCapture?.Invoke(this, new Tuple<NSObject?, NSError?>(data, error));
						IsBusy = false;
					},
					WillCapturePhotoAnimation = () => Animate(0.25, () => previewLayer.Opacity = 1)
				};
			}
			return photoCaptureDelegate;
		}

		AVCaptureFlashMode GetFlashMode()
		{
			// Note: torch is set elsewhere
			switch (flashMode)
			{
				case CameraFlashMode.On:
					return AVCaptureFlashMode.On;
				case CameraFlashMode.Auto:
					return AVCaptureFlashMode.Auto;
				case CameraFlashMode.Off:
				default:
					return AVCaptureFlashMode.Off;
			}
		}

		string ConstructVideoFilename()
		{
			var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var library = Path.Combine(documents, "..", "Library");
			var timeStamp = DateTime.Now.ToString("yyyyddMM_HHmmss");
			return Path.Combine(library, $"VID_{timeStamp}.mov");
		}

		public void StartRecord()
		{
			if (isBusy || device == null || videoOutput?.Recording == true)
				return;

			captureSession.BeginConfiguration();

			videoOutput = new AVCaptureMovieFileOutput
			{
				MovieFragmentInterval = CMTime.Invalid
			};

			if (captureSession.CanAddOutput(videoOutput))
				captureSession.AddOutput(videoOutput);

			var audioDevice = AVCaptureDevice.GetDefaultDevice(AVMediaTypes.Audio);
			var audioInput = AVCaptureDeviceInput.FromDevice(audioDevice);

			if (captureSession.CanAddInput(audioInput))
				captureSession.AddInput(audioInput);

			captureSession.CommitConfiguration();

			IsBusy = true;
			try
			{
				videoOutput.Connections[0].VideoOrientation = previewLayer.Connection?.VideoOrientation ?? throw new NullReferenceException();
				var connection = videoOutput.Connections[0];

				if (connection.SupportsVideoOrientation)
					connection.VideoOrientation = previewLayer.Connection.VideoOrientation;
				if (connection.SupportsVideoStabilization)
					connection.PreferredVideoStabilizationMode = VideoStabilization ? AVCaptureVideoStabilizationMode.Auto : AVCaptureVideoStabilizationMode.Off;

				var outputFileURL = NSUrl.FromFilename(ConstructVideoFilename());

				videoOutput.StartRecordingToOutputFile(outputFileURL, this);
			}
			catch (Exception error)
			{
				LogError("Error with camera output capture", error);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public void StopRecord()
		{
			if (!isBusy && device != null && videoOutput != null && videoOutput.Recording)
			{
				IsBusy = true;
				videoOutput.StopRecording();
			}
		}

		public void FinishedRecording(AVCaptureFileOutput captureOutput, NSUrl outputFileUrl, NSObject[] connections, NSError? error)
		{
			FinishCapture?.Invoke(this, new Tuple<NSObject?, NSError?>(outputFileUrl, error));

			_ = videoOutput ?? throw new NullReferenceException();

			captureSession.RemoveOutput(videoOutput);
			videoOutput = null;
			IsBusy = false;
		}

		public void SwitchFlash(CameraFlashMode newFlashMode)
		{
			if (isAvailable && device != null && newFlashMode != flashMode)
				UpdateFlash(newFlashMode);
		}

		void UpdateFlash(CameraFlashMode? newFlashMode = null)
		{
			try
			{
				var desiredFlashMode = newFlashMode ?? flashMode;

				_ = device ?? throw new NullReferenceException();
				device.LockForConfiguration(out var err);

				if (CheckFlashModeSupported(desiredFlashMode))
				{
					switch (desiredFlashMode)
					{
						default:
						case CameraFlashMode.Off:
							device.FlashMode = AVCaptureFlashMode.Off;
							break;
						case CameraFlashMode.On:
							device.FlashMode = AVCaptureFlashMode.On;
							break;
						case CameraFlashMode.Auto:
							device.FlashMode = AVCaptureFlashMode.Auto;
							break;
						case CameraFlashMode.Torch:
							device.TorchMode = AVCaptureTorchMode.On;
							break;
					}

					flashMode = desiredFlashMode;
				}

				if (desiredFlashMode != CameraFlashMode.Torch
					&& device.TorchMode == AVCaptureTorchMode.On
					&& device.IsTorchModeSupported(AVCaptureTorchMode.Off))
				{
					device.TorchMode = AVCaptureTorchMode.Off;
				}

				device.UnlockForConfiguration();
			}
			catch (Exception error)
			{
				LogError("Failed to switch flash on/off", error);
			}
		}

		bool CheckFlashModeSupported(CameraFlashMode flashMode)
		{
			_ = device ?? throw new NullReferenceException();

			return flashMode switch
			{
				CameraFlashMode.Off => device.IsFlashModeSupported(AVCaptureFlashMode.Off),
				CameraFlashMode.On => device.IsFlashModeSupported(AVCaptureFlashMode.On),
				CameraFlashMode.Auto => device.IsFlashModeSupported(AVCaptureFlashMode.Auto),
				CameraFlashMode.Torch => device.IsTorchModeSupported(AVCaptureTorchMode.On),
				_ => device.IsFlashModeSupported(AVCaptureFlashMode.Off)
			};
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
				_ = device ?? throw new NullReferenceException();
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
					AVCaptureDevice.RequestAccessForMediaType(AVMediaType.Video, granted =>
						InvokeOnMainThread(() => RetrieveCameraDevice(cameraOptions)));
					return;
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

			// Cache the last position requested, so we only initialize the camera if it's changed
			if (position == lastPosition)
				return;
			lastPosition = position;

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
			UpdateFlash();
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
					SetOrientation();
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

			photoCaptureDelegate?.Dispose();
			photoCaptureDelegate = null;

			input?.Dispose();
			input = null;

			imageOutput?.Dispose();
			imageOutput = null;

			photoOutput?.Dispose();
			photoOutput = null;

			videoOutput?.Dispose();
			videoOutput = null;
		}

		protected override void Dispose(bool disposing)
		{
			if (isDisposed)
				return;

			isDisposed = true;

			if (device?.TorchMode == AVCaptureTorchMode.On)
			{
				flashMode = CameraFlashMode.Off;
				UpdateFlash();
			}

			ClearCaptureSession();
			captureSession?.Dispose();

			base.Dispose(disposing);
		}
	}

	class PhotoCaptureDelegate : NSObject, IAVCapturePhotoCaptureDelegate
	{
		public Action<NSData?, NSError>? OnFinishCapture;
		public Action? WillCapturePhotoAnimation;

		NSData? photoData;

		[Export("captureOutput:willCapturePhotoForResolvedSettings:")]
		public void WillCapturePhoto(AVCapturePhotoOutput captureOutput, AVCaptureResolvedPhotoSettings resolvedSettings) => WillCapturePhotoAnimation?.Invoke();

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
			=> OnFinishCapture?.Invoke(photoData, error);
	}
}