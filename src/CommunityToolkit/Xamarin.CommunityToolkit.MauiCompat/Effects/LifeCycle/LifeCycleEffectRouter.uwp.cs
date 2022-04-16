using System;using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.UI.Xaml;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.UWP.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;

[assembly: ExportEffect(typeof(LifeCycleEffectRouter), nameof(LifecycleEffect))]

namespace Xamarin.CommunityToolkit.UWP.Effects
{
	/// <summary>
	/// UWP implementation of the <see cref="LifecycleEffect" />
	/// </summary>
	public class LifeCycleEffectRouter : Microsoft.Maui.Controls.Platform.PlatformEffect
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