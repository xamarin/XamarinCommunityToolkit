using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class SemanticOrderViewPage : BasePage
	{
		public SemanticOrderViewPage()
		{
			InitializeComponent();
			acv.ViewOrder = new List<View> { first, second, third, fourth, fifth };
		}
	}
}