using System;
using Xamarin.Forms.Platform.GTK;

namespace Xamarin.CommunityToolkit.Sample.GTK
{
	public static class Program
	{
		[STAThread]
		public static void Main()
		{
			Gtk.Application.Init();
			Forms.Forms.Init();

			var app = new App();
			var window = new FormsWindow();
			window.LoadApplication(app);
			window.SetApplicationTitle("Xamarin Community Toolkit");
			window.Show();

			Gtk.Application.Run();
		}
	}
}