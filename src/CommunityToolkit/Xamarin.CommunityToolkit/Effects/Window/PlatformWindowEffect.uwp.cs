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

[assembly: ExportEffect(typeof(PlatformWindowEffect), nameof(WindowEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	public class PlatformWindowEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			SetStatusBarColor(WindowEffect.GetStatusBarColor(Element).ToWindowsColor());
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
				SetStatusBarColor(WindowEffect.GetStatusBarColor(Element).ToWindowsColor());
			}
			else if (args.PropertyName == WindowEffect.StatusBarStyleProperty.PropertyName)
			{
				SetStatusBarStyle(WindowEffect.GetStatusBarStyle(Element));
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
