using System.Globalization;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.Sample.Pages;
using Xamarin.CommunityToolkit.Sample.Resx;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace Xamarin.CommunityToolkit.Sample
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