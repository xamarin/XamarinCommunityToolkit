using System;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using ViewManagement = Windows.UI.ViewManagement;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class StatusBar
	{
		static partial void SetColor(Color color)
		{
			var windowsColor = color.ToWindowsColor();
			UpdateStatusBar(
				sb => sb.BackgroundColor = windowsColor,
				tb => tb.BackgroundColor = windowsColor);
		}

		static partial void SetStyle(StatusBarStyle style)
		{
			var foregroundColor = style switch
			{
				StatusBarStyle.LightContent => Color.White.ToWindowsColor(),
				StatusBarStyle.DarkContent => Color.Black.ToWindowsColor(),
				_ => Color.Default.ToWindowsColor(),
			};

			UpdateStatusBar(
				sb => sb.ForegroundColor = foregroundColor,
				tb => tb.ForegroundColor = foregroundColor);
		}

		static void UpdateStatusBar(Action<ViewManagement::StatusBar> updateStatusBar, Action<ApplicationViewTitleBar> updateTitleBar)
		{
			if (ApiInformation.IsTypePresent(typeof(StatusBar).FullName ?? string.Empty))
			{
				var statusBar = ViewManagement::StatusBar.GetForCurrentView();
				if (statusBar != null)
				{
					updateStatusBar(statusBar);
				}
			}
			else
			{
				var titleBar = ApplicationView.GetForCurrentView()?.TitleBar;
				if (titleBar != null)
				{
					updateTitleBar(titleBar);
				}
			}
		}
	}
}
