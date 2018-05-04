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
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            RenderMarkdown();
        }
    }
}
