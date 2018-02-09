using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinCommunityToolkit.Samples.Views
{
    public partial class EffectsPage : ContentPage
    {
        public EffectsPage()
        {
            InitializeComponent();
        }

        private async void OnLabelButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsLabelPage());
        }

        private async void OnEntryButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntryPage());
        }

        private async void OnSwitchButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsSwitchPage());
        }

        private async void OnViewButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsViewPage());
        }
    }
}