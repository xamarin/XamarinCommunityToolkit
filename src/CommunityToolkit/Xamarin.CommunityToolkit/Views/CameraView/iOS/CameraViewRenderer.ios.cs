using System;
using System.Diagnostics;
using AVFoundation;
using Foundation;
using Photos;
using UIKit;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class CameraViewRenderer : ViewRenderer<CameraView, FormsCameraView>
	{
		bool disposed;

		protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			base.OnElementChanged(e);

			if (Control == null && !disposed)
			{
				SetNativeControl(new FormsCameraView());

				_ = Control ?? throw new NullReferenceException($"{nameof(Control)} cannot be null");
				Control.Busy += OnBusy;
				Control.Available += OnAvailability;
				Control.FinishCapture += FinishCapture;

				Control.SwitchFlash(Element.FlashMode);
				Control.SetBounds(Element.WidthRequest, Element.HeightRequest);
				Control.VideoStabilization = Element.VideoStabilization;
				Control.Zoom = (float)Element.Zoom;
				Control.RetrieveCameraDevice(Element.CameraOptions);
			}

			if (e.OldElement != null)
			{
				e.OldElement.ShutterClicked -= HandleShutter;
			}

			if (e.NewElement != null)
			{
				e.NewElement.ShutterClicked += HandleShutter;
			}
		}

		void OnBusy(object? sender, bool busy) => Element.IsBusy = busy;

		void OnAvailability(object? sender, bool available)
		{
			Element.MaxZoom = Control.MaxZoom;
			Element.IsAvailable = available;
		}

		void FinishCapture(object? sender, Tuple<NSObject?, NSError?> e)
		{
			if (Element == null || Control == null)
				return;

			if (e.Item2 != null)
			{
				Element.RaiseMediaCaptureFailed(e.Item2.LocalizedDescription);
				return;
			}

			var photoData = e.Item1 as NSData;

			// See TODO on CameraView.SavePhotoToFile
			// if (!Element.SavePhotoToFile && photoData != null)
			if (photoData != null)
			{
				var data = UIImage.LoadFromData(photoData)?.AsJPEG().ToArray();
				Device.BeginInvokeOnMainThread(() =>
				{
					Element.RaiseMediaCaptured(new MediaCapturedEventArgs(imageData: data));
				});
				return;
			}

			PHObjectPlaceholder? placeholder = null;
			PHPhotoLibrary.RequestAuthorization(status =>
			{
				if (status != PHAuthorizationStatus.Authorized)
					return;

				// Save the captured file to the photo library.
				PHPhotoLibrary.SharedPhotoLibrary.PerformChanges(() =>
				{
					var creationRequest = PHAssetCreationRequest.CreationRequestForAsset();
					if (photoData != null)
					{
						creationRequest.AddResource(PHAssetResourceType.Photo, photoData, null);
						placeholder = creationRequest.PlaceholderForCreatedAsset;
					}
					else if (e.Item1 is NSUrl outputFileUrl)
					{
						creationRequest.AddResource(
							PHAssetResourceType.Video,
							outputFileUrl,
							new PHAssetResourceCreationOptions
							{
								ShouldMoveFile = true
							});
						placeholder = creationRequest.PlaceholderForCreatedAsset;
					}
				}, (success2, error2) =>
				{
					if (!success2)
					{
						Debug.WriteLine($"Could not save media to photo library: {error2}");
						if (error2 != null)
						{
							Element.RaiseMediaCaptureFailed(error2.LocalizedDescription);
							return;
						}
						Element.RaiseMediaCaptureFailed($"Could not save media to photo library");
						return;
					}

					_ = placeholder ?? throw new NullReferenceException();
					if (PHAsset.FetchAssetsUsingLocalIdentifiers(new[] { placeholder.LocalIdentifier }, null).firstObject is not PHAsset asset)
					{
						Element.RaiseMediaCaptureFailed($"Could not save media to photo library");
						return;
					}
					if (asset.MediaType == PHAssetMediaType.Image)
					{
						asset.RequestContentEditingInput(new PHContentEditingInputRequestOptions
						{
							CanHandleAdjustmentData = p => true
						}, (input, info) =>
						{
							Device.BeginInvokeOnMainThread(() =>
							{
								Element.RaiseMediaCaptured(new MediaCapturedEventArgs(input.FullSizeImageUrl?.Path));
							});
						});
					}
					else if (asset.MediaType == PHAssetMediaType.Video)
					{
						PHImageManager.DefaultManager.RequestAvAsset(asset, new PHVideoRequestOptions
						{
							Version = PHVideoRequestOptionsVersion.Original
						}, (avAsset, mix, info) =>
						{
							if (avAsset is not AVUrlAsset urlAsset)
							{
								Element.RaiseMediaCaptureFailed($"Could not save media to photo library");
								return;
							}
							Device.BeginInvokeOnMainThread(() =>
							{
								Element.RaiseMediaCaptured(new MediaCapturedEventArgs(urlAsset.Url.Path));
							});
						});
					}
				});
			});
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			disposed = true;
			Control?.Dispose();
			base.Dispose(disposing);
		}

		protected override void OnElementPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (Element == null || Control == null)
				return;

			switch (e.PropertyName)
			{
				case nameof(CameraView.CameraOptions):
					Control.RetrieveCameraDevice(Element.CameraOptions);
					break;
				case nameof(VisualElement.HeightProperty):
				case nameof(VisualElement.WidthProperty):
					Control.SetBounds(Element.Width, Element.Height);
					break;
				case nameof(CameraView.VideoStabilization):
					Control.VideoStabilization = Element.VideoStabilization;
					break;
				case nameof(CameraView.FlashMode):
					Control.SwitchFlash(Element.FlashMode);
					break;
				case nameof(CameraView.Zoom):
					Control.Zoom = (float)Element.Zoom;
					break;
				case nameof(CameraView.Height):
				case nameof(CameraView.Width):
					Control.SetBounds(Element.Width, Element.Height);
					Control.SetCameraOrientation();
					break;
			}
		}

		async void HandleShutter(object? sender, EventArgs e)
		{
			switch (Element.CaptureMode)
			{
				case CameraCaptureMode.Default:
				case CameraCaptureMode.Photo:
					if (Control != null)
						await Control.TakePhoto();
					break;
				case CameraCaptureMode.Video:
					if (Control == null)
						return;

					if (!Control.VideoRecorded)
						Control.StartRecord();
					else
						Control.StopRecord();
					break;
			}
		}
	}
}