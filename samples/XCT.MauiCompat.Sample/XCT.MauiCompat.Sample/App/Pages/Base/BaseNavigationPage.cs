using Maui = Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

namespace Xamarin.CommunityToolkit.Sample.Pages
{
	public class BaseNavigationPage : Maui.NavigationPage
	{
		public BaseNavigationPage(Maui.Page root)
			: base(root)
		{
			On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().SetPrefersHomeIndicatorAutoHidden(true);
			On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().SetModalPresentationStyle(Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific.UIModalPresentationStyle.FormSheet);
			On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().DisableTranslucentNavigationBar();
			On<Microsoft.Maui.Controls.PlatformConfiguration.iOS>().SetHideNavigationBarSeparator(true);
		}
	}
}