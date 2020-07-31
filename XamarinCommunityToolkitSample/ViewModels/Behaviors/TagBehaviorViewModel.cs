
using System.Windows.Input;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Behaviors
{
    public class TagBehaviorViewModel : BaseViewModel
    {
        string text = "Hello, here is a list of hashtags and other types of tags: #DotNetMAUI #Community #OpenSource @xamarinhq @dotnet @microsoft #MicrosoftToolkitXamarinForms #Xamarin #Monkeys #XamarinForms @planetxamarin #ILoveXamarin #CSharp #Behaviors";

        public string Text
        {
            get => text;
            set => Set(ref text, value);
        }

        ICommand tagTappedCommand;

        public ICommand TagTappedCommand => tagTappedCommand ?? new Command<string>(async (s) => await Application.Current.MainPage.DisplayAlert("Tag Tapped:", s, "Ok"));
    }
}
