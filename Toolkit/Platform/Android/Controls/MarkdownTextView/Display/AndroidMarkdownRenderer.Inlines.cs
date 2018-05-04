using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;
using Microsoft.Toolkit.Parsers.Markdown.Render;
using Xamarin.Toolkit.Droid.Helpers;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Display
{
    public partial class AndroidMarkdownRenderer
    {
        protected override void RenderBoldRun(BoldTextInline element, IRenderContext context)
        {
            var context_ = context as AndroidRenderContext;
            var builder = context_.Builder;

            // Render the children into the bold inline.
            var inlines = new SpannableStringBuilder();

            RenderInlineChildren(element.Inlines, context);
            var spannedText = SpannableStringBuilder.ValueOf(inlines);

            // Set the Span to bold style.
            spannedText.SetSpanAll(new StyleSpan(TypefaceStyle.Bold));

            // Add the internal bold spanned inlines to the current inlines.
            builder.Append(spannedText);
        }

        protected override void RenderCodeRun(CodeInline element, IRenderContext context)
        {
            var context_ = context as AndroidRenderContext;
            var builder = context_.Builder;

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
            var context_ = context as AndroidRenderContext;
            var builder = context_.Builder;
            var span = EmojiCompat?.Process(element.Text);

            if (span != null)
            {
                // Add it to the current inlines
                builder.Append(span);
            }
        }

        protected override void RenderImage(ImageInline element, IRenderContext context)
        {
            // TODO: Implement
        }

        protected override void RenderItalicRun(ItalicTextInline element, IRenderContext context)
        {
            var context_ = context as AndroidRenderContext;
            var builder = context_.Builder;

            // Render the children into the bold inline.
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
            var context_ = context as AndroidRenderContext;
            var builder = context_.Builder;

            var subbuilder = new SpannableStringBuilder();
            var subcontext = context.Clone() as AndroidRenderContext;
            subcontext.Builder = subbuilder;

            RenderInlineChildren(element.Inlines, subcontext);

            var span = SpannableString.ValueOf(subbuilder);
            MakeHyperlinkSpan(element.Url, span, context);

            // Add it to the current inlines
            builder.Append(span);
        }

        protected override void RenderHyperlink(HyperlinkInline element, IRenderContext context)
        {
            var context_ = context as AndroidRenderContext;
            var builder = context_.Builder;

            var span = new SpannableString(CollapseWhitespace(context, element.Text));
            MakeHyperlinkSpan(element.Url, span, context);

            // Add it to the current inlines
            builder.Append(span);
        }

        protected override void RenderStrikethroughRun(StrikethroughTextInline element, IRenderContext context)
        {
            var context_ = context as AndroidRenderContext;
            var builder = context_.Builder;

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
            var context_ = context as AndroidRenderContext;
            var builder = context_.Builder;

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
            var context_ = context as AndroidRenderContext;
            var builder = context_.Builder;

            // Creates raw text.
            var text = new SpannableString(CollapseWhitespace(context, element.Text));
            text.SetSpanAll(new ForegroundColorSpan(Foreground));

            // Add it to the current inlines
            builder.Append(text);
        }
    }
}
