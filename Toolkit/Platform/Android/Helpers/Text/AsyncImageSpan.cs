using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Text.Style;
using Android.Widget;
using Xamarin.Toolkit.Droid.Helpers.Models;

namespace Xamarin.Toolkit.Droid.Helpers.Text
{
    public class AsyncImageSpan : ImageSpan
    {
        public AsyncImageSpan()
            : this(new LevelListDrawable())
        {
        }

        private AsyncImageSpan(LevelListDrawable backing)
            : base(backing)
        {
            this.backing = backing;
        }

        public void Attach(TextView textview)
        {
            this.textview = textview;
            textview.ViewAttachedToWindow += Textview_ViewAttachedToWindow;
        }

        public void SetImageSource(ImageSource source)
        {
            switch (source)
            {
                case BitmapImageSource bitmap:
                    SetImageSource(bitmap.Source);
                    break;
            }
        }

        public void SetImageSource(Bitmap bitmap)
        {
            SetImageSource(new BitmapDrawable(bitmap));
        }

        public void SetImageSource(Drawable drawable)
        {
            var width = drawable.IntrinsicWidth;
            var height = drawable.IntrinsicHeight;

            backing.AddLevel(0, 0, drawable);
            backing.SetLevel(0);

            backing.SetBounds(0, 0, width > 0 ? width.GetDisplayPixels() : 0, height > 0 ? height.GetDisplayPixels() : 0);

            // Update the TextView with new image data.
            if (textview != null && textviewLoaded)
            {
                if (textview.TextFormatted is ISpannable text)
                {
                    var start = text.GetSpanStart(this);
                    var end = text.GetSpanEnd(this);

                    text.SetSpan(ClickHandler, start, end, SpanTypes.ExclusiveExclusive);
                    textview.SetText(text, TextView.BufferType.Spannable);
                }
            }
        }

        public void SetPlaceholder()
        {
            usePlaceholder = true;
            if (textview != null && textviewLoaded)
            {
                RemoveSpan();
            }
        }

        private void Textview_ViewAttachedToWindow(object sender, global::Android.Views.View.ViewAttachedToWindowEventArgs e)
        {
            textviewLoaded = true;
            if (usePlaceholder)
            {
                RemoveSpan();
            }
        }

        private void RemoveSpan()
        {
            var text = textview.TextFormatted;
            if (text is SpannableString str)
            {
                str.RemoveSpan(this);
                textview.SetText(str, TextView.BufferType.Spannable);
            }
        }

        public EventClickableSpan ClickHandler { get; } = new EventClickableSpan();

        private LevelListDrawable backing;

        private TextView textview;

        private bool textviewLoaded;

        private bool usePlaceholder;
    }
}
