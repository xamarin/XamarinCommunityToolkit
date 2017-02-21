using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FormsCommunityToolkit.Effects.Droid;
using Android.Runtime;

[assembly: ExportEffect(typeof(EntrySelectAllText), nameof(EntrySelectAllText))]

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