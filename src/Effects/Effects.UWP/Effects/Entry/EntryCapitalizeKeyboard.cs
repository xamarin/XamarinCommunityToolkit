using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.UWP;
using RoutingEffects = FormsCommunityToolkit.Effects;
using PlatformEffects = FormsCommunityToolkit.Effects.UWP;

[assembly: ExportEffect(typeof(PlatformEffects.EntryCapitalizeKeyboard), nameof(RoutingEffects.EntryCapitalizeKeyboard))]
namespace FormsCommunityToolkit.Effects.UWP
{
    [Preserve]
    public class EntryCapitalizeKeyboard : PlatformEffect
    {
        protected override void OnAttached()
        {
            var textbox = Control as TextBox;

            if (textbox == null)
                return;

            
            textbox.TextChanging -= TextboxOnTextChanging;
            textbox.TextChanging += TextboxOnTextChanging;
        }

        private static void TextboxOnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            var selectionStart = sender.SelectionStart;
            sender.Text = sender.Text.ToUpper();
            sender.SelectionStart = selectionStart;
            sender.SelectionLength = 0;
        }

        protected override void OnDetached()
        {
            var textbox = Control as TextBox;
            if (textbox == null)
                return;
            else
                textbox.TextChanging -= TextboxOnTextChanging;
        }
    }
}
