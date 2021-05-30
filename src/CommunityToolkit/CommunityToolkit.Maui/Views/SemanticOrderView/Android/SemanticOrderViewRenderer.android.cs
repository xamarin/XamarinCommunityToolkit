using System.Linq;
using Android.Content;
using CommunityToolkit.Maui.Android.Effects;
using CommunityToolkit.Maui.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SemanticOrderView), typeof(SemanticOrderViewRenderer))]

namespace CommunityToolkit.Maui.UI.Views
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