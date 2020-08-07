
using System.Windows.Input;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Resx;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Behaviors
{
    public class MatchBehaviorViewModel : BaseViewModel
    {
        string text = $"{AppResources.MatchTypesIntroText}: #DotNetMAUI #Community #OpenSource https://www.xamarin.com www.github.com @xamarinhq @dotnet @microsoft #MicrosoftToolkitXamarinForms #Xamarin #Monkeys #XamarinForms @planetxamarin #ILoveXamarin #CSharp #Behaviors";

        public string Text
        {
            get => text;
            set => Set(ref text, value);
        }

        public ICommand TagTappedCommand => new Command<string>(async s => await Application.Current.MainPage.DisplayAlert("Tag Tapped:", s, "Ok"));
    }
}
