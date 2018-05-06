using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;
using Microsoft.Toolkit.Parsers.Markdown.Render;
using Xamarin.Toolkit.Droid.Helpers;
using Xamarin.Toolkit.Droid.Helpers.Text;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Render
{
    public partial class AndroidMarkdownRenderer
    {
        protected override void RenderBoldRun(BoldTextInline element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var builder = localcontext.Builder;

            // Set the Span to bold style.
            var subbuilder = new SpannableStringBuilder();
            var subcontext = context.Clone() as AndroidRenderContext;
            subcontext.Builder = subbuilder;

            RenderInlineChildren(element.Inlines, subcontext);

            // Set the Span to bold style.
            subbuilder.SetSpanAll(new StyleSpan(TypefaceStyle.Bold));

            // Add the internal bold spanned inlines to the current inlines.
            builder.Append(subbuilder);
        }

        protected override void RenderCodeRun(CodeInline element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var builder = localcontext.Builder;

            var span = new SpannableString(element.Text);
            var length = span.Length();

            if (InlineCodeBackground != null)
            {
                span.SetSpanAll(new BackgroundColorSpan(InlineCodeBackground.Value));
            }

            // Add it to the current inlines
            builder.Append(span);
        }

        protected override void RenderEmoji(EmojiInline element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var builder = localcontext.Builder;
            var span = EmojiCompat?.Process(element.Text);

            if (span != null)
            {
                // Add it to the current inlines
                builder.Append(span);
            }
        }

        protected override void RenderImage(ImageInline element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var builder = localcontext.Builder;
            var parent = localcontext.Parent as ViewGroup;

            var imagespan = new AsyncImageSpan();
            imagespan.ClickHandler.Url = element.Url;
            linkRegister.RegisterNewHyperLink(imagespan.ClickHandler, true);

            var text = new SpannableString(element.Text);
            text.SetSpanAll(new ForegroundColorSpan(Foreground));
            text.SetSpanAll(imagespan);
            builder.Append(text);

            // Image view container
            Task.Run(() => imageResolver.ResolveImageAsync(element.Url, element.Tooltip)).ContinueWith(image =>
            {
                ((Activity)androidContext).RunOnUiThread(() =>
                {
                    if (image.Result != null)
                    {
                        imagespan.SetImageSource(image.Result);
                    }
                    else
                    {
                        imagespan.SetPlaceholder();
                    }
                });
            });
        }

        protected override void RenderItalicRun(ItalicTextInline element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var builder = localcontext.Builder;

            // Render the children into the italic inline.
            var subbuilder = new SpannableStringBuilder();
            var subcontext = context.Clone() as AndroidRenderContext;
            subcontext.Builder = subbuilder;

            RenderInlineChildren(element.Inlines, subcontext);

            // Set the Span to bold style.
            subbuilder.SetSpanAll(new StyleSpan(TypefaceStyle.Italic));

            // Add the internal bold spanned inlines to the current inlines.
            builder.Append(subbuilder);
        }

        protected override void RenderMarkdownLink(MarkdownLinkInline element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var builder = localcontext.Builder;

            var subbuilder = new SpannableStringBuilder();
            var subcontext = context.Clone() as AndroidRenderContext;
            subcontext.Builder = subbuilder;

            RenderInlineChildren(element.Inlines, subcontext);

            MakeHyperlinkSpan(element.Url, subbuilder, context);

            // Add it to the current inlines
            builder.Append(subbuilder);
        }

        protected override void RenderHyperlink(HyperlinkInline element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var builder = localcontext.Builder;

            var subbuilder = new SpannableStringBuilder(CollapseWhitespace(context, element.Text));
            MakeHyperlinkSpan(element.Url, subbuilder, context);

            // Add it to the current inlines
            builder.Append(subbuilder);
        }

        protected override void RenderStrikethroughRun(StrikethroughTextInline element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var builder = localcontext.Builder;

            // Render the children into the bold inline.
            var subbuilder = new SpannableStringBuilder();
            var subcontext = context.Clone() as AndroidRenderContext;
            subcontext.Builder = subbuilder;

            RenderInlineChildren(element.Inlines, subcontext);
            var spannedText = SpannableStringBuilder.ValueOf(subbuilder);

            // Set the Span to bold style.
            spannedText.SetSpanAll(new StrikethroughSpan());

            // Add the internal bold spanned inlines to the current inlines.
            builder.Append(spannedText);
        }

        protected override void RenderSuperscriptRun(SuperscriptTextInline element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var builder = localcontext.Builder;

            // Render the children into the bold inline.
            var subbuilder = new SpannableStringBuilder();
            var subcontext = context.Clone() as AndroidRenderContext;
            subcontext.Builder = subbuilder;

            RenderInlineChildren(element.Inlines, subcontext);
            var spannedText = SpannableStringBuilder.ValueOf(subbuilder);

            // Set the Span to bold style.
            spannedText.SetSpanAll(new SuperscriptSpan());

            // Add the internal bold spanned inlines to the current inlines.
            builder.Append(spannedText);
        }

        protected override void RenderTextRun(TextRunInline element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var builder = localcontext.Builder;

            // Creates raw text.
            var text = new SpannableString(element.Text);
            text.SetSpanAll(new ForegroundColorSpan(Foreground));

            // Add it to the current inlines
            builder.Append(text);
        }
    }
}
