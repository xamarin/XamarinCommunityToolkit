using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OtpEffectPage
	{
		public OtpEffectPage()
		{
			InitializeComponent();
		}

		async void OnOtpReceived(object sender, CommunityToolkit.Effects.OtpReceivedEventArgs e)
		{
			await DisplayAlert("One time password received", $"Received one time password '{e.Code}'.", "ok");
		}
	}
}