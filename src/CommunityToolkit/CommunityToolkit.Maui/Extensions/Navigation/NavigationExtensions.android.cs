using System.Threading.Tasks;
using CommunityToolkit.Maui.UI.Views;
using Xamarin.Forms.Platform.Android;

namespace CommunityToolkit.Maui.Extensions
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