using System;
using System.Linq;
using Foundation;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(LifeCycleEffectRouter), nameof(LifeCycleEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class LifeCycleEffectRouter : PlatformEffect
	{
		const NSKeyValueObservingOptions observingOptions = NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.OldNew | NSKeyValueObservingOptions.Prior; 
		LifeCycleEffect lifeCycleEffect;
		IDisposable isLoadedObserverDisposable;

		protected override void OnAttached()
		{
			lifeCycleEffect = Element.Effects.OfType<LifeCycleEffect>().FirstOrDefault();
			_ = lifeCycleEffect ?? throw new ArgumentNullException($"The effect {nameof(LifeCycleEffect)} can't be null.");

			var nativeView = Control ?? Container;
			var key = nativeView.Superview == null ? "subviews" : "superview";
			isLoadedObserverDisposable = nativeView.AddObserver(key, observingOptions, OnViewLoadedObserver);
		}

		void OnViewLoadedObserver(NSObservedChange nSObservedChange)
		{
			if (!nSObservedChange?.NewValue?.Equals(NSNull.Null) ?? false)
				lifeCycleEffect.RaiseLoadedEvent(Element);
			else if (!nSObservedChange?.OldValue?.Equals(NSNull.Null) ?? false)
				lifeCycleEffect.RaiseUnloadedEvent(Element);
		}

		protected override void OnDetached()
		{
			lifeCycleEffect.RaiseUnloadedEvent(Element);
			isLoadedObserverDisposable.Dispose();
			isLoadedObserverDisposable = null;
			lifeCycleEffect = null;
		}
	}
}
