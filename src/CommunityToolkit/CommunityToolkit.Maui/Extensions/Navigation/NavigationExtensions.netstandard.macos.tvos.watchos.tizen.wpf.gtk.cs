using System;
using System.Threading.Tasks;
using CommunityToolkit.Maui.UI.Views;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.Extensions
{
	public static partial class NavigationExtensions
	{
		static void PlatformShowPopup(BasePopup popup) =>
			throw new NotSupportedException($"The current platform '{Device.RuntimePlatform}' does not support CommunityToolkit.Maui.BasePopup");

		static Task<T> PlatformShowPopupAsync<T>(Popup<T> popup) =>
			throw new NotSupportedException($"The current platform '{Device.RuntimePlatform}' does not support CommunityToolkit.Maui.Popup.");
	}
}