using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FormsCommunityToolkit.Samples.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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