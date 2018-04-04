using System.Linq;
using Android.Runtime;
using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PlatformEffects = Xamarin.Toolkit.Effects.Droid;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.EntryCapitalizeKeyboard), nameof(RoutingEffects.EntryCapitalizeKeyboard))]
namespace Xamarin.Toolkit.Effects.Droid
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
            {
                return;
            }

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
            {
                return;
            }

            editText.SetRawInputType(_old);
            editText.SetFilters(_oldFilters);
        }
    }
}