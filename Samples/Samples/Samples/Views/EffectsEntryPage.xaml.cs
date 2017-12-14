using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsCommunityToolkit.Samples.Views
{
    public partial class EffectsEntryPage : ContentPage
    {
        public EffectsEntryPage()
        {
            InitializeComponent();
        }

        private async void OnCapitalizeKeyboardButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntryCapitalizeKeyboardPage());
        }

        private async void OnClearButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntryClearPage());
        }

        private async void OnDisableAutoCorrectButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntryDisableAutoCorrectPage());
        }

        private async void OnItalicPlaceholderButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntryItalicPlaceholderPage());
        }

        private async void OnRemoveBorderButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntryRemoveBorderPage());
        }

        private async void OnRemoveLineButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntryRemoveLinePage());
        }

        private async void OnSelectAllTextButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntrySelectAllTextPage());
        }
    }
}