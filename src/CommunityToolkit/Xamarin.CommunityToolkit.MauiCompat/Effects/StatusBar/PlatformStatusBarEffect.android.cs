using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System.ComponentModel;
using Android.Views;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;

[assembly: ExportEffect(typeof(PlatformStatusBarEffect), nameof(StatusBarEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	using Xamarin.CommunityToolkit.MauiCompat; public class PlatformStatusBarEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
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

		void SetColor(Color color)
		{
			if (!BarStyle.IsSupported())
				return;

			Activity.Window.SetStatusBarColor(color.ToAndroid());
		}

		void SetStyle(StatusBarStyle style)
		{
			if (!BarStyle.IsSupported())
				return;

			switch (style)
			{
				case StatusBarStyle.DarkContent:
					BarStyle.AddBarAppearanceFlag(Activity, (StatusBarVisibility)SystemUiFlags.LightStatusBar);
					break;
				default:
					BarStyle.RemoveBarAppearanceFlag(Activity, (StatusBarVisibility)SystemUiFlags.LightStatusBar);
					break;
			}
		}

		MauiAppCompatActivity Activity
		{
			get
			{
				if (Control != null)
					return (MauiAppCompatActivity)Control.Context!;
				else
					return (MauiAppCompatActivity)Container.Context!;
			}
		}
	}
}