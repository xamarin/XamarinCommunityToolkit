using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class StatusBarEffect : RoutingEffect
	{
		public static readonly BindableProperty StatusBarColorProperty = BindableProperty.CreateAttached(
			nameof(StatusBarColor), typeof(Color), typeof(StatusBarEffect), Color.Default, propertyChanged: TryGenerateEffect);

		public static readonly BindableProperty StatusBarStyleProperty = BindableProperty.CreateAttached(
			nameof(StatusBarStyle), typeof(StatusBarStyle), typeof(StatusBarEffect), StatusBarStyle.Default, propertyChanged: TryGenerateEffect);

		public StatusBarEffect()
			: base(EffectIds.StatusBarEffect)
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Android.Effects.PlatformStatusBarEffect();
#elif __IOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new iOS.Effects.PlatformStatusBarEffect();
#elif UWP
			if (System.DateTime.Now.Ticks < 0)
				_ = new UWP.Effects.PlatformStatusBarEffect();
#endif
			#endregion
		}

		public static Color GetStatusBarColor(BindableObject bindable) =>
			(Color)bindable.GetValue(StatusBarColorProperty);

		public static StatusBarStyle GetStatusBarStyle(BindableObject bindable) =>
			(StatusBarStyle)bindable.GetValue(StatusBarStyleProperty);

		static void TryGenerateEffect(BindableObject bindable, object oldValue, object newValue)
		{
			if (!(bindable is Page page))
				return;

			var oldEffect = page.Effects.FirstOrDefault(e => e is StatusBarEffect);
			if (oldEffect != null)
				page.Effects.Remove(oldEffect);

			page.Effects.Add(new StatusBarEffect());
		}

		public Color StatusBarColor => GetStatusBarColor(Element);

		public StatusBarStyle StatusBarStyle => GetStatusBarStyle(Element);
	}
}