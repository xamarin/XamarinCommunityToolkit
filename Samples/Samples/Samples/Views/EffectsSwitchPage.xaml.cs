using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.Toolkit.Samples.Views
{
    public partial class EffectsSwitchPage : ContentPage
    {
        public EffectsSwitchPage()
        {
            InitializeComponent();
        }

        private async void OnChangeColorButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new EffectsSwitchChangeColorPage());
        }
    }
}