using System.Windows.Input;
using Xamarin.CommunityToolkit.Sample.Resx;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
{
    public class RequiredStringValidationBehaviorViewModel : BaseViewModel
    {
        public ICommand PasswordMatchedCommand { get; set; }

        public RequiredStringValidationBehaviorViewModel() => PasswordMatchedCommand = new Command(OnPasswordMatched);

        private static void OnPasswordMatched(object obj) => Application.Current.MainPage.DisplayAlert("", AppResources.PasswordMatched, AppResources.Cancel);
    }
}