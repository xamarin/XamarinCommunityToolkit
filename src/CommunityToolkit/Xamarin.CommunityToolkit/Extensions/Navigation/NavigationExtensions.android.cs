using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static partial class NavigationExtensions
	{
		static void OnShowPopup(BasePopup popup)
		{
			Platform.CreateRendererWithContext(popup, ToolkitPlatform.Context);
		}

		static Task<T> OnShowPopupTask<T>(Popup<T> popup)
		{
			OnShowPopup(popup);
			return popup.Result;
		}
	}
}
