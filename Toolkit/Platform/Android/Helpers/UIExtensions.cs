using Android.Text;
using Android.Views;
using Xamarin.Toolkit.Droid.Helpers.Models;

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

        /// <summary>
        /// Sets a Span on all text.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="what"></param>
        public static void SetSpanAll(this SpannableString str, Java.Lang.Object what)
        {
            str.SetSpan(what, 0, str.Length(), SpanTypes.ExclusiveExclusive);
        }

        /// <summary>
        /// Sets a Span on all text.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="what"></param>
        public static void SetSpanAll(this SpannableStringBuilder str, Java.Lang.Object what)
        {
            str.SetSpan(what, 0, str.Length(), SpanTypes.ExclusiveExclusive);
        }
    }
}
