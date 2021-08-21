using Paint = Android.Graphics.Paint;using Path = Android.Graphics.Path;﻿using System.Linq;
using Android.Content;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.UI.Views;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;

[assembly: ExportRenderer(typeof(SemanticOrderView), typeof(SemanticOrderViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class SemanticOrderViewRenderer : ViewRenderer
	{
		SemanticOrderView AccessibilityContentView => (SemanticOrderView)Element;

		public SemanticOrderViewRenderer(Context context)
			: base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);
			SetAccessibilityElements();
		}

		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			SetAccessibilityElements();
			base.OnLayout(changed, left, top, right, bottom);
		}

		void SetAccessibilityElements()
		{
			var viewOrder = AccessibilityContentView.ViewOrder.OfType<View>().ToList();

			for (var i = 1; i < viewOrder.Count; i++)
			{
				var view1 = viewOrder[i - 1].GetViewForAccessibility();
				var view2 = viewOrder[i].GetViewForAccessibility();

				if (view1 == null || view2 == null)
					return;

				view2.AccessibilityTraversalAfter = view1.Id;
				view1.AccessibilityTraversalBefore = view2.Id;
			}
		}
	}
}