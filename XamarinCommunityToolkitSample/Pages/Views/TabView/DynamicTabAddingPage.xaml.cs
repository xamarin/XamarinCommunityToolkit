using System;
using System.Collections.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.TabView
{
	public partial class DynamicTabAddingPage
	{
		public ObservableCollection<string> TabSource { get; }

		public DynamicTabAddingPage()
		{
			TabSource = new ObservableCollection<string> {"One", "Two"};
			BindingContext = this;
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

			TabSource.Add("Three");
		}
	}
}