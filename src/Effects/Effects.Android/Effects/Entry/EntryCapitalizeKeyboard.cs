using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FormsCommunityToolkit.Effects.Droid;
using Android.Runtime;
using System.Linq;

[assembly: ExportEffect(typeof(EntryCapitalizeKeyboard), nameof(EntryCapitalizeKeyboard))]
namespace FormsCommunityToolkit.Effects.Droid
{
    [Preserve(AllMembers = true)]
    public class EntryCapitalizeKeyboard : PlatformEffect
    {
        private InputTypes _old;
        private IInputFilter[] _oldFilters;

        protected override void OnAttached()
        {
            var editText = Control as EditText;
            if (editText == null)
                return;
            
            _old = editText.InputType;
            _oldFilters = editText.GetFilters().ToArray();

            editText.SetRawInputType(InputTypes.ClassText | InputTypes.TextFlagCapCharacters);

            var newFilters = _oldFilters.ToList();
            newFilters.Add(new InputFilterAllCaps());
            editText.SetFilters(newFilters.ToArray());
        }

        protected override void OnDetached()
        {
            var editText = Control as EditText;
            if (editText == null)
                return;

            editText.SetRawInputType(_old);
            editText.SetFilters(_oldFilters);
        }
    }
}