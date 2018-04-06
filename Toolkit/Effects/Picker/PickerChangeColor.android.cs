using Android.Runtime;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using Picker = Android.Widget.EditText;
using PlatformEffects = Xamarin.Toolkit.Effects.Droid;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.PickerChangeColor), nameof(RoutingEffects.PickerChangeColorEffect))]
namespace Xamarin.Toolkit.Effects.Droid
{
    [Preserve(AllMembers = true)]
    public class PickerChangeColor : PlatformEffect
    {
        Color color;

        protected override void OnAttached()
        {
            color = (Color)Element.GetValue(RoutingEffects.PickerChangeColor.ColorProperty);
            ((Picker)Control).SetHintTextColor(color.ToAndroid());
        }

        protected override void OnDetached()
        {
        }
    }
}
