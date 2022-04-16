using System;using Microsoft.Extensions.Logging;
using Foundation;
using UIKit;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static partial class SemanticExtensions
	{
		static void PlatformSetSemanticFocus(this VisualElement element)
		{
			var iosView = Platform.GetRenderer(element);

			if (iosView == null)
				throw new NullReferenceException("Can't access view");

			if (iosView is not NSObject nativeView)
				return;

			UIAccessibility.PostNotification(UIAccessibilityPostNotification.LayoutChanged, nativeView);
		}

		static void PlatformAnnounce(string text)
		{
			if (!UIAccessibility.IsVoiceOverRunning)
				return;

			UIAccessibility.PostNotification(UIAccessibilityPostNotification.Announcement, new NSString(text));
		}
	}
}