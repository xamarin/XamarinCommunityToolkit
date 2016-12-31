using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsCommunityToolkit.Effects.iOS;
using UIKit;
using Foundation;

[assembly: ExportEffect(typeof(CapitalizeKeyboardEffect), nameof(CapitalizeKeyboardEffect))]

namespace FormsCommunityToolkit.Effects.iOS
{
    [Preserve(AllMembers = true)]
	public class CapitalizeKeyboardEffect : PlatformEffect
	{
        UITextAutocapitalizationType old;

		protected override void OnAttached()
		{
            var editText = Control as UITextField;
			if (editText == null)
				return;

			old = editText.AutocapitalizationType;
			editText.AutocapitalizationType = UITextAutocapitalizationType.AllCharacters;
		}

		protected override void OnDetached()
		{
			var editText = Control as UITextField;
			if (editText == null)
				return;

			editText.AutocapitalizationType = old;
		}
	}
}
