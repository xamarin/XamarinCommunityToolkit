using System;
using System.ComponentModel;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(PlatformWindow), nameof(Window))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class PlatformWindow : PlatformEffect
	{
		protected override void OnAttached()
		{
			SetStatusBarColor(Window.GetStatusBarColor(Element).ToUIColor());
			SetStatusBarStyle(Window.GetStatusBarStyle(Element));
		}

		protected override void OnDetached()
		{
			if (Window.GetStatusBarColor(Element) != (Color)Window.StatusBarColorProperty.DefaultValue)
			{
				SetStatusBarColor(UIColor.Black);
			}
			if (Window.GetStatusBarStyle(Element) != (StatusBarStyle)Window.StatusBarStyleProperty.DefaultValue)
			{
				SetStatusBarStyle(StatusBarStyle.Default);
			}
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == Window.StatusBarColorProperty.PropertyName)
			{
				SetStatusBarColor(Window.GetStatusBarColor(Element).ToUIColor());
			}
			else if (args.PropertyName == Window.StatusBarStyleProperty.PropertyName)
			{
				SetStatusBarStyle(Window.GetStatusBarStyle(Element));
			}
		}

		public void SetStatusBarColor(UIColor color)
		{
		}

		public void SetStatusBarStyle(StatusBarStyle style)
		{
		}
	}
}
