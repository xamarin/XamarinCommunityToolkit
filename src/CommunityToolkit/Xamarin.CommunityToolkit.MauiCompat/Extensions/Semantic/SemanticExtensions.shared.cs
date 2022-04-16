using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static partial class SemanticExtensions
	{
		/// <summary>
		/// Force semantic screen reader focus to specified element
		/// </summary>
		/// <param name="element"></param>
		public static void SetSemanticFocus(this VisualElement element) =>
			PlatformSetSemanticFocus(element);

		public static void Announce(string text) =>
			PlatformAnnounce(text);
	}
}