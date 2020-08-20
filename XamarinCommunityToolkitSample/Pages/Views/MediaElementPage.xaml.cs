using System;
using System.Diagnostics;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class MediaElementPage : BasePage
	{
		public MediaElementPage() => InitializeComponent();

		void OnMediaOpened(object sender, EventArgs e) => Debug.WriteLine("Media opened");

		void OnMediaEnded(object sender, EventArgs e) => Debug.WriteLine("Media ended");

		void OnMediaFailed(object sender, EventArgs e) => Debug.WriteLine("Media failed");
	}
}