using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Runtime;
using RoutingEffects = FormsCommunityToolkit.Effects;
using PlatformEffects = FormsCommunityToolkit.Effects.Droid;

[assembly: ExportEffect(typeof(PlatformEffects.EntrySelectAllText), nameof(RoutingEffects.EntrySelectAllText))]

namespace FormsCommunityToolkit.Effects.Droid
{
    [Preserve(AllMembers = true)]
    public class EntrySelectAllText : PlatformEffect
    {
 
        protected override void OnAttached()
        {
            var editText = Control as EditText;
            if (editText == null)
                return;

            editText.SetSelectAllOnFocus(true);
        }

        protected override void OnDetached()
        {
            var editText = Control as EditText;
            if (editText == null)
                return;

            editText.SetSelectAllOnFocus(false);
        }
    }
}