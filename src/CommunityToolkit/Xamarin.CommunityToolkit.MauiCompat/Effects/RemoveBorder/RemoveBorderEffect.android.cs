using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Xamarin.CommunityToolkit.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;
using Effects = Xamarin.CommunityToolkit.Android.Effects;

[assembly: ExportEffect(typeof(Effects.RemoveBorderEffect), nameof(RemoveBorderEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class RemoveBorderEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
	{
		Drawable? originalBackground;

		protected override void OnAttached()
		{
			originalBackground = Control.Background;

			var shape = new global::Android.Graphics.Drawables.ShapeDrawable(new RectShape());
			if (shape.Paint != null)
			{
				shape.Paint.Color = global::Android.Graphics.Color.Transparent;
				shape.Paint.StrokeWidth = 0;
				shape.Paint.SetStyle(Paint.Style.Stroke);
			}

			Control.Background = shape;
		}

		protected override void OnDetached() => Control.Background = originalBackground;
	}
}