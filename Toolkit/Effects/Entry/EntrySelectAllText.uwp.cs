using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;
using PlatformEffects = Xamarin.Toolkit.Effects.UWP;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.EntrySelectAllText), nameof(RoutingEffects.EntrySelectAllText))]
namespace Xamarin.Toolkit.Effects.UWP
{
    [Preserve]
    public class EntrySelectAllText : PlatformEffect
    {
        protected override void OnAttached()
        {
            var textBox = Control as TextBox;
            if (textBox == null)
                return;

            textBox.GotFocus -= TextboxOnGotFocus;
            textBox.GotFocus += TextboxOnGotFocus;
        }

        void TextboxOnGotFocus(object sender, RoutedEventArgs e) =>
            ((TextBox)sender).SelectAll();

        protected override void OnDetached()
        {
            var textbox = Control as TextBox;
            if (textbox == null)
                return;

            textbox.GotFocus -= TextboxOnGotFocus;
        }
    }
}
