using System;
using System.ComponentModel;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
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
			if (args.PropertyName == StatusBarEffect.ColorProperty.PropertyName)
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