using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static partial class NavigationExtensions
	{
		static void PlatformShowPopup(BasePopup popup) =>
			Platform.CreateRendererWithContext(popup, ToolkitPlatform.Context);

		static Task<T?> PlatformShowPopupAsync<T>(Popup<T> popup)
		{
			PlatformShowPopup(popup);
			return popup.Result;
		}
	}
}