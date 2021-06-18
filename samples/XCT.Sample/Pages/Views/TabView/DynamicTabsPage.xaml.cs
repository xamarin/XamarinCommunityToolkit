using System;
using System.Collections.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.TabView
{
		public partial class DynamicTabsPage
	{
		private int TabCounter { get; set; } = 3;
		public ObservableCollection<string> TabSource { get; }

		public static readonly BindableProperty StartPositionProperty = BindableProperty.Create(
			nameof(StartPosition),
			typeof(string),
			typeof(DynamicTabsPage),
			"0");

		public string StartPosition
		{
			get => (string)GetValue(StartPositionProperty);
			set => SetValue(StartPositionProperty, value);
		}

		public static readonly BindableProperty EndPositionProperty = BindableProperty.Create(
			nameof(EndPosition),
			typeof(string),
			typeof(DynamicTabsPage),
			"1");

		public string EndPosition
		{
			get => (string)GetValue(EndPositionProperty);
			set => SetValue(EndPositionProperty, value);
		}

		public DynamicTabsPage()
		{
			TabSource = new ObservableCollection<string> { "1", "2" };
			BindingContext = this;
			InitializeComponent();
		}

		private void OnAdd(object sender, EventArgs e)
		{
			MyTabView.TabItems.Add(new TabViewItem()
			{
				Text = $"Tab {TabCounter}",
				Content = new Grid()
				{
					Children =
					{
						new Label { Text = $"Tab {TabCounter}"}
					}
				}
			});

			TabSource.Add(TabCounter.ToString());
			TabCounter++;
		}

		private void OnInsert(object sender, EventArgs e)
		{
			var start = Convert.ToInt32(StartPosition);
			MyTabView.TabItems.Insert(start, new TabViewItem()
			{
				Text = $"Tab {TabCounter}",
				Content = new Grid()
				{
					Children =
					{
						new Label { Text = $"Tab {TabCounter}"}
					}
				}
			});

			TabSource.Insert(start, TabCounter.ToString());
			TabCounter++;
		}

		private void OnDelete(object sender, EventArgs e)
		{
			var start = Convert.ToInt32(StartPosition);
			MyTabView.TabItems.RemoveAt(start);
			TabSource.RemoveAt(start);
		}

		private void OnMove(object sender, EventArgs e)
		{
			var start = Convert.ToInt32(StartPosition);
			var end = Convert.ToInt32(EndPosition);
			MyTabView.TabItems.Move(start, end);
			TabSource.Move(start, end);
		}
	}
}