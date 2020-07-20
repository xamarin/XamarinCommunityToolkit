using System;
using System.Linq;
using Octokit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.Pages
{
    public partial class AboutPage : BasePage
    {
        public AboutPage()
            => InitializeComponent();

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ((AboutViewModel)BindingContext).OnAppearing();
        }

        async void OnCloseClicked(object sender, EventArgs e)
            => await Navigation.PopModalAsync();
    }
}