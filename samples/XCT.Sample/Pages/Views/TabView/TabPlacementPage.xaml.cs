using System;
using CommunityToolkit.Maui.UI.Views;

namespace CommunityToolkit.Maui.Sample.Pages.Views.TabView
{
	public partial class TabPlacementPage : BasePage
	{
		public TabPlacementPage() => InitializeComponent();

		void OnChangeTabStripPlacementClicked(object? sender, EventArgs e)
		{
			if (TabView.TabStripPlacement == TabStripPlacement.Bottom)
				TabView.TabStripPlacement = TabStripPlacement.Top;
			else
				TabView.TabStripPlacement = TabStripPlacement.Bottom;
		}
	}
}