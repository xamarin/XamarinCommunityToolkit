using System;

namespace CommunityToolkit.Maui.Sample.Pages
{
	public partial class SettingPage : BasePage
	{
		public SettingPage()
		{
			InitializeComponent();
		}

		async void OnCloseClicked(object? sender, EventArgs e) => await Navigation.PopModalAsync();
	}
}