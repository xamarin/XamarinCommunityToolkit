using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using PlatformEffects = Xamarin.Toolkit.Effects.Droid;
using RoutingEffects = Xamarin.Toolkit.Effects;
using Switch = Android.Widget.Switch;

[assembly: ExportEffect(typeof(PlatformEffects.SwitchChangeColor), nameof(RoutingEffects.SwitchChangeColorEffect))]
namespace Xamarin.Toolkit.Effects.Droid
{
    /// <summary>
    /// http://stackoverflow.com/questions/11253512/change-on-color-of-a-switch
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SwitchChangeColor : PlatformEffect
    {
        Color trueColor;
        Color falseColor;

        protected override void OnAttached()
        {
            if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.JellyBean)
            {
                trueColor = (Color)Element.GetValue(RoutingEffects.SwitchChangeColor.TrueColorProperty);
                falseColor = (Color)Element.GetValue(RoutingEffects.SwitchChangeColor.FalseColorProperty);

                ((SwitchCompat)Control).CheckedChange += OnCheckedChange;

                // Supported formats for Parse are: #RRGGBB #AARRGGBB 'red', 'blue', 'green', 'black', 'white', 'gray', 'cyan', 'magenta', 'yellow', 'lightgray', 'darkgray'
                ((SwitchCompat)Control).ThumbDrawable.SetColorFilter(falseColor.ToAndroid(), PorterDuff.Mode.Multiply);
            }
        }

        void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs checkedChangeEventArgs)
        {
            if (checkedChangeEventArgs.IsChecked)
            {
                ((SwitchCompat)Control).ThumbDrawable.SetColorFilter(trueColor.ToAndroid(), PorterDuff.Mode.Multiply);
            }
            else
            {
                ((SwitchCompat)Control).ThumbDrawable.SetColorFilter(falseColor.ToAndroid(), PorterDuff.Mode.Multiply);
            }

            ((Forms.Switch)Element).IsToggled = checkedChangeEventArgs.IsChecked;
        }

        protected override void OnDetached()
        {
            if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.JellyBean && global::Android.OS.Build.VERSION.SdkInt < global::Android.OS.BuildVersionCodes.LollipopMr1)
            {
                ((Switch)Control).CheckedChange -= OnCheckedChange;
            }
        }
    }
}
