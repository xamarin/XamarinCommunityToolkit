using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class CameraViewModel : BaseViewModel
	{
		public Command ShutterCommand { get; }

		// The dev should have the set in this property
		public Command CameraShutterCommand { get; set; }

		public CameraViewModel()
		{
			ShutterCommand = new Command(Shutter);
			CameraShutterCommand = new Command(DoNothing);
		}

		void Shutter() => CameraShutterCommand?.Execute(null);

		void DoNothing() =>
			Console.WriteLine("This is just to prove that the user can't override the CameraViewShutterCommand");
	}
}
