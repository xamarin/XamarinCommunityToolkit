using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System;using Microsoft.Extensions.Logging;
using Android.Views.Accessibility;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;
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

			view.SendAccessibilityEvent(EventTypes.ViewHoverEnter);
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