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
    class ShrinkTextOnLayoutChangeListener : Java.Lang.Object, global::Android.Views.View.IOnLayoutChangeListener
    {
        const string textMeasure = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        const float threshold = 0.5f; // How close we have to be

        readonly TextView textView;

        public ShrinkTextOnLayoutChangeListener(TextView textView)
            : base() => this.textView = textView;

        public void OnLayoutChange(global::Android.Views.View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
        {
            if (textView.Width <= 0 || textView.Height <= 0)
                return;

            var hi = ConvertSpToPixels(textView.TextSize, textView.Context);
            var lo = 1f;

            var paint = new Paint(textView.Paint);
            var bounds = new Rect();

            while ((hi - lo) > threshold)
            {
                var size = (hi + lo) / 2;
                paint.TextSize = size;
                paint.GetTextBounds(textMeasure, 0, textMeasure.Length, bounds);

                if (paint.MeasureText(textView.Text) >= textView.Width || bounds.Height() >= textView.Height)
                    hi = size; // too big
                else
                    lo = size; // too small
            }

            textView.SetTextSize(ComplexUnitType.Px, lo);
        }

        static float ConvertSpToPixels(float sp, Context context) => TypedValue.ApplyDimension(ComplexUnitType.Px, sp, context.Resources.DisplayMetrics);
    }

    [Preserve(AllMembers = true)]
    public class LabelSizeFontToFit : PlatformEffect
    {
        ShrinkTextOnLayoutChangeListener listener;

        protected override void OnAttached()
        {
            var textView = Control as TextView;
            if (textView == null)
                return;

            textView.AddOnLayoutChangeListener(listener = new ShrinkTextOnLayoutChangeListener(textView));
        }

        protected override void OnDetached()
        {
            var textView = Control as TextView;
            if (textView == null)
                return;

            textView.RemoveOnLayoutChangeListener(listener);
        }
    }
}
