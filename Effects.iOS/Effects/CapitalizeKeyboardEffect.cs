using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsCommunityToolkit.Effects.iOS.Effects;
using UIKit;
using Foundation;

[assembly: ExportEffect(typeof(CapitalizeKeyboardEffect), nameof(CapitalizeKeyboardEffect))]

namespace FormsCommunityToolkit.Effects.iOS.Effects
{
    [Preserve(AllMembers = true)]
	public class CapitalizeKeyboardEffect : PlatformEffect
	{
        private UITextAutocapitalizationType _old;

		protected override void OnAttached()
		{
            var editText = Control as UITextField;
			if (editText != null)
			{
                _old = editText.AutocapitalizationType;
                editText.AutocapitalizationType = UITextAutocapitalizationType.AllCharacters;
			}
		}

		protected override void OnDetached()
		{
			var editText = Control as UITextField;
			if (editText != null)
				editText.AutocapitalizationType = _old;
		}
	}
}
