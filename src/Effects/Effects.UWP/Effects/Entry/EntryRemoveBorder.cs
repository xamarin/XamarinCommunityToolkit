using Windows.UI.Xaml.Controls;
using FormsCommunityToolkit.Effects.UWP;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportEffect(typeof(EntryRemoveBorder), nameof(EntryRemoveBorder))]
namespace FormsCommunityToolkit.Effects.UWP
{
    public class EntryRemoveBorder : PlatformEffect
    {
        private Windows.UI.Xaml.Thickness _old;

        protected override void OnAttached()
        {
            var textBox = Control as TextBox;
            if (textBox == null)
                return;
            _old = textBox.BorderThickness;
            textBox.BorderThickness = new Windows.UI.Xaml.Thickness(0);
        }


        protected override void OnDetached()
        {
            var textBox = Control as TextBox;
            if (textBox == null)
                return;

            textBox.BorderThickness = _old;
        }
    }
}