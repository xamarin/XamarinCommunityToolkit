using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PlatformEffects = Xamarin.Toolkit.Effects.Droid;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.EntryClear), nameof(RoutingEffects.EntryClear))]
namespace Xamarin.Toolkit.Effects.Droid
{
    internal class OnDrawableTouchListener : Java.Lang.Object, global::Android.Views.View.IOnTouchListener
    {
        public bool OnTouch(global::Android.Views.View v, MotionEvent e)
        {
            if (v is EditText && e.Action == MotionEventActions.Up)
            {
                var editText = (EditText)v;
                if (e.RawX >= (editText.Right - editText.GetCompoundDrawables()[2].Bounds.Width()))
                {
                    editText.Text = string.Empty;
                    return true;
                }
            }

            return false;
        }
    }
}