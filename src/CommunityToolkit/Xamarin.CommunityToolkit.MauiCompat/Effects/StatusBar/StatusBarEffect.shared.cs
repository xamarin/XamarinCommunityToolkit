using System.Linq;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Effects
{
	public class StatusBarEffect : RoutingEffect
	{
		public static readonly BindableProperty ColorProperty = BindableProperty.CreateAttached(
			"Color", typeof(Color), typeof(StatusBarEffect), new Microsoft.Maui.Graphics.Color(), propertyChanged: TryGenerateEffect);

		public static readonly BindableProperty StyleProperty = BindableProperty.CreateAttached(
			"Style", typeof(StatusBarStyle), typeof(StatusBarEffect), StatusBarStyle.Default, propertyChanged: TryGenerateEffect);

		public StatusBarEffect()
			: base(EffectIds.StatusBar)
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Android.Effects.PlatformStatusBarEffect();
#elif __IOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new iOS.Effects.PlatformStatusBarEffect();
#endif
			#endregion
		}

		public static Color GetColor(BindableObject bindable) =>
			(Color)bindable.GetValue(ColorProperty);

		public static StatusBarStyle GetStyle(BindableObject bindable) =>
			(StatusBarStyle)bindable.GetValue(StyleProperty);

		static void TryGenerateEffect(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not Page page)
				return;

			var oldEffect = page.Effects.FirstOrDefault(e => e is StatusBarEffect);
			if (oldEffect != null)
				page.Effects.Remove(oldEffect);

			page.Effects.Add(new StatusBarEffect());
		}
	}
}