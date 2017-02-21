using Android.Runtime;
using FormsCommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using Picker = Android.Widget.EditText;
using PickerChangeColor = FormsCommunityToolkit.Effects.Droid.PickerChangeColor;

[assembly: ExportEffect(typeof(PickerChangeColor), nameof(PickerChangeColorEffect))]
namespace FormsCommunityToolkit.Effects.Droid
{

    [Preserve (AllMembers = true)]
    public class PickerChangeColor : PlatformEffect
    {
        private Color _color;

        protected override void OnAttached ()
        {
            _color = (Color)Element.GetValue(FormsCommunityToolkit.Effects.PickerChangeColor.ColorProperty);
            ((Picker)Control).SetHintTextColor(_color.ToAndroid());
        }

        protected override void OnDetached ()
        {
        }
    }
}