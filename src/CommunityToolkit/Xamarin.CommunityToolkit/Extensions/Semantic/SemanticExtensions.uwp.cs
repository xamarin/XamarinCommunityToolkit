using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Extensions
{
	public static partial class SemanticExtensions
	{
		static void PlatformSetSemanticFocus(this VisualElement element) =>
			throw new NotSupportedException($"The current platform '{Device.RuntimePlatform}' does not support Xamarin.CommunityToolkit.SemanticExtensions");


		static void PlatformAnnounce(string text) =>
			throw new NotSupportedException($"The current platform '{Device.RuntimePlatform}' does not support Xamarin.CommunityToolkit.SemanticExtensions");
		/*{
			if (Application.MainWindow == null)
				return;

			var peer = FindAutomationPeer(Platform.CurrentWindow.Content);

			// This GUID correlates to the internal messages used by UIA to perform an announce
			// You can extract it  by using accessibility insights to monitor UIA events
			// If you're curious how this works then do a google search for the GUID
			peer.RaiseNotificationEvent(
				AutomationNotificationKind.ActionAborted,
				AutomationNotificationProcessing.ImportantMostRecent,
				text,
				"270FA098-C644-40A2-A0BE-A9BEA1222A1E");
		}*/
	}
}
