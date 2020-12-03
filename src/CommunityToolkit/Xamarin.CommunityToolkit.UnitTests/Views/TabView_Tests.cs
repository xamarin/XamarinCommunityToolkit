using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class TabView_Tests
	{
		public TabView_Tests()
			=> Device.PlatformServices = new MockPlatformServices();

		[Fact]
		public void TestPropagateBindingContext()
		{
			var bindingContext = new object();

			var tabView = new TabView();

			var contentView1 = new ContentView();

			var tabViewItem1 = new TabViewItem
			{
				Text = "Tab 1"
			};

			tabViewItem1.Content = contentView1;

			var contentView2 = new ContentView();

			var tabViewItem2 = new TabViewItem
			{
				Text = "Tab 2"
			};

			tabViewItem2.Content = contentView2;

			tabView.TabItems.Add(tabViewItem1);
			tabView.TabItems.Add(tabViewItem2);

			tabView.BindingContext = bindingContext;

			Assert.Equal(bindingContext, tabViewItem1.BindingContext);
			Assert.Equal(bindingContext, contentView1.BindingContext);

			Assert.Equal(bindingContext, tabViewItem2.BindingContext);
			Assert.Equal(bindingContext, contentView2.BindingContext);
		}
	}
}
