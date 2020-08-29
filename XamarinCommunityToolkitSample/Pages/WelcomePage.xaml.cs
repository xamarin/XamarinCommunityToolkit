using System;

namespace Xamarin.CommunityToolkit.Sample.Pages
{
	public partial class WelcomePage : BasePage
	{
		public WelcomePage()
			=> InitializeComponent();

		async void OnAboutClicked(object sender, EventArgs e)
			=> await Navigation.PushModalAsync(new BaseNavigationPage(new AboutPage()));

		async void OnSettingsClicked(object sender, EventArgs e)
			=> await Navigation.PushModalAsync(new BaseNavigationPage(new SettingPage()));
	}
}