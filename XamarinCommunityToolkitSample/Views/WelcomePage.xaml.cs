using System.Windows.Input;
using Xamarin.Forms;
using XamarinCommunityToolkitSample.Models;

namespace XamarinCommunityToolkitSample.Views
{
    public partial class WelcomePage : ContentPage
    {
        ICommand navigateCommand;

        public WelcomePage()
            => InitializeComponent();

        public ICommand NavigateCommand => navigateCommand ??= new Command(parameter
            => Navigation.PushAsync(GetPage((WelcomeSectionId)parameter)));

        Page GetPage(WelcomeSectionId id)
        {
            var page = id switch
            {
                WelcomeSectionId.Behaviors => new BehaviorsPage(),
                WelcomeSectionId.Converters => new ContentPage(),
                WelcomeSectionId.Views => new ContentPage(),
                WelcomeSectionId.TestCases => new ContentPage(),
                _ => throw new System.NotImplementedException()
            };
            page.Title = id.GetTitle();
            return page;
        }
    }
}
