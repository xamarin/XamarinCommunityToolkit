using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PlatformEffects = Xamarin.Toolkit.Effects.Droid;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.EntryDisableAutoCorrect), nameof(RoutingEffects.EntryDisableAutoCorrect))]
namespace Xamarin.Toolkit.Effects.Droid
{
    public class EntryDisableAutoCorrect : PlatformEffect
    {
        InputTypes old;

        protected override void OnAttached()
        {
            var editText = Control as EditText;
            if (editText == null)
                return;

            old = editText.InputType;
            editText.InputType = editText.InputType | global::Android.Text.InputTypes.TextFlagNoSuggestions;
        }

        protected override void OnDetached()
        {
            var editText = Control as EditText;
            if (editText == null)
                return;

            editText.InputType = old;
        }
    }
}
