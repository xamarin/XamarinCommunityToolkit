using System;
using System.Linq;
using Foundation;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.iOS.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(LifeCycleEffectRouter), nameof(LifecycleEffect))]

namespace Xamarin.CommunityToolkit.iOS.Effects
{
	public class LifeCycleEffectRouter : PlatformEffect
	{
		const NSKeyValueObservingOptions observingOptions = NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.OldNew | NSKeyValueObservingOptions.Prior;
		LifecycleEffect lifeCycleEffect;
		IDisposable isLoadedObserverDisposable;

		protected override void OnAttached()
		{
			lifeCycleEffect = Element.Effects.OfType<LifecycleEffect>().FirstOrDefault();
			_ = lifeCycleEffect ?? throw new ArgumentNullException($"The effect {nameof(LifecycleEffect)} can't be null.");

			var nativeView = Control ?? Container;
			var key = nativeView.Superview == null ? "subviews" : "superview";
			isLoadedObserverDisposable = nativeView.AddObserver(key, observingOptions, OnViewLoadedObserver);
		}

		void OnViewLoadedObserver(NSObservedChange nSObservedChange)
		{
			if (!nSObservedChange?.NewValue?.Equals(NSNull.Null) ?? false)
				lifeCycleEffect.RaiseLoadedEvent(Element);
			else if (!nSObservedChange?.OldValue?.Equals(NSNull.Null) ?? false)
			{
				lifeCycleEffect.RaiseUnloadedEvent(Element);
				isLoadedObserverDisposable.Dispose();
				isLoadedObserverDisposable = null;
				lifeCycleEffect = null;
			}
		}

		protected override void OnDetached()
		{
		}
	}
}
