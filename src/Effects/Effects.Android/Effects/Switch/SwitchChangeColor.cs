using System;
using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Widget;
using FormsCommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using Switch = Android.Widget.Switch;
using SwitchChangeColor = FormsCommunityToolkit.Effects.Droid.SwitchChangeColor;

[assembly: ExportEffect(typeof(SwitchChangeColor), nameof(SwitchChangeColorEffect))]
namespace FormsCommunityToolkit.Effects.Droid
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
                _trueColor = (Color)Element.GetValue(FormsCommunityToolkit.Effects.SwitchChangeColor.TrueColorProperty);
                _falseColor = (Color)Element.GetValue(FormsCommunityToolkit.Effects.SwitchChangeColor.FalseColorProperty);

                ((SwitchCompat)Control).CheckedChange += OnCheckedChange;

                //Supported formats for Parse are: #RRGGBB #AARRGGBB 'red', 'blue', 'green', 'black', 'white', 'gray', 'cyan', 'magenta', 'yellow', 'lightgray', 'darkgray' 
                ((SwitchCompat)Control).ThumbDrawable.SetColorFilter(_falseColor.ToAndroid(), PorterDuff.Mode.Multiply);
            }

            //TODO: Glenn - From lollipop mr1 you can also use the TintList instead of working with events... not sure what the best approach is
            //if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.LollipopMr1)
            //{
            //    ColorStateList buttonStates = new ColorStateList(new int[][]
            //        {
            //            new int[] {Android.Resource.Attribute.StateChecked},
            //            new int[] {-Android.Resource.Attribute.StateEnabled},
            //            new int[] {}
            //        },
            //        new int[]
            //        {
            //            Android.Graphics.Color.Red,
            //            Android.Graphics.Color.Blue,
            //            Android.Graphics.Color.Green
            //        }
            //    );

            //    ((SwitchCompat)Control).ThumbDrawable.SetTintList(buttonStates);
            //}
        }

        private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs checkedChangeEventArgs)
        {
            if (checkedChangeEventArgs.IsChecked)
            {
                ((SwitchCompat)Control).ThumbDrawable.SetColorFilter(_trueColor.ToAndroid(), PorterDuff.Mode.Multiply);
                //((SwitchCompat) Control).TrackDrawable.SetColorFilter(Android.Graphics.Color.Green, PorterDuff.Mode.Multiply);
            }
            else
            {
                ((SwitchCompat)Control).ThumbDrawable.SetColorFilter(_falseColor.ToAndroid(), PorterDuff.Mode.Multiply);
                //((SwitchCompat)Control).TrackDrawable.SetColorFilter(Android.Graphics.Color.Green, PorterDuff.Mode.Multiply);
            }
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