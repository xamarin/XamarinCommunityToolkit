using System;

namespace CommunityToolkit.Maui.Sample.Pages
{
	public partial class AboutPage : BasePage
	{
		public AboutPage()
			=> InitializeComponent();

		async void OnCloseClicked(object? sender, EventArgs e)
			=> await Navigation.PopModalAsync();
	}
}