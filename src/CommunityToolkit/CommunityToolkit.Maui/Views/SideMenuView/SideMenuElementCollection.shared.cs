using System.Collections.ObjectModel;
using Xamarin.Forms;
using static CommunityToolkit.Maui.UI.Views.SideMenuView;

namespace CommunityToolkit.Maui.UI.Views
{
	sealed class SideMenuElementCollection : ObservableCollection<View>, ISideMenuList<View>
	{
		public void Add(View view, SideMenuPosition position)
		{
			SetPosition(view, position);
			Add(view);
		}

		public void Add(View view, SideMenuPosition position, double menuWidthPercentage)
		{
			SetMenuWidthPercentage(view, menuWidthPercentage);
			Add(view, position);
		}

		public void AddMainView(View view)
			=> Add(view, SideMenuPosition.MainView);

		public void AddMainView(View view, double menuWidthPercentage)
			=> Add(view, SideMenuPosition.MainView, menuWidthPercentage);

		public void AddLeftMenu(View view)
			=> Add(view, SideMenuPosition.LeftMenu);

		public void AddLeftMenu(View view, double menuWidthPercentage)
			=> Add(view, SideMenuPosition.LeftMenu, menuWidthPercentage);

		public void AddRightMenu(View view)
			=> Add(view, SideMenuPosition.RightMenu);

		public void AddRightMenu(View view, double menuWidthPercentage)
			=> Add(view, SideMenuPosition.RightMenu, menuWidthPercentage);
	}
}