using System;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinCommunityToolkitSample.Models;
using XamarinCommunityToolkitSample.Pages.Behaviors;
using XamarinCommunityToolkitSample.Pages.Converters;
using XamarinCommunityToolkitSample.Pages.Views;

namespace XamarinCommunityToolkitSample.Pages
{
    public partial class WelcomePage : ContentPage
    {
        ICommand navigateCommand;

        public WelcomePage()
            => InitializeComponent();

        public ICommand NavigateCommand => navigateCommand ??= new Command(parameter
            => Navigation.PushAsync(PreparePage((WelcomeSectionId)parameter)));

        Page PreparePage(WelcomeSectionId id)
        {
            var page = GetPage(id);
            page.Title = id.GetTitle();
            return page;
        }

        Page GetPage(WelcomeSectionId id)
            => id switch
            {
                WelcomeSectionId.Behaviors => new BehaviorsGalleryPage(),
                WelcomeSectionId.Converters => new ConvertersGalleryPage(),
                WelcomeSectionId.Views => new ViewsGalleryPage(),
                WelcomeSectionId.Extensions => new ContentPage(),
                WelcomeSectionId.TestCases => new ContentPage(),
                _ => throw new NotImplementedException()
            };

        async void OnAboutClicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new BaseNavigationPage(new AboutPage()));
    }
}