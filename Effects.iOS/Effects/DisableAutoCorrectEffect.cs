using System;
using FormsCommunityToolkit.Effects.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(DisableAutoCorrectEffect), nameof(DisableAutoCorrectEffect))]

namespace FormsCommunityToolkit.Effects.iOS.Effects
{
	[Preserve]
	public class DisableAutoCorrectEffect : PlatformEffect
	{
		UITextSpellCheckingType _spellCheckingType;
		UITextAutocorrectionType _autocorrectionType;
		UITextAutocapitalizationType _autocapitalizationType;


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