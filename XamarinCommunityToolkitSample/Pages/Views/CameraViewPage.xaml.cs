using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class CameraViewPage : BasePage
	{
		public CameraViewPage()
		{
			InitializeComponent();
		}

		void ZoomSlider_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			cameraView.Zoom = (float)zoomSlider.Value;
			zoomLabel.Text = $"Zoom: {zoomSlider.Value}";
		}

		void Switch_Toggled(object sender, ToggledEventArgs e)
		{
			if (e.Value)
				cameraView.CaptureOptions = CameraCaptureOptions.Video;
			else
				cameraView.CaptureOptions = CameraCaptureOptions.Photo;

			doCameraThings.Text = e.Value ? "Start recording" : "Snap picture";
		}

		void DoCameraThings_Clicked(object sender, EventArgs e)
		{
			cameraView.Shutter();
			doCameraThings.Text = cameraView.CaptureOptions != CameraCaptureOptions.Video ? "Snap picture" : "Stop recording";
		}

		// TODO due to timing on iOS this never works
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
					previewMovie.IsVisible = false;
					previewPicture.IsVisible = true;
					previewPicture.Source = e.Image;
					doCameraThings.Text = "Snap picture";
					break;
				case CameraCaptureOptions.Video:
					previewPicture.IsVisible = false;
					previewMovie.IsVisible = true;
					previewMovie.Source = e.Video;
					doCameraThings.Text = "Start recording";
					break;
			}
		}
	}
}