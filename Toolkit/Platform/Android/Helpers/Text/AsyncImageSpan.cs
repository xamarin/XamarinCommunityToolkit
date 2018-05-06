using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Text.Style;
using Android.Widget;
using Com.Caverock.Androidsvg;
using Java.Lang;
using Xamarin.Toolkit.Droid.Helpers.Models;

namespace Xamarin.Toolkit.Droid.Helpers.Text
{
    public class AsyncImageSpan : ImageSpan
    {
        // Default dimensions from AndroidSVG
        private const int svgDefaultSize = 512;

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

                case SVGImageSource svg:
                    SetImageSource(svg.Source);
                    break;
            }
        }

        public void SetImageSource(Bitmap bitmap)
        {
            var drawable = new BitmapDrawable(bitmap);
            var width = bitmap.Width;
            var height = bitmap.Height;

            if (Width.HasValue)
            {
                width = Width.Value;
            }

            if (Height.HasValue)
            {
                height = Height.Value;
            }

            if (MaxWidth.HasValue && width > MaxWidth)
            {
                width = MaxWidth.Value;
            }

            if (MaxHeight.HasValue && height > MaxHeight)
            {
                height = MaxHeight.Value;
            }

            drawable.SetBounds(0, 0, width, height);
            SetImageSource(drawable);
        }

        public void SetImageSource(SVG svg)
        {
            svg.DocumentPreserveAspectRatio = PreserveAspectRatio.Letterbox;

            // Default dimensions from AndroidSVG
            var height = svgDefaultSize;
            var width = svgDefaultSize;

            // Create a canvas to draw onto
            if (svg.DocumentWidth != -1)
            {
                width = (int)Math.Ceil(svg.DocumentWidth);
                height = (int)Math.Ceil(svg.DocumentHeight);
            }
            else
            {
                svg.SetDocumentHeight("100%");
                svg.SetDocumentWidth("100%");
            }

            var newBM = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            var bmcanvas = new Canvas(newBM);

            // Set Background.
            bmcanvas.DrawColor(BackgroundColor);

            // Render our document onto our canvas
            svg.RenderToCanvas(bmcanvas);

            SetImageSource(newBM);
        }

        public void SetImageSource(Drawable drawable)
        {
            var bounds = drawable.Bounds;

            var width = drawable.IntrinsicWidth;
            var height = drawable.IntrinsicHeight;

            // Hacky way to force full dimensions.
            if (drawable is BitmapDrawable bitmap)
            {
                width = bitmap.Bitmap.Width;
                height = bitmap.Bitmap.Height;
            }

            if (Width.HasValue)
            {
                width = Width.Value;
            }

            if (Height.HasValue)
            {
                height = Height.Value;
            }

            if (MaxWidth.HasValue && width > MaxWidth)
            {
                width = MaxWidth.Value;
            }

            if (MaxHeight.HasValue && height > MaxHeight)
            {
                height = MaxHeight.Value;
            }

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

        public Color BackgroundColor { get; set; } = Color.White;

        public int? MaxWidth { get; set; }

        public int? MaxHeight { get; set; }

        public int? Height { get; set; }

        public int? Width { get; set; }

        private LevelListDrawable backing;

        private TextView textview;

        private bool textviewLoaded;

        private bool usePlaceholder;
    }
}
