using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PlatformEffects = Xamarin.Toolkit.Effects.Droid;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.LabelSizeFontToFit), nameof(RoutingEffects.LabelSizeFontToFit))]
namespace Xamarin.Toolkit.Effects.Droid
{
    [Preserve(AllMembers = true)]
    public class LabelSizeFontToFit : PlatformEffect
    {
        private ShrinkTextOnLayoutChangeListener _listener;

        protected override void OnAttached()
        {
            var textView = Control as TextView;
            if (textView == null)
            {
                return;
            }

            textView.AddOnLayoutChangeListener(_listener = new ShrinkTextOnLayoutChangeListener(textView));
        }

        protected override void OnDetached()
        {
            var textView = Control as TextView;
            if (textView == null)
            {
                return;
            }

            textView.RemoveOnLayoutChangeListener(_listener);
        }
    }
}
