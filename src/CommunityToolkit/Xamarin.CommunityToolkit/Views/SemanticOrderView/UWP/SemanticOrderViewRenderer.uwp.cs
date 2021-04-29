using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(SemanticOrderView), typeof(SemanticOrderViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class SemanticOrderViewRenderer : ViewRenderer<ContentView, FrameworkElement>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
		{
			base.OnElementChanged(e);
			var results = GetAccessibilityElements();

			if (results == null)
				return;

			for (var i = 0; i < results.Count; i++)
			{
				if (results[i] is Control control)
				{
					control.TabNavigation = Windows.UI.Xaml.Input.KeyboardNavigationMode.Once;
					control.TabIndex = i;
				}
			}
		}

		SemanticOrderView AccessibilityContentView => (SemanticOrderView)Element;

		List<FrameworkElement>? GetAccessibilityElements()
		{
			var viewOrder = AccessibilityContentView.ViewOrder;

			var returnValue = new List<FrameworkElement>();
			foreach (View view in viewOrder)
			{
				returnValue.Add(Platform.GetRenderer(view).ContainerElement);
			}

			return returnValue.Count == 0 ? null : returnValue;
		}
	}
}
