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

[assembly: ExportEffect(typeof(PlatformWindow), nameof(Window))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class PlatformWindow : PlatformEffect
	{
		protected override void OnAttached()
		{
			SetStatusBarColor(Window.GetStatusBarColor(Element).ToWindowsColor());
			SetStatusBarStyle(Window.GetStatusBarStyle(Element));
		}

		protected override void OnDetached()
		{
			if (Window.GetStatusBarColor(Element) != (Forms.Color)Window.StatusBarColorProperty.DefaultValue)
			{
				SetStatusBarColor(Color.FromArgb(0, 0, 0, 0));
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
				SetStatusBarColor(Window.GetStatusBarColor(Element).ToWindowsColor());
			}
			else if (args.PropertyName == Window.StatusBarStyleProperty.PropertyName)
			{
				SetStatusBarStyle(Window.GetStatusBarStyle(Element));
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
