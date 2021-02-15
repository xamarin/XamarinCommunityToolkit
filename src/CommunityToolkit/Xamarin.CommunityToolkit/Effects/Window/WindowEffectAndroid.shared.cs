using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class WindowEffectAndroid : RoutingEffect
	{
		public static readonly BindableProperty NavigationBarColorProperty = BindableProperty.CreateAttached(
			nameof(NavigationBarColor), typeof(Color), typeof(WindowEffectAndroid), Color.Default, propertyChanged: TryGenerateEffect);

		public static readonly BindableProperty NavigationBarStyleProperty = BindableProperty.CreateAttached(
			nameof(NavigationBarStyle), typeof(NavigationBarStyle), typeof(WindowEffectAndroid), NavigationBarStyle.Default, propertyChanged: TryGenerateEffect);

		public WindowEffectAndroid()
			: base(EffectIds.WindowEffectAndroid)
		{
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Android.Effects.PlatformWindowEffectAndroid();
#endif
		}

		public static Color GetNavigationBarColor(BindableObject bindable) =>
			(Color)bindable.GetValue(NavigationBarColorProperty);

		public static NavigationBarStyle GetNavigationBarStyle(BindableObject bindable) =>
			(NavigationBarStyle)bindable.GetValue(NavigationBarStyleProperty);

		static void TryGenerateEffect(BindableObject bindable, object oldValue, object newValue)
		{
			if (!(bindable is Page page))
				return;

			var oldEffect = page.Effects.FirstOrDefault(e => e is WindowEffectAndroid);
			if (oldEffect != null)
				page.Effects.Remove(oldEffect);

			page.Effects.Add(new WindowEffectAndroid());
		}

		public Color NavigationBarColor => GetNavigationBarColor(Element);

		public NavigationBarStyle NavigationBarStyle => GetNavigationBarStyle(Element);
	}
}