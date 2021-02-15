using System.ComponentModel;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(PlatformWindowEffect), nameof(WindowEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class PlatformWindowEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			SetStatusBarColor(WindowEffect.GetStatusBarColor(Element).ToUIColor());
			SetStatusBarStyle(WindowEffect.GetStatusBarStyle(Element));
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == WindowEffect.StatusBarColorProperty.PropertyName)
			{
				SetStatusBarColor(WindowEffect.GetStatusBarColor(Element).ToUIColor());
			}
			else if (args.PropertyName == WindowEffect.StatusBarStyleProperty.PropertyName)
			{
				SetStatusBarStyle(WindowEffect.GetStatusBarStyle(Element));
			}
		}

		public void SetStatusBarColor(UIColor color)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
			{
				foreach (var window in UIApplication.SharedApplication.Windows)
				{
					const int statusBarTag = 38482;
					var statusBar = window.ViewWithTag(statusBarTag) ?? new UIView(UIApplication.SharedApplication.StatusBarFrame);
					statusBar.Tag = statusBarTag;
					statusBar.BackgroundColor = color;
					statusBar.TintColor = color;
					window.AddSubview(statusBar);
				}
			}
			else
			{
				var statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
				if (statusBar != null && statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
				{
					statusBar.BackgroundColor = color;
				}
			}

			GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
		}

		public void SetStatusBarStyle(StatusBarStyle style)
		{
			switch (style)
			{
				case StatusBarStyle.Default:
					UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, false);
					break;
				case StatusBarStyle.LightContent:
					UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
					break;
				case StatusBarStyle.DarkContent:
					UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.DarkContent, false);
					break;
			}

			GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
		}

		UIViewController GetCurrentViewController()
		{
			var window = UIApplication.SharedApplication.KeyWindow;

			var vc = window.RootViewController;
			while (vc.PresentedViewController != null)
			{
				vc = vc.PresentedViewController;
			}
			return vc;
		}
	}
}
