using System;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class MediaElementPage
	{
		public MediaElementPage() => InitializeComponent();

		void OnMediaOpened(object sender, EventArgs e) => Console.WriteLine("Media opened.");

		void OnMediaFailed(object sender, EventArgs e) => Console.WriteLine("Media failed.");

		void OnMediaEnded(object sender, EventArgs e) => Console.WriteLine("Media ended.");

		void OnSeekCompleted(object sender, EventArgs e) => Console.WriteLine("Seek completed.");

		void OnResetClicked(object sender, EventArgs e) => mediaElement.Source = null;
	}
}