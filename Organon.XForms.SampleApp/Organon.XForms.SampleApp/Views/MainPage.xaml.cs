using System;
using Xamarin.Forms;

namespace FormsCommunityToolkit.Effects.SampleApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnEntryButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EntryPage());
        }

        private void OnViewButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewPage());
        }

        private void OnSwitchButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SwitchPage());
        }

        private void OnLabelButtonClicked(object sender, EventArgs args)
        {
            Navigation.PushAsync(new LabelPage());
        }
    }
}
