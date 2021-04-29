using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(SemanticOrderView), typeof(SemanticOrderViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class SemanticOrderViewRenderer : ViewRenderer<SemanticOrderView, FrameworkElement>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<SemanticOrderView> e)
		{
			base.OnElementChanged(e);
			UpdateViewOrder();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == SemanticOrderView.ViewOrderProperty.PropertyName)
				UpdateViewOrder();
		}

		void UpdateViewOrder()
		{
			var i = 1;
			foreach (var element in Element.ViewOrder)
			{
				if (element is VisualElement ve)
					ve.TabIndex = i++;
			}
		}
	}
}
