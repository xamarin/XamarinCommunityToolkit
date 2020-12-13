using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class LifecycleEffect : RoutingEffect
	{
		public event EventHandler Loaded;

		public event EventHandler Unloaded;

		public LifecycleEffect()
			: base(EffectIds.LifeCycleEffect)
		{
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.Effects.LifeCycleEffectRouter();
#elif __IOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.Effects.LifeCycleEffectRouter();
#elif UWP
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.UWP.Effects.LifeCycleEffectRouter();
#endif
		}

		internal void RaiseLoadedEvent(Element element) => Loaded?.Invoke(element, EventArgs.Empty);

		internal void RaiseUnloadedEvent(Element element) => Unloaded?.Invoke(element, EventArgs.Empty);
	}
}
