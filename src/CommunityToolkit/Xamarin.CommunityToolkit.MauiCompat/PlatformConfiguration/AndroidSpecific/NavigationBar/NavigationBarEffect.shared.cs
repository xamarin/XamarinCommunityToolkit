using System.Linq;
using Xamarin.CommunityToolkit.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Internals;
using FormsElement = Microsoft.Maui.Controls.Page;
using XFP = Microsoft.Maui.Controls.PlatformConfiguration;

namespace Xamarin.CommunityToolkit.PlatformConfiguration.AndroidSpecific
{
	[Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
	public static class NavigationBarEffect
	{
		static NavigationBarEffect()
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new PlatformNavigationBarEffect();
#endif
			#endregion
		}

		public static readonly BindableProperty ColorProperty = BindableProperty.CreateAttached(
			"Color", typeof(Color), typeof(NavigationBarEffect), new Microsoft.Maui.Graphics.Color(), propertyChanged: TryGenerateEffect);

		public static readonly BindableProperty StyleProperty = BindableProperty.CreateAttached(
			"Style", typeof(NavigationBarStyle), typeof(NavigationBarEffect), NavigationBarStyle.Default, propertyChanged: TryGenerateEffect);

		public static Color GetColor(BindableObject bindable) =>
			(Color)bindable.GetValue(ColorProperty);

		public static void SetColor(BindableObject bindable, Color value) =>
			bindable.SetValue(ColorProperty, value);

		public static NavigationBarStyle GetStyle(BindableObject bindable) =>
			(NavigationBarStyle)bindable.GetValue(StyleProperty);

		public static void SetStyle(BindableObject bindable, NavigationBarStyle value) =>
			bindable.SetValue(StyleProperty, value);

		public static IPlatformElementConfiguration<XFP.Android, FormsElement> SetNavigationBarColor(this IPlatformElementConfiguration<XFP.Android, FormsElement> config, Color color)
		{
			SetColor(config.Element, color);
			return config;
		}

		public static Color GetNavigationBarColor(this IPlatformElementConfiguration<XFP.Android, FormsElement> config) =>
			GetColor(config.Element);

		public static IPlatformElementConfiguration<XFP.Android, FormsElement> SetNavigationBarStyle(this IPlatformElementConfiguration<XFP.Android, FormsElement> config, NavigationBarStyle style)
		{
			SetStyle(config.Element, style);
			return config;
		}

		public static NavigationBarStyle GetNavigationBarStyle(this IPlatformElementConfiguration<XFP.Android, FormsElement> config) =>
			GetStyle(config.Element);

		static void TryGenerateEffect(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not FormsElement page)
				return;

			DetachEffect(page);

			AttachEffect(page);
		}

		static void AttachEffect(FormsElement element)
		{
			IElementController controller = element;
			if (controller is null || controller.EffectIsAttached(EffectIds.NavigationBar))
				return;

			element.Effects.Add(Effect.Resolve(EffectIds.NavigationBar));
		}

		static void DetachEffect(FormsElement element)
		{
			IElementController controller = element;
			if (controller is null || !controller.EffectIsAttached(EffectIds.NavigationBar))
				return;

			var toRemove = element.Effects.FirstOrDefault(e => e.ResolveId == Effect.Resolve(EffectIds.NavigationBar).ResolveId);
			if (toRemove != null)
				element.Effects.Remove(toRemove);
		}
	}
}