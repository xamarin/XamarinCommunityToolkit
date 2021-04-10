using System;
using System.Linq;
using Windows.UI.Xaml;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.UWP.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportEffect(typeof(LifeCycleEffectRouter), nameof(LifecycleEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	/// <summary>
	/// UWP implementation of the <see cref="LifecycleEffect" />
	/// </summary>
	public class LifeCycleEffectRouter : PlatformEffect
	{
		FrameworkElement? nativeView;
		LifecycleEffect? lifeCycleEffect;

		protected override void OnAttached()
		{
			lifeCycleEffect = Element.Effects.OfType<LifecycleEffect>().FirstOrDefault() ??
				throw new ArgumentNullException($"The effect {nameof(LifecycleEffect)} can't be null.");

			nativeView = Control ?? Container;

			nativeView.Loaded += OnNativeViewLoaded;
			nativeView.Unloaded += OnNativeViewUnloaded;
		}

		void OnNativeViewLoaded(object? sender, RoutedEventArgs e) => lifeCycleEffect?.RaiseLoadedEvent(Element);

		void OnNativeViewUnloaded(object? sender, RoutedEventArgs e)
		{
			if (lifeCycleEffect != null)
			{
				lifeCycleEffect.RaiseUnloadedEvent(Element);
			}

			if (nativeView != null)
			{
				nativeView.Unloaded -= OnNativeViewUnloaded;
				nativeView.Loaded -= OnNativeViewLoaded;
			}

			lifeCycleEffect = null;
			nativeView = null;
		}

		protected override void OnDetached()
		{
		}
	}
}