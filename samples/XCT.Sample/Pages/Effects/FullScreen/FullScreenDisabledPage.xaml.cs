namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class FullScreenDisabledPage
	{
		public FullScreenDisabledPage() => InitializeComponent();

		async void Button_Clicked(object sender, System.EventArgs e)
		{
			await Navigation.PopAsync();
		}
	}
}