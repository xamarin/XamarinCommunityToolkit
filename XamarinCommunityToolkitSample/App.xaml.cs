using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.Sample.Pages;
using Xamarin.CommunityToolkit.Sample.Resx;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample
{
	public partial class App : Application
	{
		public App()
		{
			LocalizationResourceManager.Current.Init(AppResources.ResourceManager);

			InitializeComponent();
			MainPage = new BaseNavigationPage(new WelcomePage());
		}
	}
}