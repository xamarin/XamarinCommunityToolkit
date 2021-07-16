namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class FullScreenLeanBackPage
	{
		public FullScreenLeanBackPage() => InitializeComponent();

		async void Button_Clicked(object sender, System.EventArgs e)
		{
			await Navigation.PopAsync();
		}
	}
}