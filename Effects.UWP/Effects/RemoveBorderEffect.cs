using Windows.UI.Xaml.Controls;
using FormsCommunityToolkit.Effects.UWP.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Thickness = Windows.UI.Xaml.Thickness;

[assembly: ExportEffect(typeof(RemoveBorderEffect), nameof(RemoveBorderEffect))]

namespace FormsCommunityToolkit.Effects.UWP.Effects
{
    public class RemoveBorderEffect : PlatformEffect
    {
        private Thickness _old;

        protected override void OnAttached()
        {
            var textBox = Control as TextBox;
            if (textBox != null)
            {
                _old = textBox.BorderThickness;
                textBox.BorderThickness = new Thickness(0);
            }
        }

        protected override void OnDetached()
        {
            var textBox = Control as TextBox;
            if (textBox != null)
            {
                textBox.BorderThickness = _old;
            }
        }
    }
}
