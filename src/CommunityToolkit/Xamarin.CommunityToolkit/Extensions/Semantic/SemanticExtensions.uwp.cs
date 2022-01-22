﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Media;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static partial class SemanticExtensions
	{
		internal const string ActivityId = "270FA098-C644-40A2-A0BE-A9BEA1222A1E";

		static void PlatformSetSemanticFocus(this VisualElement element) =>
			throw new NotSupportedException($"The current platform '{Device.RuntimePlatform}' does not support Xamarin.CommunityToolkit.SemanticExtensions");

		static void PlatformAnnounce(string text)
		{
			if (Window.Current == null)
				return;

			var peer = FindAutomationPeer(Window.Current.Content);

			// This GUID correlates to the internal messages used by UIA to perform an announce
			// You can extract it  by using accessibility insights to monitor UIA events
			// If you're curious how this works then do a google search for the GUID
			peer?.RaiseNotificationEvent(
				AutomationNotificationKind.ActionAborted,
				AutomationNotificationProcessing.ImportantMostRecent,
				text,
				ActivityId);
		}

		// This isn't great but it's the only way I've found to announce with WinUI.
		// You have to locate a control that has an automation peer and then use that 
		// to perform the announce operation. This creates scenarios where the 
		// screen might not have any automation peers on it to use but in those cases
		// you really shouldn't be using the announce API
		static AutomationPeer? FindAutomationPeer(DependencyObject depObj)
		{
			if (depObj == null)
				return null;

			var count = VisualTreeHelper.GetChildrenCount(depObj);
			for (var i = 0; i < count; i++)
			{
				var child = VisualTreeHelper.GetChild(depObj, i);
				if (child is UIElement element && FrameworkElementAutomationPeer.FromElement(element) != null)
				{
					return FrameworkElementAutomationPeer.FromElement(element);
				}

				var childItem = FindAutomationPeer(child);
				if (childItem != null)
					return childItem;
			}
			return null;
		}
	}
}