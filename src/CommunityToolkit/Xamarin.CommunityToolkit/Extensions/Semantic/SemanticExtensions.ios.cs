using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

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
