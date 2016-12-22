using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using FormsCommunityToolkit.Effects.Droid;
using Android.Runtime;
using Android.Widget;
using Android.Graphics;
using Android.Util;
using Android.Content;

[assembly: ExportEffect(typeof(SizeFontToFitEffect), nameof(SizeFontToFitEffect))]

namespace FormsCommunityToolkit.Effects.Droid
{
    class ShrinkTextOnLayoutChangeListener : Java.Lang.Object, Android.Views.View.IOnLayoutChangeListener
    {
        const string TextMeasure = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        const float Threshold = 0.5f; // How close we have to be

        readonly TextView textView;

        public ShrinkTextOnLayoutChangeListener(TextView textView) : base()
        {
            this.textView = textView;
        }

        public void OnLayoutChange(Android.Views.View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight, int oldBottom)
        {
            if (textView.Width <= 0 || textView.Height <= 0) return;
            
            var hi = ConvertSpToPixels(textView.TextSize, textView.Context);
            var lo = 1f;

            var paint = new Paint(textView.Paint);
            var bounds = new Rect();

            while ((hi - lo) > Threshold)
            {
                float size = (hi + lo) / 2;
                paint.TextSize = size;
                paint.GetTextBounds(TextMeasure, 0, TextMeasure.Length, bounds);

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
    public class SizeFontToFitEffect : PlatformEffect
    {
        ShrinkTextOnLayoutChangeListener _listener;

        protected override void OnAttached()
        {
            var textView = Control as TextView;
            if (textView != null)
                textView.AddOnLayoutChangeListener(_listener = new ShrinkTextOnLayoutChangeListener(textView));
        }

        protected override void OnDetached()
        {
            var textView = Control as TextView;
            if (textView != null)
                textView.RemoveOnLayoutChangeListener(_listener);
        }
    }
}
