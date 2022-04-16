using Button = Android.Widget.Button;using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System;using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Linq;
using Android.Graphics;
using Android.Widget;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Extensions;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;
using Effects = Xamarin.CommunityToolkit.Android.Effects;

[assembly: Microsoft.Maui.Controls.ExportEffect(typeof(Effects.IconTintColorEffectRouter), nameof(IconTintColorEffectRouter))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	using Xamarin.CommunityToolkit.MauiCompat; public class IconTintColorEffectRouter : Microsoft.Maui.Controls.Platform.PlatformEffect
	{
		protected override void OnAttached()
			=> ApplyTintColor();

		protected override void OnDetached()
			=> ClearTintColor();

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (!args.PropertyName?.Equals(IconTintColorEffect.TintColorProperty.PropertyName) is true &&
				!args.PropertyName?.Equals(Microsoft.Maui.Controls.Image.SourceProperty.PropertyName) is true &&
				!args.PropertyName?.Equals(Microsoft.Maui.Controls.ImageButton.SourceProperty.PropertyName) is true)
				return;

			ApplyTintColor();
		}

		void ApplyTintColor()
		{
			if (!Control.IsAlive() || Element == null)
			{
				return;
			}

			var color = IconTintColorEffect.GetTintColor(Element);

			switch (Control)
			{
				case ImageView image:
					SetImageViewTintColor(image, color);
					break;
				case Button button:
					SetButtonTintColor(button, color);
					break;
			}
		}

		void ClearTintColor()
		{
			// Because of a XF bug: https://github.com/xamarin/Xamarin.Forms/issues/13889
			if (!Control.IsAlive())
			{
				return;
			}

			switch (Control)
			{
				case ImageView image:
					image.ClearColorFilter();
					break;
				case Button button:
					foreach (var drawable in button.GetCompoundDrawables())
						drawable?.ClearColorFilter();
					break;
			}
		}

		void SetImageViewTintColor(ImageView image, Microsoft.Maui.Graphics.Color color)
		{
			if (color .Equals(new Microsoft.Maui.Graphics.Color()))
				image.ClearColorFilter();

			image.SetColorFilter(new PorterDuffColorFilter(color.ToAndroid(), PorterDuff.Mode.SrcIn ?? throw new NullReferenceException()));
		}

		void SetButtonTintColor(Button button, Microsoft.Maui.Graphics.Color color)
		{
			var drawables = button.GetCompoundDrawables().Where(d => d != null);

			if (color .Equals(new Microsoft.Maui.Graphics.Color()))
			{
				foreach (var img in drawables)
					img.ClearColorFilter();
			}

			foreach (var img in drawables)
				img.SetTint(color.ToAndroid());
		}
	}
}