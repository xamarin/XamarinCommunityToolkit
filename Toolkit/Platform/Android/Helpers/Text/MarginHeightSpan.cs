using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Xamarin.Toolkit.Droid.Helpers.Models;

namespace Xamarin.Toolkit.Droid.Helpers.Text
{
    internal class MarginHeightSpan : Java.Lang.Object, ILineHeightSpanWithDensity
    {
        public MarginHeightSpan(Thickness margin)
        {
            Margin = margin;
        }

        public void ChooseHeight(Java.Lang.ICharSequence text, int start, int end, int spanstartv, int v, Paint.FontMetricsInt fm)
        {
            // Should not get called, at least not by StaticLayout.
            ChooseHeight(text, start, end, spanstartv, v, fm, null);
        }

        public void ChooseHeight(Java.Lang.ICharSequence text, int start, int end, int spanstartv, int v, Paint.FontMetricsInt fm, TextPaint paint)
        {
            fm.Top -= Margin.Top;
            fm.Bottom += Margin.Bottom;
        }

        private Thickness Margin { get; }
    }
}
