using System;
using Android.Views.Accessibility;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AApplication = Android.App.Application;
using AContext = Android.Content.Context;
using AView = Android.Views.View;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static partial class SemanticExtensions
	{
		static void PlatformSetSemanticFocus(this VisualElement element)
		{
			var androidView = Platform.GetRenderer(element);

			if (androidView is not AView view)
				throw new NullReferenceException("Can't access view");

			view.SendAccessibilityEvent((EventTypes)(int)WindowsChange.AccessibilityFocused);
		}

		static void PlatformAnnounce(string text)
		{
			var manager = AApplication.Context.GetSystemService(AContext.AccessibilityService) as AccessibilityManager;
			var announcement = AccessibilityEvent.Obtain();

			if (manager == null || announcement == null)
				return;

			if (!(manager.IsEnabled || manager.IsTouchExplorationEnabled))
				return;

			announcement.EventType = EventTypes.Announcement;
			announcement.Text?.Add(new Java.Lang.String(text));
			manager.SendAccessibilityEvent(announcement);
		}
	}
}
