using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static partial class SemanticExtensions
	{
		static void PlatformSetSemanticFocus(this VisualElement element) =>
			throw new NotSupportedException($"The current platform '{Device.RuntimePlatform}' does not support Xamarin.CommunityToolkit.SemanticExtensions");

		static void PlatformAnnounce(string text) =>
			throw new NotSupportedException($"The current platform '{Device.RuntimePlatform}' does not support Xamarin.CommunityToolkit.SemanticExtensions");
	}
}