using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PlatformEffects = Xamarin.Toolkit.Effects.iOS;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.EntryCapitalizeKeyboard), nameof(RoutingEffects.EntryCapitalizeKeyboard))]
namespace Xamarin.Toolkit.Effects.iOS
{
    [Preserve(AllMembers = true)]
    public class EntryCapitalizeKeyboard : PlatformEffect
    {
        private UITextAutocapitalizationType _old;

        protected override void OnAttached()
        {
            var editText = Control as UITextField;
            if (editText == null)
            {
                return;
            }

            _old = editText.AutocapitalizationType;
            editText.AutocapitalizationType = UITextAutocapitalizationType.AllCharacters;
        }

        protected override void OnDetached()
        {
            var editText = Control as UITextField;
            if (editText == null)
            {
                return;
            }

            editText.AutocapitalizationType = _old;
        }
    }
}
