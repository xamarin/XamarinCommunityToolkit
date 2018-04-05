using Xamarin.Forms;

namespace Xamarin.Samples.Views
{
    public partial class EffectsLabelPage : ContentPage
    {
        public EffectsLabelPage()
        {
            InitializeComponent();
        }

        private async void OnSizeFontToFitButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsLabelSizeFontToFitPage());
        }
    }
}
