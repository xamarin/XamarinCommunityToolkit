using System.Globalization;
using CommunityToolkit.Maui.Helpers;
using CommunityToolkit.Maui.Sample.Pages;
using CommunityToolkit.Maui.Sample.Resx;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace CommunityToolkit.Maui.Sample
{
	public partial class App : Forms.Application
	{
		public App()
		{
			On<Windows>().SetImageDirectory("Assets");

			LocalizationResourceManager.Current.PropertyChanged += (sender, e) => AppResources.Culture = LocalizationResourceManager.Current.CurrentCulture;
			LocalizationResourceManager.Current.Init(AppResources.ResourceManager);
			LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("en");

			InitializeComponent();
			MainPage = new BaseNavigationPage(new WelcomePage());
		}
	}
}