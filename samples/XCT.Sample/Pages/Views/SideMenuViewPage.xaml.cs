using System;
using CommunityToolkit.Maui.UI.Views;

namespace CommunityToolkit.Maui.Sample.Pages.Views
{
	public partial class SideMenuViewPage
	{
		public SideMenuViewPage()
			=> InitializeComponent();

		void OnLeftButtonClicked(object? sender, EventArgs e)
			=> SideMenuView.State = SideMenuState.LeftMenuShown;

		void OnRightButtonClicked(object? sender, EventArgs e)
			=> SideMenuView.State = SideMenuState.RightMenuShown;
	}
}