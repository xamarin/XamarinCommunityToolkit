using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsCommunityToolkit.Samples.Views
{
    public partial class EffectsPage : ContentPage
    {
        public EffectsPage()
        {
            InitializeComponent();
        }

        private async void OnEntryButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntryPage());
        }

        private async void OnSwitchButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsSwitchPage());
        }
    }
}