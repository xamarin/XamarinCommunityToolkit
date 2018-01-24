using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using Switch = Android.Widget.Switch;
using RoutingEffects = XamarinCommunityToolkit.Effects;
using PlatformEffects = XamarinCommunityToolkit.Effects.Droid;

[assembly: ExportEffect(typeof(PlatformEffects.SwitchChangeColor), nameof(RoutingEffects.SwitchChangeColorEffect))]
namespace XamarinCommunityToolkit.Effects.Droid
{
    /// <summary>
    /// http://stackoverflow.com/questions/11253512/change-on-color-of-a-switch
    /// </summary>
    [Preserve(AllMembers = true)]
    public class SwitchChangeColor : PlatformEffect
    {
        private Color _trueColor;
        private Color _falseColor;

        protected override void OnAttached()
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {
                _trueColor = (Color)Element.GetValue(RoutingEffects.SwitchChangeColor.TrueColorProperty);
                _falseColor = (Color)Element.GetValue(RoutingEffects.SwitchChangeColor.FalseColorProperty);

                ((SwitchCompat)Control).CheckedChange += OnCheckedChange;

                //Supported formats for Parse are: #RRGGBB #AARRGGBB 'red', 'blue', 'green', 'black', 'white', 'gray', 'cyan', 'magenta', 'yellow', 'lightgray', 'darkgray' 
                ((SwitchCompat)Control).ThumbDrawable.SetColorFilter(_falseColor.ToAndroid(), PorterDuff.Mode.Multiply);
            }
        }

        private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs checkedChangeEventArgs)
        {
            if (checkedChangeEventArgs.IsChecked)
            {
                ((SwitchCompat)Control).ThumbDrawable.SetColorFilter(_trueColor.ToAndroid(), PorterDuff.Mode.Multiply);
            }
            else
            {
                ((SwitchCompat)Control).ThumbDrawable.SetColorFilter(_falseColor.ToAndroid(), PorterDuff.Mode.Multiply);
            }

            ((Xamarin.Forms.Switch)Element).IsToggled = checkedChangeEventArgs.IsChecked;
        }

        protected override void OnDetached()
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean && Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.LollipopMr1)
            {
                ((Switch)Control).CheckedChange -= OnCheckedChange;
            }
        }
    }
}