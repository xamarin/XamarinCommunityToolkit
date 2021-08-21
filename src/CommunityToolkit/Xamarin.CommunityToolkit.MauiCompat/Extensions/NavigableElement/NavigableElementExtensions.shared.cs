using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="NavigableElement"/>
	/// </summary>
	public static class NavigableElementExtensions
	{
		/// <summary>
		/// Displays a popup.
		/// </summary>
		/// <param name="element">
		/// The current <see cref="NavigableElement"/> that has a valid <see cref="INavigation"/>.
		/// </param>
		/// <param name="popup">
		/// The <see cref="BasePopup"/> to display.
		/// </param>
		public static void ShowPopup(this NavigableElement element, BasePopup popup) =>
			element.Navigation.ShowPopup(popup);

		/// <summary>
		/// Displays a poup and returns a result.
		/// </summary>
		/// <typeparam name="T">
		/// The <typeparamref name="T"/> result that is returned when the popup is dismissed.
		/// </typeparam>
		/// <param name="element">
		/// The current <see cref="NavigableElement"/> that has a valid <see cref="INavigation"/>.
		/// </param>
		/// <param name="popup">
		/// The <see cref="Popup{T}"/> to display.
		/// </param>
		/// <returns>
		/// A task that will complete once the <see cref="Popup{T}"/> is dismissed.
		/// </returns>
		public static Task<T?> ShowPopupAsync<T>(this NavigableElement element, Popup<T?> popup) =>
			element.Navigation.ShowPopupAsync(popup);
	}
}