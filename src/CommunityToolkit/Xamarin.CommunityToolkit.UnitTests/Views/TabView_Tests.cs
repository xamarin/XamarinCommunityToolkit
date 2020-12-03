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
		public void TestConstructor()
		{
			var tabView = new TabView();
			var tabViewItem = new TabViewItem();
			tabView.TabItems.Add(tabViewItem);

			Assert.Single(tabView.TabItems);
		}

		[Fact]
		public void TestAddRemoveTabViewItems()
		{
			var tabView = new TabView();
			var tabViewItem = new TabViewItem();
			tabView.TabItems.Add(tabViewItem);

			Assert.Single(tabView.TabItems);

			tabView.TabItems.Remove(tabViewItem);

			Assert.Empty(tabView.TabItems);
		}

		[Fact]
		public void TestTabViewItemParent()
		{
			var tabView = new TabView();
			var tabViewItem = new TabViewItem();
			tabView.TabItems.Add(tabViewItem);

			Assert.Equal(tabView, tabViewItem.Parent);
		}
	}
}