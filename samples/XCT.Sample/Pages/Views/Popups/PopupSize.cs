using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.Popups
{
	static class PopupSize
	{
		public static Size Android => new Size(1000, 1200);

 		public static Size iOS => new Size(250, 350);
		
		public static Size UWP => new Size(300, 400);
	}
}
