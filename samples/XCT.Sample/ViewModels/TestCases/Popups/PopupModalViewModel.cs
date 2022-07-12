using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Sample.Pages.TestCases.Popups;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.TestCases.Popups
{
	public class PopupModalViewModel : BaseViewModel
	{
		public PopupModalViewModel()
		{
			ShowPopup = new Command(PerformShowPopup);
			PushModal = new Command(PerformPushModal);
			PopModal = new Command(PerformPopModal);
		}

		INavigation Navigation => Application.Current.MainPage.Navigation;

		public ICommand ShowPopup { get; }

		public ICommand PushModal { get; }

		public ICommand PopModal { get; }

		void PerformShowPopup() => Navigation.ShowPopup(new SimplePopup());

		async void PerformPushModal() => await Navigation.PushModalAsync(new PopupModalPage());

		async void PerformPopModal()
		{
			if (Navigation.ModalStack.Count > 0)
				await Navigation.PopModalAsync();
		}
	}
}