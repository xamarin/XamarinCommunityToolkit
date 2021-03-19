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

			if (ApiInformation.IsTypePresent(typeof(StatusBar).FullName ?? string.Empty))
			{
				var statusBar = ViewManagement::StatusBar.GetForCurrentView();
				if (statusBar != null)
				{
					statusBar.BackgroundColor = windowsColor;
				}
			}
			else
			{
				var titleBar = ApplicationView.GetForCurrentView()?.TitleBar;
				if (titleBar != null)
				{
					titleBar.BackgroundColor = windowsColor;
				}
			}
		}

		static partial void SetStyle(StatusBarStyle style)
		{
			var foregroundColor = style switch
			{
				StatusBarStyle.LightContent => Color.White,
				StatusBarStyle.DarkContent => Color.Black,
				_ => Color.Default,
			};

			if (ApiInformation.IsTypePresent(typeof(StatusBar).FullName ?? string.Empty))
			{
				var statusBar = ViewManagement::StatusBar.GetForCurrentView();
				if (statusBar != null)
				{
					statusBar.ForegroundColor = foregroundColor.ToWindowsColor();
				}
			}
			else
			{
				var titleBar = ApplicationView.GetForCurrentView()?.TitleBar;
				if (titleBar != null)
				{
					titleBar.ForegroundColor = foregroundColor.ToWindowsColor();
				}
			}
		}
	}
}
