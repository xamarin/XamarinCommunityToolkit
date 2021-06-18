using System.Linq;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.TabView
{
	public partial class TabItemsPage : BasePage
	{
		public TabItemsPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			var tabContent1 = new Grid { BackgroundColor = Color.Gray };
			tabContent1.Children.Add(new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "TabContent1" });

			var tabContent2 = new Grid { };
			tabContent2.Children.Add(new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "TabContent2" });

			foreach (var tab in MyTabView.TabItems.ToList())
				MyTabView.TabItems.Remove(tab);

			MyTabView.TabItems.Add(new TabViewItem { Text = "Tab 1", Icon = "triangle.png", TextColor = Color.White, TextColorSelected = Color.Yellow, Content = tabContent1 });
			MyTabView.TabItems.Add(new TabViewItem { Text = "Tab 2", Icon = "circle.png", TextColor = Color.White, TextColorSelected = Color.Yellow, Content = tabContent2 });

			MyTabView.SelectedIndex = 0;
		}
	}
}