using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;
using RoutingEffects = Xamarin.Toolkit.Effects;
using PlatformEffects = Xamarin.Toolkit.Effects.iOS;

[assembly: ExportEffect(typeof(PlatformEffects.EntryDisableAutoCorrect), nameof(RoutingEffects.EntryDisableAutoCorrect))]
namespace Xamarin.Toolkit.Effects.iOS
{
    [Preserve]
    public class EntryDisableAutoCorrect : PlatformEffect
    {
        private UITextSpellCheckingType _spellCheckingType;
        private UITextAutocorrectionType _autocorrectionType;
        private UITextAutocapitalizationType _autocapitalizationType;

        protected override void OnAttached()
        {
            var editText = Control as UITextField;
            if (editText == null) return;

            _spellCheckingType = editText.SpellCheckingType;
            _autocorrectionType = editText.AutocorrectionType;
            _autocapitalizationType = editText.AutocapitalizationType;

            editText.SpellCheckingType = UITextSpellCheckingType.No;             // No Spellchecking
            editText.AutocorrectionType = UITextAutocorrectionType.No;           // No Autocorrection
            editText.AutocapitalizationType = UITextAutocapitalizationType.None;
        }

        protected override void OnDetached()
        {
            var editText = Control as UITextField;
            if (editText == null) return;

            editText.SpellCheckingType = _spellCheckingType;
            editText.AutocorrectionType = _autocorrectionType;
            editText.AutocapitalizationType = _autocapitalizationType;
        }
    }
}