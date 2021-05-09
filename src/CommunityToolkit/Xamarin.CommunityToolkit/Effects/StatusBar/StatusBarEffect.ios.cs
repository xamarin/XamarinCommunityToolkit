using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.Multiplatform
{
	public static partial class StatusBar
	{
		static partial void PlatformSetColor(Color color)
		{
			var uiColor = color.ToUIColor();

			if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
			{
				const int statusBarTag = 38482;
				foreach (var window in UIApplication.SharedApplication.Windows)
				{
					var statusBar = window.ViewWithTag(statusBarTag) ?? new UIView(UIApplication.SharedApplication.StatusBarFrame);
					statusBar.Tag = statusBarTag;
					statusBar.BackgroundColor = uiColor;
					statusBar.TintColor = uiColor;
					window.AddSubview(statusBar);

					UpdateStatusBarAppearance(window);
				}
			}
			else
			{
				var statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
				if (statusBar != null && statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
				{
					statusBar.BackgroundColor = uiColor;
				}

				UpdateStatusBarAppearance();
			}
		}

		static partial void PlatformSetStyle(StatusBarStyle style)
		{
			var uiStyle = style switch
			{
				StatusBarStyle.LightContent => UIStatusBarStyle.LightContent,
				StatusBarStyle.DarkContent => UIStatusBarStyle.DarkContent,
				_ => UIStatusBarStyle.Default
			};
			UIApplication.SharedApplication.SetStatusBarStyle(uiStyle, false);

			UpdateStatusBarAppearance();
		}

		static void UpdateStatusBarAppearance()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
			{
				foreach (var window in UIApplication.SharedApplication.Windows)
				{
					UpdateStatusBarAppearance(window);
				}
			}
			else
			{
				var window = UIApplication.SharedApplication.KeyWindow;
				UpdateStatusBarAppearance(window);
			}
		}

		static void UpdateStatusBarAppearance(UIWindow window)
		{
			var vc = window.RootViewController ?? throw new NullReferenceException(nameof(window.RootViewController));
			while (vc.PresentedViewController != null)
			{
				vc = vc.PresentedViewController;
			}

			vc.SetNeedsStatusBarAppearanceUpdate();
		}
	}
}
