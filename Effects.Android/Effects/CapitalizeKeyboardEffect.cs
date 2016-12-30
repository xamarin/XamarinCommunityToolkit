using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FormsCommunityToolkit.Effects.Droid;
using Android.Runtime;
using System.Linq;

[assembly: ExportEffect(typeof(CapitalizeKeyboardEffect), nameof(CapitalizeKeyboardEffect))]

namespace FormsCommunityToolkit.Effects.Droid
{
    [Preserve(AllMembers = true)]
    public class CapitalizeKeyboardEffect : PlatformEffect
    {
        InputTypes old;
        IInputFilter[] oldFilters;

        protected override void OnAttached()
        {
            var editText = Control as EditText;
			if (editText == null)
				return;
			else
            {
                old = editText.InputType;
                oldFilters = editText.GetFilters().ToArray();

                editText.SetRawInputType(InputTypes.ClassText | InputTypes.TextFlagCapCharacters);

                var newFilters = oldFilters.ToList();
                newFilters.Add(new InputFilterAllCaps());
                editText.SetFilters(newFilters.ToArray());
            }
        }

        protected override void OnDetached()
        {
            var editText = Control as EditText;
			if (editText == null)
				return;
			else
            {
                editText.SetRawInputType(old);
                editText.SetFilters(oldFilters);
            }
        }
    }
}