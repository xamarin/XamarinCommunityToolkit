using Android.Runtime;
using FormsCommunityToolkit.Effects.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using Picker = Android.Widget.EditText;

[assembly: ExportEffect(typeof(ChangeColorPickerEffect), nameof(ChangeColorPickerEffect))]
namespace FormsCommunityToolkit.Effects.Droid
{

    [Preserve (AllMembers = true)]
    public class ChangeColorPickerEffect : PlatformEffect
    {
        private Color _color;

        protected override void OnAttached ()
        {
            
            _color = (Color)Element.GetValue(ChangePickerColorEffect.ColorProperty);
            ((Picker)Control).SetHintTextColor(_color.ToAndroid());

        }

        protected override void OnDetached ()
        {
        }
    }
}