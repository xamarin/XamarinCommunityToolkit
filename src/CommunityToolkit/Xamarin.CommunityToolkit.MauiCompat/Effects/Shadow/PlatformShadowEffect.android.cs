using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System.ComponentModel;
using Android.Graphics;
using Android.OS;
using Android.Views;
using System;using Microsoft.Extensions.Logging;using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Helpers;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;
using AButton = Android.Widget.Button;
using AEditText = Android.Widget.EditText;
using ATextView = Android.Widget.TextView;
using AView = Android.Views.View;

[assembly: ExportEffect(typeof(PlatformShadowEffect), nameof(ShadowEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	using Xamarin.CommunityToolkit.MauiCompat; public class PlatformShadowEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
	{
		const float defaultRadius = 10f;

		const float defaultOpacity = 1f;

		AView View => Control ?? Container;

		protected override void OnAttached()
			=> Update();

		protected override void OnDetached()
		{
			if (View == null)
				return;

			View.Elevation = 0;
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (View == null)
				return;

			switch (args.PropertyName)
			{
				case ShadowEffect.ColorPropertyName:
				case ShadowEffect.OpacityPropertyName:
				case ShadowEffect.RadiusPropertyName:
				case ShadowEffect.OffsetXPropertyName:
				case ShadowEffect.OffsetYPropertyName:
				case nameof(VisualElement.Width):
				case nameof(VisualElement.Height):
				case nameof(VisualElement.BackgroundColor):
				case nameof(IBorderElement.CornerRadius):
					View.Invalidate();
					Update();
					break;
			}
		}

		void Update()
		{
			if (View == null || XCT.SdkInt < (int)BuildVersionCodes.Lollipop)
				return;

			var radius = (float)ShadowEffect.GetRadius(Element);
			if (radius < 0)
				radius = defaultRadius;

			var opacity = ShadowEffect.GetOpacity(Element);
			if (opacity < 0)
				opacity = defaultOpacity;

			var color = ShadowEffect.GetColor(Element);
			if (!color.IsDefault())
				color = color.MultiplyAlpha((float)opacity);

			var androidColor = color.ToAndroid();
			var offsetX = (float)ShadowEffect.GetOffsetX(Element);
			var offsetY = (float)ShadowEffect.GetOffsetY(Element);
			var cornerRadius = Element is IBorderElement borderElement ? borderElement.CornerRadius : 0;

			if (View is AButton button)
			{
				button.StateListAnimator = null;
			}
			else if (View is not AEditText && View is ATextView textView)
			{
				textView.SetShadowLayer(radius, offsetX, offsetY, androidColor);
				return;
			}

			var pixelOffsetX = Microsoft.Maui.Platform.ContextExtensions.ToPixels(View.Context ?? throw new NullReferenceException(), offsetX);
			var pixelOffsetY = Microsoft.Maui.Platform.ContextExtensions.ToPixels(View.Context ?? throw new NullReferenceException(), offsetY);
			var pixelCornerRadius = Microsoft.Maui.Platform.ContextExtensions.ToPixels(View.Context ?? throw new NullReferenceException(), cornerRadius);
			View.OutlineProvider = new ShadowOutlineProvider(pixelOffsetX, pixelOffsetY, pixelCornerRadius);
			View.Elevation = Microsoft.Maui.Platform.ContextExtensions.ToPixels(View.Context ?? throw new NullReferenceException(), radius);
			if (View.Parent is ViewGroup group)
				group.SetClipToPadding(false);

#pragma warning disable
			if (XCT.SdkInt < (int)BuildVersionCodes.P)
				return;

			View.SetOutlineAmbientShadowColor(androidColor);
			View.SetOutlineSpotShadowColor(androidColor);
#pragma warning restore
		}

		class ShadowOutlineProvider : ViewOutlineProvider
		{
			readonly float offsetX;
			readonly float offsetY;
			readonly float cornerRadius;

			public ShadowOutlineProvider(float offsetX, float offsetY, float cornerRadius)
			{
				this.offsetX = offsetX;
				this.offsetY = offsetY;
				this.cornerRadius = cornerRadius;
			}

			public override void GetOutline(AView? view, Outline? outline)
				=> outline?.SetRoundRect((int)offsetX, (int)offsetY, view?.Width ?? 0, view?.Height ?? 0, cornerRadius);
		}
	}
}