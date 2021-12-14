using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Sample.Pages.Views.Popups;
using Xamarin.CommunityToolkit.Sample.ViewModels;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.TestCases.Popups
{
	public partial class GH_BUG_1761
	{
		public GH_BUG_1761() => InitializeComponent();

		private async void ButtonAB_Clicked(object sender, EventArgs e)
		{
			var popupA = new ReturnResultPopup();
			var result = await Navigation.ShowPopupAsync(popupA);
			var popupB = new ButtonPopup();
			Navigation.ShowPopup(popupB);
		}
	}


	public class GH_BUG_1761_ViewModel : BaseViewModel
	{
		public GH_BUG_1761_ViewModel()
		{

		}

		INavigation Navigation => Application.Current.MainPage.Navigation;
	}
}