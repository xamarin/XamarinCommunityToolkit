using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Views;
using AView = Android.Views.View;
using Android.Content;
using Android.OS;
using System.ComponentModel;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Android.Effects;
using Android.Widget;

[assembly: ExportEffect(typeof(PlatformShadowEffect), nameof(ShadowEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class PlatformShadowEffect : PlatformEffect
	{
		const float defaultRadius = 10f;

		const float defaultOpacity = 1f;

		AView View => Control ?? Container;

		protected override void OnAttached()
		{
			if (View == null)
				return;

			Update();
		}

		protected override void OnDetached()
		{
			if (View == null)
				return;

			View.SetElevation(0);
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
			if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
				return;

			var color = ShadowEffect.GetColor(Element);
			if (color.IsDefault)
			{
				View.SetElevation(0);
				return;
			}

			var radius = (float)ShadowEffect.GetRadius(Element);
			if (radius < 0)
				radius = defaultRadius;

			var opacity = ShadowEffect.GetOpacity(Element);
			if (opacity < 0)
				opacity = defaultOpacity;

			var androidColor = color.MultiplyAlpha(opacity).ToAndroid();

			if (View is TextView textView)
			{
				var offsetX = (float)ShadowEffect.GetOffsetX(Element);
				var offsetY = (float)ShadowEffect.GetOffsetY(Element);
				textView.SetShadowLayer(radius, offsetX, offsetY, androidColor);
				return;
			}

			View.SetElevation(View.Context.ToPixels(radius));

			if (Build.VERSION.SdkInt < BuildVersionCodes.P)
				return;

#pragma warning disable XA0001 // Find issues with Android API usage
			View.SetOutlineAmbientShadowColor(androidColor);
			View.SetOutlineSpotShadowColor(androidColor);
#pragma warning restore XA0001 // Find issues with Android API usage
		}
	}
}