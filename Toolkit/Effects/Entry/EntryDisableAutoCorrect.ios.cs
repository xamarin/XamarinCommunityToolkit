using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;
using PlatformEffects = Xamarin.Toolkit.Effects.iOS;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.EntryDisableAutoCorrect), nameof(RoutingEffects.EntryDisableAutoCorrect))]
namespace Xamarin.Toolkit.Effects.iOS
{
    [Preserve]
    public class EntryDisableAutoCorrect : PlatformEffect
    {
        UITextSpellCheckingType spellCheckingType;
        UITextAutocorrectionType autocorrectionType;
        UITextAutocapitalizationType autocapitalizationType;

        protected override void OnAttached()
        {
            var editText = Control as UITextField;
            if (editText == null)
                return;

            spellCheckingType = editText.SpellCheckingType;
            autocorrectionType = editText.AutocorrectionType;
            autocapitalizationType = editText.AutocapitalizationType;

            editText.SpellCheckingType = UITextSpellCheckingType.No;             // No Spellchecking
            editText.AutocorrectionType = UITextAutocorrectionType.No;           // No Autocorrection
            editText.AutocapitalizationType = UITextAutocapitalizationType.None;
        }

        protected override void OnDetached()
        {
            var editText = Control as UITextField;
            if (editText == null)
                return;

            editText.SpellCheckingType = spellCheckingType;
            editText.AutocorrectionType = autocorrectionType;
            editText.AutocapitalizationType = autocapitalizationType;
        }
    }
}
