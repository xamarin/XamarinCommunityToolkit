using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Net;
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
    public partial class AndroidMarkdownRenderer : MarkdownRendererBase
    {
        public AndroidMarkdownRenderer(LinearLayout rootLayout, MarkdownDocument document, IImageResolver imageResolver)
            : base(document)
        {
            RootLayout = rootLayout;
            rootLayout.SetBackgroundColor(Background);
            this.imageResolver = imageResolver;

            if (EmojiCompat == null)
            {
                var config = new BundledEmojiCompatConfig(Application.Context)
                    .SetEmojiSpanIndicatorEnabled(true)
                    .SetEmojiSpanIndicatorColor(Color.Green)
                    .SetReplaceAll(true);

                EmojiCompat = EmojiCompat.Init(config);
            }
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
                var viewLink = new Intent(Intent.ActionView, Uri.Parse(Url));
                var activity = widget.Context as Activity;
                activity.StartActivity(viewLink);
            }

            public string Url { get; }
        }
    }
}
