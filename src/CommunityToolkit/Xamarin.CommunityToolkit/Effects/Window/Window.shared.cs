using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class Window : RoutingEffect
	{
		public static readonly BindableProperty StatusBarColorProperty = BindableProperty.CreateAttached(
			nameof(StatusBarColor), typeof(Color), typeof(Window), Color.Default, propertyChanged: TryGenerateEffect);

		public static readonly BindableProperty StatusBarStyleProperty = BindableProperty.CreateAttached(
			nameof(StatusBarStyle), typeof(StatusBarStyle), typeof(Window), StatusBarStyle.Default, propertyChanged: TryGenerateEffect);

		public Window()
			: base(EffectIds.Window)
		{
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.PlatformWindow();
#elif __IOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.PlatformWindow();
#elif UWP
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.UWP.Effects.PlatformWindow();
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

			var oldEffect = page.Effects.FirstOrDefault(e => e is Window);
			if (oldEffect != null)
				page.Effects.Remove(oldEffect);

			page.Effects.Add(new Window());
		}

		public Color StatusBarColor => GetStatusBarColor(Element);

		public StatusBarStyle StatusBarStyle => GetStatusBarStyle(Element);
	}
}