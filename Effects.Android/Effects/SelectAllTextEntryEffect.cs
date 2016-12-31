using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FormsCommunityToolkit.Effects.Droid;
using Android.Runtime;
using System.Linq;

[assembly: ExportEffect(typeof(SelectAllTextEntryEffect), nameof(SelectAllTextEntryEffect))]

namespace FormsCommunityToolkit.Effects.Droid
{
    [Preserve(AllMembers = true)]
	public class SelectAllTextEntryEffect : PlatformEffect
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