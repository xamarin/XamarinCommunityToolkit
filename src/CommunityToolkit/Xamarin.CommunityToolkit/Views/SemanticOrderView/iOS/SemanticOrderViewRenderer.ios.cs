using System.Collections.Generic;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SemanticOrderView), typeof(SemanticOrderViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class SemanticOrderViewRenderer : ViewRenderer, IUIAccessibilityContainer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			var result = GetAccessibilityElements();

			if (result != null)
				this.SetAccessibilityElements(NSArray.FromNSObjects(result.ToArray()));
		}

		SemanticOrderView AccessibilityContentView => (SemanticOrderView)Element;

		List<NSObject>? GetAccessibilityElements()
		{
			if (AccessibilityContentView == null)
				return null;

			var viewOrder = AccessibilityContentView.ViewOrder;

			var returnValue = new List<NSObject>();
			foreach (View view in viewOrder)
			{
				returnValue.Add(Platform.GetRenderer(view).NativeView);
			}

			return returnValue.Count == 0 ? null : returnValue;
		}
	}
}
