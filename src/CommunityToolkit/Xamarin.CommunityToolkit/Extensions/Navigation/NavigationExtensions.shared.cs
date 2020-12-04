using System;
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
	}
}
