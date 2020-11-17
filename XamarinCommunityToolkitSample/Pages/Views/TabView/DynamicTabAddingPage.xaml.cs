using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.TabView
{
	public partial class DynamicTabAddingPage
	{
		public DynamicTabAddingPage()
		{
			InitializeComponent();
		}

		private void OnAdd(object sender, EventArgs e)
		{
			MyTabView.TabItems.Add(new TabViewItem()
			{
				Text = "Three",
				Content = new Grid()
				{
					Children =
					{
						new Label { Text = "Tab 3"}
					}
				}
			});
		}
	}
}