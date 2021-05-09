﻿using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class NavigationBarEffect : RoutingEffect
	{
		public static readonly BindableProperty ColorProperty = BindableProperty.CreateAttached(
			nameof(Color), typeof(Color), typeof(StatusBarEffect), Color.Default, propertyChanged: TryGenerateEffect);

		public static readonly BindableProperty StyleProperty = BindableProperty.CreateAttached(
			nameof(Style), typeof(NavigationBarStyle), typeof(StatusBarEffect), NavigationBarStyle.Default, propertyChanged: TryGenerateEffect);

		public NavigationBarEffect()
			: base(EffectIds.NavigationBar)
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Android.Effects.PlatformNavigationBarEffect();
#endif
			#endregion
		}

		public static Color GetColor(BindableObject bindable) =>
			(Color)bindable.GetValue(ColorProperty);

		public static NavigationBarStyle GetStyle(BindableObject bindable) =>
			(NavigationBarStyle)bindable.GetValue(StyleProperty);

		static void TryGenerateEffect(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is not Page page)
				return;

			var oldEffect = page.Effects.FirstOrDefault(e => e is StatusBarEffect);
			if (oldEffect != null)
				page.Effects.Remove(oldEffect);

			page.Effects.Add(new StatusBarEffect());
		}

		public Color Color => GetColor(Element);

		public NavigationBarStyle Style => GetStyle(Element);
	}
}