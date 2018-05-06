using Android.App;
using Android.Text;
using Android.Views;
using Xamarin.Toolkit.Droid.Helpers.Models;
using Xamarin.Toolkit.Droid.Helpers.Text;

namespace Xamarin.Toolkit.Droid.Helpers
{
    /// <summary>
    /// Extensions for Android UI.
    /// </summary>
    public static class UIExtensions
    {
        /// <summary>
        /// Sets the Padding on a View.
        /// </summary>
        /// <param name="view">View to pad</param>
        /// <param name="padding">Padding</param>
        public static void SetPadding(this View view, Thickness padding)
        {
            view.SetPadding(padding.Left, padding.Top, padding.Right, padding.Bottom);
        }

        public static void SetMargin(this View view, Thickness margin)
        {
            if (view.LayoutParameters is ViewGroup.MarginLayoutParams @params)
            {
                @params.SetMargin(margin);
            }
        }

        public static void SetMargin(this ViewGroup.MarginLayoutParams @params, Thickness margin)
        {
            @params.SetMargins(margin.Left, margin.Top, margin.Right, margin.Bottom);
        }

        /// <summary>
        /// Sets a Span on all text.
        /// </summary>
        /// <param name="str">String to set span on.</param>
        /// <param name="what">Span to set</param>
        public static void SetSpanAll(this SpannableString str, Java.Lang.Object what)
        {
            str.SetSpan(what, 0, str.Length(), SpanTypes.ExclusiveExclusive);
        }

        /// <summary>
        /// Sets a Span on all text.
        /// </summary>
        /// <param name="str">String to set span on.</param>
        /// <param name="what">Span to set</param>
        public static void SetSpanAll(this SpannableStringBuilder str, Java.Lang.Object what)
        {
            str.SetSpan(what, 0, str.Length(), SpanTypes.ExclusiveExclusive);
        }

        /// <summary>
        /// Sets the margin on all text.
        /// </summary>
        /// <param name="str">String to set span on.</param>
        /// <param name="margin">Margin to set on the text.</param>
        /// <param name="foreground">Foreground color of the text</param>
        public static void SetMarginSpanAll(this SpannableString str, Thickness margin)
        {
            str.SetMarginSpan(margin, 0, str.Length(), SpanTypes.ExclusiveExclusive);
        }

        /// <summary>
        /// Sets the margin on all text.
        /// </summary>
        /// <param name="str">String to set span on.</param>
        /// <param name="margin">Margin to set on the text.</param>
        /// <param name="foreground">Foreground color of the text</param>
        /// <param name="start">Start position to set Margin</param>
        /// <param name="end">End position of Margin</param>
        /// <param name="flags">Inclusion flags</param>
        public static void SetMarginSpan(this SpannableString str, Thickness margin, int start, int end, SpanTypes flags)
        {
            str.SetSpan(new MarginHeightSpan(margin), start, end, flags);
        }

        /// <summary>
        /// Sets the margin on all text.
        /// </summary>
        /// <param name="str">String to set span on.</param>
        /// <param name="margin">Margin to set on the text.</param>
        /// <param name="foreground">Foreground color of the text</param>
        public static void SetMarginSpanAll(this SpannableStringBuilder str, Thickness margin)
        {
            str.SetMarginSpan(margin, 0, str.Length(), SpanTypes.ExclusiveExclusive);
        }

        /// <summary>
        /// Sets the margin on all text.
        /// </summary>
        /// <param name="str">String to set span on.</param>
        /// <param name="margin">Margin to set on the text.</param>
        /// <param name="foreground">Foreground color of the text</param>
        /// <param name="start">Start position to set Margin</param>
        /// <param name="end">End position of Margin</param>
        /// <param name="flags">Inclusion flags</param>
        public static void SetMarginSpan(this SpannableStringBuilder str, Thickness margin, int start, int end, SpanTypes flags)
        {
            str.SetSpan(new MarginHeightSpan(margin), start, end, flags);
        }

        /// <summary>
        /// Converts real pixels to Display Pixels
        /// </summary>
        /// <param name="pixels">Real Pixels</param>
        /// <returns>Display pixels</returns>
        public static int GetDisplayPixels(this int pixels)
        {
            var metrics = Application.Context.Resources.DisplayMetrics;
            return (int)(pixels * metrics.Density);
        }

        /// <summary>
        /// Converts real pixels to Display Pixels
        /// </summary>
        /// <param name="pixels">Real Pixels</param>
        /// <returns>Display pixels</returns>
        public static float GetDisplayPixels(this float pixels)
        {
            var metrics = Application.Context.Resources.DisplayMetrics;
            return pixels * metrics.Density;
        }
    }
}
