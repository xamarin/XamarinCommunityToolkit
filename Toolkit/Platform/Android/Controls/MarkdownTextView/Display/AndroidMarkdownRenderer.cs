using Android.App;
using Android.Content;
using Android.Support.Text.Emoji;
using Android.Support.Text.Emoji.Bundled;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Render;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Display
{
    internal partial class AndroidMarkdownRenderer : MarkdownRendererBase
    {
        public AndroidMarkdownRenderer(LinearLayout rootLayout, MarkdownDocument document)
            : base(document)
        {
            if (EmojiCompat == null)
            {
                try
                {
                    EmojiCompat = EmojiCompat.Get();
                }
                catch
                {
                    EmojiCompat = EmojiCompat.Init(new BundledEmojiCompatConfig(Application.Context));
                }
            }

            RootLayout = rootLayout;
        }

        public void Render()
        {
            Render(new AndroidRenderContext { Foreground = Foreground, Parent = RootLayout });
        }

        private void MakeHyperlinkSpan(string url, SpannableString span, IRenderContext context)
        {
            var context_ = context as AndroidRenderContext;
            var foreground = LinkForeground ?? context_.Foreground;

            var length = span.Length();
            span.SetSpan(new URLSpan(url), 0, length, SpanTypes.ExclusiveExclusive);
            span.SetSpan(new MarkdownClickSpan(url), 0, length, SpanTypes.ExclusiveExclusive);
            if (LinkForeground != null)
            {
                span.SetSpan(new ForegroundColorSpan(LinkForeground.Value), 0, length, SpanTypes.ExclusiveExclusive);
            }
        }

        private class MarkdownClickSpan : ClickableSpan
        {
            public MarkdownClickSpan(string url)
            {
                Url = url;
            }

            public override void OnClick(View widget)
            {
                var viewLink = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse(Url));
                var activity = widget.Context as Activity;
                activity.StartActivity(viewLink);
            }

            public string Url { get; }
        }
    }
}
