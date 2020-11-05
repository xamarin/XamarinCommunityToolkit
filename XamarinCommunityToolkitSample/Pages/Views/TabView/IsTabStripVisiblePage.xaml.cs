using System;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.TabView
{
	public partial class IsTabStripVisiblePage : BasePage
	{
		public IsTabStripVisiblePage() => InitializeComponent();
		
		void OnChangeTabStripPlacementClicked(object sender, EventArgs e)
		{
			if (TabView.TabStripPlacement == TabStripPlacement.Bottom)
				TabView.TabStripPlacement = TabStripPlacement.Top;
			else
				TabView.TabStripPlacement = TabStripPlacement.Bottom;
		}
	}
}