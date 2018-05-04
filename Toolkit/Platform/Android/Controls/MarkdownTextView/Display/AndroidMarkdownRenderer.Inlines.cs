// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Microsoft.Toolkit.Parsers.Markdown.Display;
using Microsoft.Toolkit.Parsers.Markdown.Parse;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Display
{
    internal partial class AndroidMarkdownRenderer
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
            spannedText.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, spannedText.Length(), SpanTypes.ExclusiveExclusive);

            // Add the internal bold spanned inlines to the current inlines.
            builder.Append(spannedText);
        }

        protected override void RenderCodeRun(CodeInline element, IRenderContext context)
        {
            var context_ = context as AndroidRenderContext;
            var builder = context_.Builder;

            var span = new SpannableString(element.Text);
            var length = span.Length();

            if (InlineCodeBackground.HasValue)
            {
                span.SetSpan(new BackgroundColorSpan(InlineCodeBackground.Value), 0, length, SpanTypes.ExclusiveExclusive);
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
            subbuilder.SetSpan(new StyleSpan(TypefaceStyle.Italic), 0, subbuilder.Length(), SpanTypes.ExclusiveExclusive);

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
            spannedText.SetSpan(new StrikethroughSpan(), 0, spannedText.Length(), SpanTypes.ExclusiveExclusive);

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
            spannedText.SetSpan(new SuperscriptSpan(), 0, spannedText.Length(), SpanTypes.ExclusiveExclusive);

            // Add the internal bold spanned inlines to the current inlines.
            builder.Append(spannedText);
        }

        protected override void RenderTextRun(TextRunInline element, IRenderContext context)
        {
            var context_ = context as AndroidRenderContext;
            var builder = context_.Builder;

            // Creates raw text.
            var text = new SpannableString(CollapseWhitespace(context, element.Text));

            if (Foreground.HasValue)
            {
                text.SetSpan(new ForegroundColorSpan(Foreground.Value), 0, text.Length(), SpanTypes.ExclusiveExclusive);
            }

            // Add it to the current inlines
            builder.Append(text);
        }
    }
}