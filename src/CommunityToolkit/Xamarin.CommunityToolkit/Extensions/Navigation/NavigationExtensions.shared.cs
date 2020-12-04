using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="INavigation"/>.
	/// </summary>
	public static partial class NavigationExtensions
	{
		/// <summary>
		/// Displays a popup.
		/// </summary>
		/// <param name="navigation">
		/// The current <see cref="INavigation"/>.
		/// </param>
		/// <param name="popup">
		/// The <see cref="BasePopup"/> to display.
		/// </param>
		public static void ShowPopup(this INavigation navigation, BasePopup popup)
		{
#if __ANDROID__
			OnShowPopup(popup);
#elif __IOS__
			OnShowPopup(popup);
#elif WINDOWS_UWP
			OnShowPopup(popup);
#else
			throw new NotSupportedException($"The current platform '{Device.RuntimePlatform}' does not support Xamarin Community Toolkit Popups.");
#endif
		}

		/// <summary>
		/// Displays a popup and returns a result.
		/// </summary>
		/// <typeparam name="T">
		/// The <see cref="T"/> result that is returned when the popup is dismissed.
		/// </typeparam>
		/// <param name="navigation">
		/// The current <see cref="INavigation"/>.
		/// </param>
		/// <param name="popup">
		/// The <see cref="Popup{T}"/> to display.
		/// </param>
		/// <returns>
		/// A task that will complete once the <see cref="Popup{T}"/> is dismissed.
		/// </returns>
		public static Task<T> ShowPopupAsync<T>(this INavigation navigation, Popup<T> popup)
		{
#if __ANDROID__
			return OnShowPopupAsync(popup);
#elif __IOS__
			return OnShowPopupAsync(popup);
#elif WINDOWS_UWP
			return OnShowPopupAsync(popup);
#else
			throw new NotSupportedException($"The current platform '{Device.RuntimePlatform}' does not support Xamarin Community Toolkit Popups.");
#endif
		}
	}
}
