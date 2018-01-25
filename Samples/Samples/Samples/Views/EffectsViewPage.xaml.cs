using Xamarin.Forms;

namespace XamarinCommunityToolkit.Samples.Views
{
    public partial class EffectsViewPage : ContentPage
    {
        public EffectsViewPage()
        {
            InitializeComponent();
        }

        private async void OnBlurImageButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsViewBlurImagePage());
        }
    }
}