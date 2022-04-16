using System;using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class SemanticOrderView : ContentView
	{
		public static readonly BindableProperty ViewOrderProperty =
			BindableProperty.Create(nameof(ViewOrder), typeof(IEnumerable), typeof(SemanticOrderView), new View[0]);

		public IEnumerable ViewOrder
		{
			get => (IEnumerable)GetValue(ViewOrderProperty);
			set => SetValue(ViewOrderProperty, value);
		}

		public SemanticOrderView()
		{
		}
	}
}