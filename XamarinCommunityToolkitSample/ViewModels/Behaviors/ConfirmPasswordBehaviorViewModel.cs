using System.Windows.Input;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Behaviors
{
    public class ConfirmPasswordBehaviorViewModel : BaseViewModel
    {
        public ICommand SamePasswordCommand { get; set; }

        public ConfirmPasswordBehaviorViewModel() => SamePasswordCommand = new Command(IsSamePassword);

        private static void IsSamePassword(object obj) => Application.Current.MainPage.DisplayAlert("alert", $"Password {obj} matched", "ok");
    }
}