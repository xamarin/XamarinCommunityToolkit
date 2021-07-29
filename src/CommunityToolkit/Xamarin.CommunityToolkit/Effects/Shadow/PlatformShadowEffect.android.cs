using System.ComponentModel;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AButton = Android.Widget.Button;
using ATextView = Android.Widget.TextView;
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
			if (View == null || Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
				return;

			var radius = (float)ShadowEffect.GetRadius(Element);
			if (radius < 0)
				radius = defaultRadius;

			var opacity = ShadowEffect.GetOpacity(Element);
			if (opacity < 0)
				opacity = defaultOpacity;

			var color = ShadowEffect.GetColor(Element);
			if (!color.IsDefault)
				color = color.MultiplyAlpha(opacity);

			var androidColor = color.ToAndroid();
			var offsetX = (float)ShadowEffect.GetOffsetX(Element);
			var offsetY = (float)ShadowEffect.GetOffsetY(Element);
			var cornerRadius = Element is IBorderElement borderElement ? borderElement.CornerRadius : 0;

			if (View is AButton button)
			{
				button.StateListAnimator = null;
			}
			else if (View is not AButton && View is ATextView textView)
			{
				textView.SetShadowLayer(radius, offsetX, offsetY, androidColor);
				return;
			}

			var pixelOffsetX = View.Context.ToPixels(offsetX);
			var pixelOffsetY = View.Context.ToPixels(offsetY);
			var pixelCornerRadius = View.Context.ToPixels(cornerRadius);
			View.OutlineProvider = new ShadowOutlineProvider(pixelOffsetX, pixelOffsetY, pixelCornerRadius);
			View.Elevation = View.Context.ToPixels(radius);
			if (View.Parent is ViewGroup group)
				group.SetClipToPadding(false);

#pragma warning disable
			if (Build.VERSION.SdkInt < BuildVersionCodes.P)
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