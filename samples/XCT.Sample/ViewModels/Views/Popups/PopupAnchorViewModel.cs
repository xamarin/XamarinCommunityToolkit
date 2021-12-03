using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Sample.Pages.Views.Popups;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views.Popups
{
	public class PopupAnchorViewModel
	{
		public PopupAnchorViewModel()
		{
			ShowPopup = new Command<View>(OnShowPopup);
		}

		INavigation Navigation => Application.Current.MainPage.Navigation;

		public ICommand ShowPopup { get; }

		void OnShowPopup(View anchor)
		{
			var popup = new TransparentPopup();
			popup.Anchor = anchor;
			Navigation.ShowPopup(popup);
		}
	}
}