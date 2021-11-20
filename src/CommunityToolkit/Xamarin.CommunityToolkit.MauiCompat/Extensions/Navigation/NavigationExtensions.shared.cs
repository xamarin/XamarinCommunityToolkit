using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

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
		public static void ShowPopup(this INavigation navigation, BasePopup popup) =>
			PlatformShowPopup(popup);

		/// <summary>
		/// Displays a popup and returns a result.
		/// </summary>
		/// <typeparam name="T">
		/// The <typeparamref name="T"/> result that is returned when the popup is dismissed.
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
		public static Task<T?> ShowPopupAsync<T>(this INavigation navigation, Popup<T?> popup) =>
			PlatformShowPopupAsync(popup);
	}
}