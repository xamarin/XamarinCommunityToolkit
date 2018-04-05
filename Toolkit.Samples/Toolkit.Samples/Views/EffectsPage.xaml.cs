using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Toolkit.Samples.Views
{
    public partial class EffectsPage : ContentPage
    {
        public EffectsPage()
        {
            InitializeComponent();
        }

        async void OnLabelButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsLabelPage());
        }

        async void OnEntryButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntryPage());
        }

        async void OnSwitchButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsSwitchPage());
        }

        async void OnViewButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsViewPage());
        }
    }
}
