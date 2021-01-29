using Windows.UI.Xaml.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.UWP.Effects;
using Xamarin.CommunityToolkit.Effects;
using System.ComponentModel;
using Color = Windows.UI.Color;

[assembly: ExportEffect(typeof(PlatformBarStyle), nameof(BarStyle))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class PlatformBarStyle : PlatformEffect
	{
		protected override void OnAttached()
		{
			SetStatusBarColor(BarStyle.GetStatusBarColor(Element).ToWindowsColor());
			SetStatusBarStyle(BarStyle.GetStatusBarStyle(Element));
		}

		protected override void OnDetached()
		{
			if (BarStyle.GetStatusBarColor(Element) != (Forms.Color)BarStyle.StatusBarColorProperty.DefaultValue)
			{
				SetStatusBarColor(Color.FromArgb(0, 0, 0, 0));
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
				SetStatusBarColor(BarStyle.GetStatusBarColor(Element).ToWindowsColor());
			}
			else if (args.PropertyName == BarStyle.StatusBarStyleProperty.PropertyName)
			{
				SetStatusBarStyle(BarStyle.GetStatusBarStyle(Element));
			}
		}

		public void SetStatusBarColor(Color color)
		{
		}

		public void SetStatusBarStyle(StatusBarStyle style)
		{
		}
	}
}
