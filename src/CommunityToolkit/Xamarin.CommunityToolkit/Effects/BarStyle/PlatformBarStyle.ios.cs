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

[assembly: ExportEffect(typeof(PlatformBarStyle), nameof(BarStyle))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class PlatformBarStyle : PlatformEffect
	{
		protected override void OnAttached()
		{
			SetStatusBarColor(BarStyle.GetStatusBarColor(Element).ToUIColor());
			SetStatusBarStyle(BarStyle.GetStatusBarStyle(Element));
		}

		protected override void OnDetached()
		{
			if (BarStyle.GetStatusBarColor(Element) != (Forms.Color)BarStyle.StatusBarColorProperty.DefaultValue)
			{
				SetStatusBarColor(UIColor.Black);
			}
			if (BarStyle.GetStatusBarStyle(Element) != (StatusBarStyle)BarStyle.StatusBarStyleProperty.DefaultValue)
			{
				SetStatusBarStyle(StatusBarStyle.Default);
			}
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == BarStyle.StatusBarColorProperty.PropertyName)
			{
				SetStatusBarColor(BarStyle.GetStatusBarColor(Element).ToUIColor());
			}
			else if (args.PropertyName == BarStyle.StatusBarStyleProperty.PropertyName)
			{
				SetStatusBarStyle(BarStyle.GetStatusBarStyle(Element));
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
