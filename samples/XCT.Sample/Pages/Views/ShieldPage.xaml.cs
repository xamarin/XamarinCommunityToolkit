using System;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class ShieldPage
	{
		public ShieldPage() => InitializeComponent();

		async void OnShieldTapped(object sender, EventArgs e) => await DisplayAlert("Shield Event", "C# Shield Tapped", "Ok");
	}
}