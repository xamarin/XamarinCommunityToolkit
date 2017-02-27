using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using RoutingEffects = FormsCommunityToolkit.Effects;
using PlatformEffects = FormsCommunityToolkit.Effects.Droid;

[assembly: ExportEffect(typeof(PlatformEffects.EntryDisableAutoCorrect), nameof(RoutingEffects.EntryDisableAutoCorrect))]
namespace FormsCommunityToolkit.Effects.Droid
{
    public class EntryDisableAutoCorrect : PlatformEffect
    {
        private InputTypes _old;

        protected override void OnAttached()
        {
            var editText = Control as EditText;
            if (editText == null)
                return;

            _old = editText.InputType;
            editText.InputType = editText.InputType | Android.Text.InputTypes.TextFlagNoSuggestions;
        }

        protected override void OnDetached()
        {
            var editText = Control as EditText;
            if (editText == null)
                return;

            editText.InputType = _old;
        }
    }
}