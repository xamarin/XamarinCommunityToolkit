using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.Popups
{
	static class PopupSize
	{
		public static Size Android => new Size(0.9 * DeviceDisplay.MainDisplayInfo.Width,
									   0.6 * DeviceDisplay.MainDisplayInfo.Height);

		public static Size iOS => new Size(0.9 * DeviceDisplay.MainDisplayInfo.Width,
									   0.6 * DeviceDisplay.MainDisplayInfo.Height);

		public static Size UWP => new Size(0.9 * DeviceDisplay.MainDisplayInfo.Width,
									   0.6 * DeviceDisplay.MainDisplayInfo.Height);
	}
}
