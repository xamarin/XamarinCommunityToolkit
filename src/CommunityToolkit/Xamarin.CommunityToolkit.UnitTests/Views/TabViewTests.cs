#if NETCOREAPP3_1_OR_GREATER
using NUnit.Framework;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class TabViewTests
	{
		[Test]
		public void TestTabView_TabViewItemGridLengthProperty()
		{
			var tabView = new TabView()
			{
				TabViewItemGridLength = Forms.GridLength.Auto
			};

			tabView.TabItems.Add(new TabViewItem() { Text = "Tab 1" });
			tabView.TabItems.Add(new TabViewItem() { Text = "Tab 2 with longer text" });

			Assert.That(tabView.TabItems[0].Width, Is.Not.EqualTo(tabView.TabItems[1].Width), "These 2 should have different widths because we set the tab strip to have GridLength.Auto");
		}
	}
}
#endif
