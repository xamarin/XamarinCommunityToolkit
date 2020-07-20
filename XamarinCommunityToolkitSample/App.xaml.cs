using Xamarin.Forms;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Pages;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample
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