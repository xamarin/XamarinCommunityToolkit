using Xamarin.CommunityToolkit.Sample.Pages;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample
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