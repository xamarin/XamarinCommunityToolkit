using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Xamarin.CommunityToolkit.Sample.Pages;
using Xamarin.CommunityToolkit.Sample.Pages.Behaviors;
using Application = Microsoft.Maui.Controls.Application;

namespace XCT.MauiCompat.Sample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

			MainPage = new BaseNavigationPage(new EmailValidationBehaviorPage());
        }
    }
}
