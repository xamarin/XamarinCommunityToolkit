using System;using Microsoft.Extensions.Logging;
using Xamarin.CommunityToolkit.Helpers;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Effects
{
	/// <summary>
	/// An effect to subscribe to the View's lifecycle events.
	/// </summary>
	public class LifecycleEffect : RoutingEffect
	{
		readonly WeakEventManager eventManager = new WeakEventManager();

		/// <summary>
		/// Event that is triggered when the <see cref="View" /> is loaded and is ready for use.
		/// </summary>
		public event EventHandler Loaded
		{
			add => eventManager.AddEventHandler(value);
			remove => eventManager.RemoveEventHandler(value);
		}

		/// <summary>
		/// Event that is triggered when the <see cref="View" /> is unloaded and isn't ready for use.
		/// </summary>
		public event EventHandler Unloaded
		{
			add => eventManager.AddEventHandler(value);
			remove => eventManager.RemoveEventHandler(value);
		}

		/// <summary>
		/// Constructor for the <see cref="LifecycleEffect" />
		/// </summary>
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

		internal void RaiseLoadedEvent(Element element) => eventManager.RaiseEvent(element, EventArgs.Empty, nameof(Loaded));

		internal void RaiseUnloadedEvent(Element element) => eventManager.RaiseEvent(element, EventArgs.Empty, nameof(Unloaded));
	}
}