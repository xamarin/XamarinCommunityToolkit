using System.ComponentModel;
using Android.OS;
using Android.Views;
using Android.Widget;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;

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
				case nameof(ShadowEffect.ColorPropertyName):
				case nameof(ShadowEffect.OpacityPropertyName):
				case nameof(ShadowEffect.RadiusPropertyName):
				case nameof(ShadowEffect.OffsetXPropertyName):
				case nameof(ShadowEffect.OffsetYPropertyName):
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

			if (View is TextView textView)
			{
				var offsetX = (float)ShadowEffect.GetOffsetX(Element);
				var offsetY = (float)ShadowEffect.GetOffsetY(Element);
				textView.SetShadowLayer(radius, offsetX, offsetY, androidColor);
				return;
			}

			View.OutlineProvider = (Element as VisualElement)?.BackgroundColor.A > 0
				? ViewOutlineProvider.PaddedBounds
				: ViewOutlineProvider.Bounds;

			View.Elevation = View.Context.ToPixels(radius);

			if (Build.VERSION.SdkInt < BuildVersionCodes.P)
				return;

			View.SetOutlineAmbientShadowColor(androidColor);
			View.SetOutlineSpotShadowColor(androidColor);
		}
	}
}