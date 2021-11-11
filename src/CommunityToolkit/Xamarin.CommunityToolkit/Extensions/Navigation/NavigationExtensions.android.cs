using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static partial class NavigationExtensions
	{
		static void PlatformShowPopup(BasePopup popup) =>
			Platform.CreateRendererWithContext(popup, XCT.Context);

		static Task<T?> PlatformShowPopupAsync<T>(Popup<T> popup)
		{
			PlatformShowPopup(popup);
			return popup.Result;
		}
	}
}