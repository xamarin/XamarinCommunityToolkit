using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using View = Android.Views.View;
using Xamarin.Forms.Platform.Android;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Android.Effects;

[assembly: ExportEffect(typeof(LifeCycleEffectRouter), nameof(LifeCycleEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class LifeCycleEffectRouter : PlatformEffect
	{
		View nativeView;
		LifeCycleEffect lifeCycleEffect;

		protected override void OnAttached()
		{
			lifeCycleEffect = Element.Effects.OfType<LifeCycleEffect>().FirstOrDefault();

			_ = lifeCycleEffect ?? throw new ArgumentNullException($"The effect {nameof(LifeCycleEffect)} can't be null.");

			nativeView = Control ?? Container;

			nativeView.ViewAttachedToWindow += OnNativeViewViewAttachedToWindow;
			nativeView.ViewDetachedFromWindow += OnNativeViewViewDetachedFromWindow;
		}

		void OnNativeViewViewAttachedToWindow(object sender, View.ViewAttachedToWindowEventArgs e) => lifeCycleEffect.RaiseLoadedEvent(Element);

		void OnNativeViewViewDetachedFromWindow(object sender, View.ViewDetachedFromWindowEventArgs e) => lifeCycleEffect.RaiseUnloadedEvent(Element);

		protected override void OnDetached()
		{
			lifeCycleEffect.RaiseUnloadedEvent(Element);
			nativeView.ViewAttachedToWindow -= OnNativeViewViewAttachedToWindow;
			nativeView.ViewDetachedFromWindow -= OnNativeViewViewDetachedFromWindow;
			nativeView = null;
			lifeCycleEffect = null;
		}
	}
}
