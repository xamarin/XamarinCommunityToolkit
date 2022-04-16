using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System;using Microsoft.Extensions.Logging;
using System.Linq;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;
using View = Android.Views.View;

[assembly: ExportEffect(typeof(LifeCycleEffectRouter), nameof(LifecycleEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	/// <summary>
	/// Android implementation of the <see cref="LifecycleEffect" />
	/// </summary>
	public class LifeCycleEffectRouter : Microsoft.Maui.Controls.Platform.PlatformEffect
	{
		View? nativeView;
		LifecycleEffect? lifeCycleEffect;

		protected override void OnAttached()
		{
			lifeCycleEffect = Element.Effects.OfType<LifecycleEffect>().FirstOrDefault() ??
				throw new ArgumentNullException($"The effect {nameof(LifecycleEffect)} can't be null.");

			nativeView = Control ?? Container;

			nativeView.ViewAttachedToWindow += OnNativeViewViewAttachedToWindow;
			nativeView.ViewDetachedFromWindow += OnNativeViewViewDetachedFromWindow;
		}

		void OnNativeViewViewAttachedToWindow(object? sender, View.ViewAttachedToWindowEventArgs e) => lifeCycleEffect?.RaiseLoadedEvent(Element);

		void OnNativeViewViewDetachedFromWindow(object? sender, View.ViewDetachedFromWindowEventArgs e)
		{
			if (lifeCycleEffect != null)
				lifeCycleEffect.RaiseUnloadedEvent(Element);

			if (nativeView != null)
			{
				nativeView.ViewDetachedFromWindow -= OnNativeViewViewDetachedFromWindow;
				nativeView.ViewAttachedToWindow -= OnNativeViewViewAttachedToWindow;
			}

			nativeView = null;
			lifeCycleEffect = null;
		}

		protected override void OnDetached()
		{
		}
	}
}