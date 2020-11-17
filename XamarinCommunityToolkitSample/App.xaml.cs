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
			LocalizationResourceManager.Current.Init(AppResources.ResourceManager);

			InitializeComponent();
			MainPage = new BaseNavigationPage(new WelcomePage());
		}
	}
}