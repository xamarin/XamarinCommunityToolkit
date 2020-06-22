using System;
using System.Linq;
using Octokit;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinCommunityToolkitSample.ViewModels;

namespace XamarinCommunityToolkitSample.Pages
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((AboutViewModel)BindingContext).OnAppearing();
        }
    }
}