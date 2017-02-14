using System;
using FormsCommunityToolkit.Effects.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(DisableAutoCorrectEffect), nameof(DisableAutoCorrectEffect))]

namespace FormsCommunityToolkit.Effects.iOS
{
    [Preserve]
    public class DisableAutoCorrectEffect : PlatformEffect
    {
        UITextSpellCheckingType spellCheckingType;
        UITextAutocorrectionType autocorrectionType;
        UITextAutocapitalizationType autocapitalizationType;


        protected override void OnAttached()
        {
            var editText = Control as UITextField;
            if (editText == null) return;


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
            if (editText == null) return;

            editText.SpellCheckingType = spellCheckingType;
            editText.AutocorrectionType = autocorrectionType;
            editText.AutocapitalizationType = autocapitalizationType;
        }
    }
}