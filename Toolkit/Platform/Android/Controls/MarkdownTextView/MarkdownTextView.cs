using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using Xamarin.Toolkit.Droid.Controls.Markdown.Display;

namespace Xamarin.Toolkit.Droid.Controls
{
    public partial class MarkdownTextView : LinearLayout, IImageResolver
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
