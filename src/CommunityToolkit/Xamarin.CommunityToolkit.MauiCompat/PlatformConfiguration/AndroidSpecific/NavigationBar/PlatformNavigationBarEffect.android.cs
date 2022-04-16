using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System.ComponentModel;
using Android.App;
using Android.Views;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.PlatformConfiguration.AndroidSpecific;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;

[assembly: ExportEffect(typeof(PlatformNavigationBarEffect), nameof(NavigationBarEffect))]

namespace Xamarin.CommunityToolkit.PlatformConfiguration.AndroidSpecific
{
	using Xamarin.CommunityToolkit.MauiCompat; public class PlatformNavigationBarEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
	{
		protected override void OnAttached()
		{
			SetColor(NavigationBarEffect.GetColor(Element));
			SetStyle(NavigationBarEffect.GetStyle(Element));
		}

		protected override void OnDetached()
		{
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == NavigationBarEffect.ColorProperty.PropertyName)
			{
				SetColor(NavigationBarEffect.GetColor(Element));
			}
			else if (args.PropertyName == NavigationBarEffect.StyleProperty.PropertyName)
			{
				SetStyle(NavigationBarEffect.GetStyle(Element));
			}
		}

		void SetColor(Color color)
		{
			if (!Effects.BarStyle.IsSupported())
				return;

			Effects.BarStyle.GetCurrentWindow(Activity).SetNavigationBarColor(color.ToAndroid());
		}

		void SetStyle(NavigationBarStyle style)
		{
			if (!Effects.BarStyle.IsSupported())
				return;

			switch (style)
			{
				case NavigationBarStyle.DarkContent:
					Effects.BarStyle.AddBarAppearanceFlag(Activity, (StatusBarVisibility)SystemUiFlags.LightNavigationBar);
					break;
				default:
					Effects.BarStyle.RemoveBarAppearanceFlag(Activity, (StatusBarVisibility)SystemUiFlags.LightNavigationBar);
					break;
			}
		}

		Activity Activity
		{
			get
			{
				if (Control != null)
					return (Activity)Control.Context!;
				else
					return (Activity)Container.Context!;
			}
		}
	}
}