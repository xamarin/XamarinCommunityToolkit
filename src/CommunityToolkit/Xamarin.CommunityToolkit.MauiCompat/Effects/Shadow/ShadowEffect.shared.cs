using System.Linq;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Effects
{
	public class ShadowEffect : RoutingEffect
	{
		internal const string ColorPropertyName = "Color";

		internal const string OpacityPropertyName = "Opacity";

		internal const string RadiusPropertyName = "Radius";

		internal const string OffsetXPropertyName = "OffsetX";

		internal const string OffsetYPropertyName = "OffsetY";

		public static readonly BindableProperty ColorProperty = BindableProperty.CreateAttached(
			ColorPropertyName,
			typeof(Color),
			typeof(ShadowEffect),
			new Microsoft.Maui.Graphics.Color(),
			propertyChanged: TryGenerateEffect);

		public static readonly BindableProperty OpacityProperty = BindableProperty.CreateAttached(
			OpacityPropertyName,
			typeof(double),
			typeof(ShadowEffect),
			-1.0,
			propertyChanged: TryGenerateEffect);

		public static readonly BindableProperty RadiusProperty = BindableProperty.CreateAttached(
			RadiusPropertyName,
			typeof(double),
			typeof(ShadowEffect),
			-1.0,
			propertyChanged: TryGenerateEffect);

		public static readonly BindableProperty OffsetXProperty = BindableProperty.CreateAttached(
			OffsetXPropertyName,
			typeof(double),
			typeof(ShadowEffect),
			.0,
			propertyChanged: TryGenerateEffect);

		public static readonly BindableProperty OffsetYProperty = BindableProperty.CreateAttached(
			OffsetYPropertyName,
			typeof(double),
			typeof(ShadowEffect),
			.0,
			propertyChanged: TryGenerateEffect);

		public ShadowEffect()
			: base(EffectIds.ShadowEffect)
		{
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.PlatformShadowEffect();
#elif __IOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.PlatformShadowEffect();
#elif __MACOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.macOS.Effects.PlatformShadowEffect();
#elif UWP
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.UWP.Effects.PlatformShadowEffect();
#endif
		}

		public static Color GetColor(BindableObject bindable)
			=> (Color)bindable.GetValue(ColorProperty);

		public static void SetColor(BindableObject bindable, Color value)
			=> bindable.SetValue(ColorProperty, value);

		public static double GetOpacity(BindableObject bindable)
			=> (double)bindable.GetValue(OpacityProperty);

		public static void SetOpacity(BindableObject bindable, double value)
			=> bindable.SetValue(OpacityProperty, value);

		public static double GetRadius(BindableObject bindable)
			=> (double)bindable.GetValue(RadiusProperty);

		public static void SetRadius(BindableObject bindable, double value)
			=> bindable.SetValue(RadiusProperty, value);

		public static double GetOffsetX(BindableObject bindable)
			=> (double)bindable.GetValue(OffsetXProperty);

		public static void SetOffsetX(BindableObject bindable, double value)
			=> bindable.SetValue(OffsetXProperty, value);

		public static double GetOffsetY(BindableObject bindable)
			=> (double)bindable.GetValue(OffsetYProperty);

		public static void SetOffsetY(BindableObject bindable, double value)
			=> bindable.SetValue(OffsetYProperty, value);

		static void TryGenerateEffect(BindableObject bindable, object oldValue, object newValue)
		{
			if (!(bindable is VisualElement view))
				return;

			var shadowEffects = view.Effects.OfType<ShadowEffect>();

			if (GetColor(view) == new Microsoft.Maui.Graphics.Color())
			{
				foreach (var effect in shadowEffects.ToArray())
					view.Effects.Remove(effect);

				return;
			}

			if (!shadowEffects.Any())
				view.Effects.Add(new ShadowEffect());
		}
	}
}