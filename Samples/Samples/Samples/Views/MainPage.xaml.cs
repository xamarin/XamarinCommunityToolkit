using Xamarin.Forms;

namespace FormsCommunityToolkit.Samples.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnAnimationsButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AnimationsPage());
        }

        private async void OnEffectsButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsPage());
        }

        private async void OnControlsButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ControlsPage());
        }
    }
}
