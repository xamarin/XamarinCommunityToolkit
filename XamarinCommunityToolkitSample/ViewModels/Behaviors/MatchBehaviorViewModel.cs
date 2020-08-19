
using System.Windows.Input;
using Xamarin.CommunityToolkit.Sample.Resx;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
{
    public class MatchBehaviorViewModel : BaseViewModel
    {
        string text = $"{AppResources.MatchTypesIntroText}: #DotNetMAUI #Community #OpenSource <a href=\"https://www.xamarin.com\">Xamarin</a>  <a href=\"www.example.com\">Test</a> <a href=\"www.github.com\">Github</a> @xamarinhq @dotnet @microsoft #MicrosoftToolkitXamarinForms #Xamarin #Monkeys #XamarinForms @planetxamarin #ILoveXamarin #CSharp #Behaviors";

        public string Text
        {
            get => text;
            set => Set(ref text, value);
        }

        public ICommand TagTappedCommand => new Command<string>(async s => await Application.Current.MainPage.DisplayAlert("Tag Tapped:", s, "Ok"));
    }
}
