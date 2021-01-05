using System;
using System.ComponentModel;
using CoreGraphics;
using UIKit;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(PlatformShadowEffect), nameof(ShadowEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class PlatformShadowEffect : PlatformEffect
	{
		const float defaultRadius = 10f;

		const float defaultOpacity = .5f;

		UIView View => Control ?? Container;

		protected override void OnAttached()
		{
			if (View == null)
				return;

			UpdateColor();
			UpdateOpacity();
			UpdateRadius();
			UpdateOffset();
		}

		protected override void OnDetached()
		{
			if (View == null)
				return;

			View.Layer.ShadowOpacity = 0;
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (View == null)
				return;

			switch (args.PropertyName)
			{
				case nameof(ShadowEffect.ColorPropertyName):
					UpdateColor();
					break;
				case nameof(ShadowEffect.OpacityPropertyName):
					UpdateOpacity();
					break;
				case nameof(ShadowEffect.RadiusPropertyName):
					UpdateRadius();
					break;
				case nameof(ShadowEffect.OffsetXPropertyName):
				case nameof(ShadowEffect.OffsetYPropertyName):
					UpdateOffset();
					break;
			}
		}

		void UpdateColor()
			=> View.Layer.ShadowColor = ShadowEffect.GetColor(Element).ToCGColor();

		void UpdateOpacity()
		{
			var opacity = (float)ShadowEffect.GetOpacity(Element);
			View.Layer.ShadowOpacity = opacity < 0
				? defaultOpacity
				: opacity;
		}

		void UpdateRadius()
		{
			var radius = (nfloat)ShadowEffect.GetRadius(Element);
			View.Layer.ShadowRadius = radius < 0
				? defaultRadius
				: radius;
		}

		void UpdateOffset()
			=> View.Layer.ShadowOffset = new CGSize((double)ShadowEffect.GetOffsetX(Element), (double)ShadowEffect.GetOffsetY(Element));
	}
}
