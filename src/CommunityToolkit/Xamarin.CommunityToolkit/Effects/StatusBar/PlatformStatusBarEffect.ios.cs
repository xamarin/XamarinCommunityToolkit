using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(PlatformStatusBarEffect), nameof(StatusBarEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class PlatformStatusBarEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			SetColor(StatusBarEffect.GetColor(Element));
			SetStyle(StatusBarEffect.GetStyle(Element));
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == StatusBarEffect.ColorProperty.PropertyName
				|| args.PropertyName == View.HeightProperty.PropertyName
				|| args.PropertyName == View.WidthProperty.PropertyName)
			{
				SetColor(StatusBarEffect.GetColor(Element));
			}
			else if (args.PropertyName == StatusBarEffect.StyleProperty.PropertyName)
			{
				SetStyle(StatusBarEffect.GetStyle(Element));
			}
		}

		static void SetColor(Color color)
		{
			var uiColor = color.ToUIColor();

			if (XCT.IsiOS13OrNewer)
			{
				const int statusBarTag = 38482;
				foreach (var window in UIApplication.SharedApplication.Windows)
				{
					var statusBar = window.ViewWithTag(statusBarTag) ?? new UIView(UIApplication.SharedApplication.StatusBarFrame);
					statusBar.Tag = statusBarTag;
					statusBar.BackgroundColor = uiColor;
					statusBar.TintColor = uiColor;
					statusBar.Frame = UIApplication.SharedApplication.StatusBarFrame;
					window.AddSubview(statusBar);

					UpdateStatusBarAppearance(window);
				}
			}
			else
			{
				if (UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) is UIView statusBar
					&& statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
				{
					statusBar.BackgroundColor = uiColor;
				}

				UpdateStatusBarAppearance();
			}
		}

		static void SetStyle(StatusBarStyle style)
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
			if (XCT.IsiOS13OrNewer)
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

		static void UpdateStatusBarAppearance(UIWindow? window)
		{
			var vc = window?.RootViewController ?? throw new NullReferenceException(nameof(window.RootViewController));
			while (vc.PresentedViewController != null)
			{
				vc = vc.PresentedViewController;
			}

			vc.SetNeedsStatusBarAppearanceUpdate();
		}
	}
}