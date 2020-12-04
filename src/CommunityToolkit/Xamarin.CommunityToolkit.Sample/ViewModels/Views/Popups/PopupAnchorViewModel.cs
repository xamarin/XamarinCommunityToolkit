using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Sample.Pages.Views.Popups;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views.Popups
{
	public class PopupAnchorViewModel
	{
		INavigation Navigation => App.Current.MainPage.Navigation;

		public ICommand ShowPopup => new Command<View>(OnShowPopup);

		void OnShowPopup(View anchor)
		{
			var popup = new TransparentPopup();
			popup.Anchor = anchor;
			Navigation.ShowPopup(popup);
		}
	}
}
