using System;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinCommunityToolkitSample.Models;
using XamarinCommunityToolkitSample.Pages.Views;

namespace XamarinCommunityToolkitSample.Pages
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
                WelcomeSectionId.Views => new ViewsGalleryPage(),
                WelcomeSectionId.Extensions => new ContentPage(),
                WelcomeSectionId.TestCases => new ContentPage(),
                _ => throw new System.NotImplementedException()
            };
            page.Title = id.GetTitle();
            return page;
        }

        async void OnAboutClicked(Object sender, EventArgs e) => await Navigation.PushAsync(new AboutPage());
    }
}