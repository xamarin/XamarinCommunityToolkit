using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Platform.iOS;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static partial class NavigationExtensions
	{
		static void OnShowPopup(BasePopup popup)
		{
			Platform.CreateRenderer(popup);
		}
	}
}
