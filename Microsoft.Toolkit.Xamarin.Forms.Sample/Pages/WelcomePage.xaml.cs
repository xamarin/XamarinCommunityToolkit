using System;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.Pages
{
    public partial class WelcomePage : BasePage
    {
        public WelcomePage()
            => InitializeComponent();

        async void OnAboutClicked(object sender, EventArgs e)
            => await Navigation.PushModalAsync(new BaseNavigationPage(new AboutPage()));
    }
}