using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static partial class NavigationExtensions
	{
		static void PlatformShowPopup(BasePopup popup) =>
			throw new NotSupportedException($"The current platform '{Device.RuntimePlatform}' does not support Xamarin.CommunityToolkit.BasePopup");

		static Task<T> PlatformShowPopupAsync<T>(Popup<T> popup) =>
			throw new NotSupportedException($"The current platform '{Device.RuntimePlatform}' does not support Xamarin.CommunityToolkit.Popup.");
	}
}