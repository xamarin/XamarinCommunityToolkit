using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Sample.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Sample.Pages.TestCases
{
	public partial class LinkerCameraViewPage
	{
		int counter = 0;

		public LinkerCameraViewPage()
		{
			InitializeComponent();
			BindingContext = new CameraViewTestViewModel();
		}

		public void CameraView_OnAvailable(object sender, bool e)
		{
			shutterButton.IsEnabled = e;
		}

		public void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e)
		{
			shutterButton.Text = $"{++counter}";
			previewPicture.Source = e.Image;
			previewPicture.Rotation = e.Rotation;
		}

		void ShutterButtonClicked(object sender, EventArgs e)
		{
			cameraView.Shutter();
		}
	}

	sealed class CameraViewTestViewModel : BaseViewModel
	{
		public Command<object> CaptureCommand { get; }

		public CameraViewTestViewModel()
		{
			CaptureCommand = new Command<object>(CaptureCommandExecute);
		}

		void CaptureCommandExecute(object args)
		{
			Console.WriteLine(args);
		}
	}
}