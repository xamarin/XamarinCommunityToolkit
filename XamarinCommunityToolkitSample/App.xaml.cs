using Xamarin.Forms;
using XamarinCommunityToolkitSample.Pages;

namespace XamarinCommunityToolkitSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new BaseNavigationPage(new WelcomePage());
        }
    }
}