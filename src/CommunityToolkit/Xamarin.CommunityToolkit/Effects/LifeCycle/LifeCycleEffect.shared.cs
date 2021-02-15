using System;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class LifecycleEffect : RoutingEffect
	{
		readonly WeakEventManager eventManager = new WeakEventManager();

		public event EventHandler Loaded
		{
			add => eventManager.AddEventHandler(value);
			remove => eventManager.RemoveEventHandler(value);
		}

		public event EventHandler Unloaded
		{
			add => eventManager.AddEventHandler(value);
			remove => eventManager.RemoveEventHandler(value);
		}

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

		internal void RaiseLoadedEvent(Element element) => eventManager.RaiseEvent(element, EventArgs.Empty, "Loaded");

		internal void RaiseUnloadedEvent(Element element) => eventManager.RaiseEvent(element, EventArgs.Empty, "Unloaded");
	}
}
