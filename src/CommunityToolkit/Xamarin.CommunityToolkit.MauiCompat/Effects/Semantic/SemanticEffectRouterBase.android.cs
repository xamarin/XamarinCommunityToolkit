using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System.ComponentModel;
using System.Linq;
using Android.Views;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;

namespace Xamarin.CommunityToolkit.Android.Effects
{
	/// <summary>
	/// Base implementation of the <see cref="SemanticEffectRouter" />
	/// </summary>
	/// <typeparam name="T">Effect</typeparam>
	public class SemanticEffectRouterBase<T> : Microsoft.Maui.Controls.Platform.PlatformEffect
		where T : Effect
	{
		public SemanticEffectRouterBase()
		{
		}

		protected override void OnAttached()
		{
			var ve = Element as VisualElement;
			var control =
				ve?.GetViewForAccessibility() ?? ve?.GetViewForAccessibility(Control);

			if (control == null)
				Element.PropertyChanged += SetupControl;
			else
			{
				Update();
			}
		}

		protected virtual void Update(global::Android.Views.View view, T effect)
		{
		}

		protected void Update()
		{
			var effect = (T)Element.Effects.FirstOrDefault(e => e is T);
			var ve = Element as VisualElement;
			var veFinal = ve?.GetViewForAccessibility() ?? ve?.GetViewForAccessibility(Control);
			if (veFinal != null)
				Update(veFinal, effect);
		}

		void SetupControl(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Renderer")
			{
				Element.PropertyChanged -= SetupControl;
				Update();
			}
		}

		protected override void OnDetached() => Element.PropertyChanged -= SetupControl;
	}

	static class SemanticEffectRouterExtensions
	{
		public static global::Android.Views.View? GetViewForAccessibility(this VisualElement visualElement)
		{
			var renderer = Platform.GetRenderer(visualElement);

			if (visualElement is Microsoft.Maui.Controls.Layout)
				return renderer?.View;
			else if (renderer is ViewGroup vg && vg.ChildCount > 0)
				return vg?.GetChildAt(0);
			else if (renderer != null)
				return renderer.View;

			return null;
		}

		public static global::Android.Views.View? GetViewForAccessibility(this VisualElement visualElement, global::Android.Views.View renderer)
		{
			if (renderer == null)
				return visualElement?.GetViewForAccessibility();

			if (visualElement is Microsoft.Maui.Controls.Layout)
				return renderer;
			else if (renderer is ViewGroup vg && vg.ChildCount > 0)
				return vg?.GetChildAt(0);

			return renderer;
		}
	}
}