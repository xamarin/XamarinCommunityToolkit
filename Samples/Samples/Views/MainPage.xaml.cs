using Xamarin.Forms;

namespace Xamarin.Samples.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void OnAnimationsButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AnimationsPage());
        }

        async void OnEffectsButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsPage());
        }
    }
}
