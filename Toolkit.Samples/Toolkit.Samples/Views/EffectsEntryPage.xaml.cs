using Xamarin.Forms;

namespace Xamarin.Toolkit.Samples.Views
{
    public partial class EffectsEntryPage : ContentPage
    {
        public EffectsEntryPage()
        {
            InitializeComponent();
        }

        async void OnCapitalizeKeyboardButtonClicked(object sender, System.EventArgs e)
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

        async void OnItalicPlaceholderButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntryItalicPlaceholderPage());
        }

        async void OnRemoveBorderButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntryRemoveBorderPage());
        }

        async void OnRemoveLineButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntryRemoveLinePage());
        }

        async void OnSelectAllTextButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsEntrySelectAllTextPage());
        }
    }
}
