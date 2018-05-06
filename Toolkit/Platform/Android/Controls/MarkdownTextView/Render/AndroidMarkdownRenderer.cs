using System.Linq;
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
using Java.Lang;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Render;
using Xamarin.Toolkit.Droid.Helpers.Text;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Render
{
    public partial class AndroidMarkdownRenderer : MarkdownRendererBase
    {
        public AndroidMarkdownRenderer(MarkdownDocument document, LinearLayout rootLayout, IImageResolver imageResolver)
            : base(document)
        {
            RootLayout = rootLayout;
            rootLayout.SetBackgroundColor(Background);
            this.imageResolver = imageResolver;

            if (EmojiCompat == null)
            {
                var config = new BundledEmojiCompatConfig(Application.Context)
                    .SetReplaceAll(true);

                EmojiCompat = EmojiCompat.Init(config);
            }
        }

        public void Render()
        {
            Render(new AndroidRenderContext { Foreground = Foreground, Parent = RootLayout });
        }

        private void SetText(TextView textview, ICharSequence str)
        {
            var type = Class.FromType(typeof(AsyncImageSpan));
            var len = str.Length();

            var asyncSpans = str is SpannableString spstr
                ? spstr.GetSpans(0, len, type)
                : str is SpannableStringBuilder strbldr
                ? strbldr.GetSpans(0, len, type)
                : null;

            if (asyncSpans?.Any() == true)
            {
                foreach (AsyncImageSpan span in asyncSpans)
                {
                    span.Attach(textview);
                }
            }

            textview.SetText(str, TextView.BufferType.Spannable);
        }

        private void MakeHyperlinkSpan(string url, SpannableString span, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var foreground = LinkForeground ?? localcontext.Foreground;

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
