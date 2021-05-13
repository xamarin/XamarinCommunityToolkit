using System.ComponentModel;
using Android.OS;
using Android.Views;
using Android.Widget;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;
using AButton = Android.Widget.Button;

[assembly: ExportEffect(typeof(PlatformShadowEffect), nameof(ShadowEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformShadowEffect : PlatformEffect
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
					View.Invalidate();
					Update();
					break;
			}
		}

		void Update()
		{
			if (View == null || Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
				return;

			var radius = (float)ShadowEffect.GetRadius(Element);
			if (radius < 0)
				radius = defaultRadius;

			var opacity = ShadowEffect.GetOpacity(Element);
			if (opacity < 0)
				opacity = defaultOpacity;

			var androidColor = ShadowEffect.GetColor(Element).MultiplyAlpha(opacity).ToAndroid();
			if (View is AButton button)
			{
				button.StateListAnimator = null;
				button.OutlineProvider = ViewOutlineProvider.Bounds;
			}
			else if (View is not AButton && View is TextView textView)
			{
				var offsetX = (float)ShadowEffect.GetOffsetX(Element);
				var offsetY = (float)ShadowEffect.GetOffsetY(Element);
				textView.SetShadowLayer(radius, offsetX, offsetY, androidColor);
				return;
			}
			else
			{
				View.OutlineProvider = (Element as VisualElement)?.BackgroundColor.A > 0
					? ViewOutlineProvider.PaddedBounds
					: ViewOutlineProvider.Bounds;
			}

			View.Elevation = View.Context.ToPixels(radius);
			if (View.Parent is ViewGroup group)
				group.SetClipToPadding(false);

			if (Build.VERSION.SdkInt < BuildVersionCodes.P)
				return;

			View.SetOutlineAmbientShadowColor(androidColor);
			View.SetOutlineSpotShadowColor(androidColor);
		}
	}
}