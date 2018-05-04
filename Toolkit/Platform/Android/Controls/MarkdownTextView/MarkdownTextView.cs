using Android.Content;
using Android.Util;
using Android.Widget;

namespace Xamarin.Toolkit.Droid.Controls
{
    public partial class MarkdownTextView : LinearLayout
    {
        public MarkdownTextView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Orientation = Orientation.Vertical;
            SetBackgroundColor(global::Android.Graphics.Color.White);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            RenderMarkdown();
        }
    }
}
