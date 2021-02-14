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
			if (WindowEffect.GetStatusBarColor(Element) != (Color)WindowEffect.StatusBarColorProperty.DefaultValue)
			{
				SetStatusBarColor(UIColor.Black);
			}
			if (WindowEffect.GetStatusBarStyle(Element) != (StatusBarStyle)WindowEffect.StatusBarStyleProperty.DefaultValue)
			{
				SetStatusBarStyle(StatusBarStyle.Default);
			}
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
		}

		public void SetStatusBarStyle(StatusBarStyle style)
		{
		}
	}
}
