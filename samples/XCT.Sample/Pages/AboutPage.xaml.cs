using System;

namespace Xamarin.CommunityToolkit.Sample.Pages
{
	public partial class AboutPage : BasePage
	{
		public AboutPage()
			=> InitializeComponent();

		async void OnCloseClicked(object? sender, EventArgs e)
			=> await Navigation.PopModalAsync();
	}
}