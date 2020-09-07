using System;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class SideMenuViewPage
	{
		public SideMenuViewPage()
			=> InitializeComponent();

		void OnLeftButtonClicked(object sender, EventArgs e)
			=> SideMenuView.State = SideMenuState.LeftMenuShown;

		void OnRightButtonClicked(object sender, EventArgs e)
			=> SideMenuView.State = SideMenuState.RightMenuShown;
	}
}