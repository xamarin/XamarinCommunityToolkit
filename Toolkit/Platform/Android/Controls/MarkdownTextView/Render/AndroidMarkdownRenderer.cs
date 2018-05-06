using System.Linq;
using Android.App;
using Android.Content;
using Android.Net;
using Android.Support.Text.Emoji;
using Android.Support.Text.Emoji.Bundled;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Render;
using Xamarin.Toolkit.Droid.Helpers;
using Xamarin.Toolkit.Droid.Helpers.Text;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Render
{
    public partial class AndroidMarkdownRenderer : MarkdownRendererBase
    {
        public AndroidMarkdownRenderer(MarkdownDocument document, LinearLayout rootLayout, IImageResolver imageResolver, ILinkRegister linkRegister)
            : base(document)
        {
            androidContext = rootLayout.Context;
            this.rootLayout = rootLayout;
            this.rootLayout.SetBackgroundColor(Background);

            this.imageResolver = imageResolver;
            this.linkRegister = linkRegister;

            if (EmojiCompat == null)
            {
                var config = new BundledEmojiCompatConfig(Application.Context)
                    .SetReplaceAll(true);

                EmojiCompat = EmojiCompat.Init(config);
            }
        }

        public void Render()
        {
            Render(new AndroidRenderContext { Foreground = Foreground, Parent = rootLayout });
        }

        private TextView CreateTextView()
        {
            var textview = new TextView(androidContext)
            {
                MovementMethod = LinkMovementMethod.Instance,
            };
            textview.SetTextIsSelectable(true);

            if (FontSize != null)
            {
                textview.SetTextSize(ComplexUnitType.Dip, FontSize.Value);
            }

            return textview;
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

        private void MakeHyperlinkSpan(string url, SpannableStringBuilder span, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var foreground = LinkForeground ?? localcontext.Foreground;

            var clickspan = new EventClickableSpan
            {
                Url = url
            };

            linkRegister.RegisterNewHyperLink(clickspan);

            span.SetSpanAll(clickspan);
            if (LinkForeground != null)
            {
                span.SetSpanAll(new ForegroundColorSpan(LinkForeground.Value));
            }
        }
    }
}
