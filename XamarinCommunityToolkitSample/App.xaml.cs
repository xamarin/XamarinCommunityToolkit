using Xamarin.Forms;
using Xamarin.CommunityToolkit.Sample.Pages;

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