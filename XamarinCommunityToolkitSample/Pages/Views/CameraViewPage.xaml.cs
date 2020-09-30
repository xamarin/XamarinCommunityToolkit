using System;
using Xamarin.CommunityToolkit.Sample.Resx;
using Xamarin.CommunityToolkit.Sample.ViewModels.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class CameraViewPage : BasePage
	{
		// Note: not all options of the CameraView are on this page, make sure to discover them for yourself!
		public CameraViewPage()
		{
			InitializeComponent();

			zoomLabel.Text = string.Format(AppResources.CameraViewSampleZoom, zoomSlider.Value);
			BindingContext = new CameraViewModel();
		}

		void ZoomSlider_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			cameraView.Zoom = (float)zoomSlider.Value;
			zoomLabel.Text = string.Format(AppResources.CameraViewSampleZoom, Math.Round(zoomSlider.Value));
		}

		void VideoSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			var captureVideo = e.Value;

			if (captureVideo)
				cameraView.CaptureOptions = CameraCaptureOptions.Video;
			else
				cameraView.CaptureOptions = CameraCaptureOptions.Photo;

			previewPicture.IsVisible = !captureVideo;

			doCameraThings.Text = e.Value ? AppResources.CameraViewSampleStartRecording
				: AppResources.CameraViewSampleSnapPicture;
		}

		// You can also set it to Default and External
		void FrontCameraSwitch_Toggled(object sender, ToggledEventArgs e)
			=> cameraView.CameraOptions = e.Value ? CameraOptions.Front : CameraOptions.Back;

		// You can also set it to Torch (always on) and Auto
		void FlashSwitch_Toggled(object sender, ToggledEventArgs e)
			=> cameraView.FlashMode = e.Value ? CameraFlashMode.On : CameraFlashMode.Off;

		void DoCameraThings_Clicked(object sender, EventArgs e)
		{
			cameraView.Shutter();
			doCameraThings.Text = cameraView.CaptureOptions == CameraCaptureOptions.Video
				? AppResources.CameraViewSampleStopRecording
				: AppResources.CameraViewSampleSnapPicture;
		}

		void CameraView_OnAvailable(object _, bool e)
		{
			if (e)
			{
				zoomSlider.Value = cameraView.Zoom;
				var max = cameraView.MaxZoom;
				if (max > zoomSlider.Minimum && max > zoomSlider.Value)
					zoomSlider.Maximum = max;
				else
					zoomSlider.Maximum = zoomSlider.Minimum + 1; // if max == min throws exception
			}

			doCameraThings.IsEnabled = e;
			zoomSlider.IsEnabled = e;
		}

		void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e)
		{
			switch (cameraView.CaptureOptions)
			{
				default:
				case CameraCaptureOptions.Default:
				case CameraCaptureOptions.Photo:
					previewPicture.IsVisible = true;
					previewPicture.Source = e.Image;
					doCameraThings.Text = AppResources.CameraViewSampleSnapPicture;
					break;
				case CameraCaptureOptions.Video:
					previewPicture.IsVisible = false;
					doCameraThings.Text = AppResources.CameraViewSampleStartRecording;
					break;
			}
		}
	}
}