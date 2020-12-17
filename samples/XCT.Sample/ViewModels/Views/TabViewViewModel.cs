using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Views.TabView;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class TabViewViewModel : BaseGalleryViewModel
	{
		public override IEnumerable<SectionModel> Items { get; } = new List<SectionModel>
		{
			new SectionModel(typeof(GettingStartedPage), "Getting Started",
				"TabView basic use case"),

			new SectionModel(typeof(CustomTabsPage), "Custom Tabs",
				"How to apply a custom appearance to any tabs"),

			new SectionModel(typeof(ScrollTabsPage), "Scrollable Tabs",
				"When the number of tabs is high, we can have scrollable tabs"),

			new SectionModel(typeof(TabItemsSourcePage), "Using TabItemsSource",
				"We can create tabs from a source"),

			new SectionModel(typeof(NestedTabsPage), "Nested Tabs",
				"Nest tabs!"),

			new SectionModel(typeof(TabBadgePage), "Tab Badge",
				"Badge support in tabs!"),

			new SectionModel(typeof(TabItemAnimationPage), "TabItem Animation",
				"We can use custom animations with tabs"),

			new SectionModel(typeof(IsTabStripVisiblePage), "IsTabStripVisible",
				"Show or hide the TabStrip"),

			new SectionModel(typeof(TabPlacementPage), "TabPlacement",
				"Be able to change the position of the tabs dynamically"),

			new SectionModel(typeof(TabWidthPage), "TabWidth",
				"Customize the tabs width"),

			new SectionModel(typeof(NoContentPage), "Tab without Content",
				"Only the TabStrip is visible")
		};
	}
}