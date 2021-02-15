using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class WindowEffect : RoutingEffect
	{
		public static readonly BindableProperty StatusBarColorProperty = BindableProperty.CreateAttached(
			nameof(StatusBarColor), typeof(Color), typeof(WindowEffect), Color.Default, propertyChanged: TryGenerateEffect);

		public static readonly BindableProperty StatusBarStyleProperty = BindableProperty.CreateAttached(
			nameof(StatusBarStyle), typeof(StatusBarStyle), typeof(WindowEffect), StatusBarStyle.Default, propertyChanged: TryGenerateEffect);

		public WindowEffect()
			: base(EffectIds.WindowEffect)
		{
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Android.Effects.PlatformWindowEffect();
#elif __IOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new iOS.Effects.PlatformWindowEffect();
#elif UWP
			if (System.DateTime.Now.Ticks < 0)
				_ = new UWP.Effects.PlatformWindowEffect();
#endif
		}

		public static Color GetStatusBarColor(BindableObject bindable) =>
			(Color)bindable.GetValue(StatusBarColorProperty);

		public static StatusBarStyle GetStatusBarStyle(BindableObject bindable) =>
			(StatusBarStyle)bindable.GetValue(StatusBarStyleProperty);

		static void TryGenerateEffect(BindableObject bindable, object oldValue, object newValue)
		{
			if (!(bindable is Page page))
				return;

			var oldEffect = page.Effects.FirstOrDefault(e => e is WindowEffect);
			if (oldEffect != null)
				page.Effects.Remove(oldEffect);

			page.Effects.Add(new WindowEffect());
		}

		public Color StatusBarColor => GetStatusBarColor(Element);

		public StatusBarStyle StatusBarStyle => GetStatusBarStyle(Element);
	}
}