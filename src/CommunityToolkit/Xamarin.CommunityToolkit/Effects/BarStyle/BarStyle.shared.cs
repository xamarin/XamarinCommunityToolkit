using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class BarStyle : RoutingEffect
	{
		public static readonly BindableProperty StatusBarColorProperty = BindableProperty.CreateAttached(
			nameof(StatusBarColor), typeof(Color), typeof(BarStyle), Color.Default, propertyChanged: TryGenerateEffect);

		public static readonly BindableProperty StatusBarStyleProperty = BindableProperty.CreateAttached(
			nameof(StatusBarStyle), typeof(StatusBarStyle), typeof(BarStyle), StatusBarStyle.Default, propertyChanged: TryGenerateEffect);

		public BarStyle()
			: base(EffectIds.BarStyle)
		{
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.PlatformBarStyle();
#elif __IOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.PlatformBarStyle();
#elif UWP
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.UWP.Effects.PlatformBarStyle();
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

			var oldEffect = page.Effects.FirstOrDefault(e => e is BarStyle);
			if (oldEffect != null)
				page.Effects.Remove(oldEffect);

			page.Effects.Add(new BarStyle());
		}

		public Color StatusBarColor => GetStatusBarColor(Element);

		public StatusBarStyle StatusBarStyle => GetStatusBarStyle(Element);
	}
}