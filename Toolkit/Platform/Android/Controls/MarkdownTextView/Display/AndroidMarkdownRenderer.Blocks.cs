using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Render;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Display
{
    internal partial class AndroidMarkdownRenderer
    {
        protected override void RenderCode(CodeBlock element, IRenderContext context)
        {
            var context_ = context as AndroidRenderContext;
            var parent = context.Parent as ViewGroup;

            var codeArea = new LinearLayout(RootLayout.Context)
            {
                Orientation = Orientation.Vertical
            };
            codeArea.SetPadding(5, 5, 5, 5);
            codeArea.SetBackgroundColor(global::Android.Graphics.Color.DodgerBlue);

            var text = new SpannableString(element.Text);
            var length = text.Length();
            text.SetSpan(new TypefaceSpan(CodeFontFamily), 0, length, SpanTypes.ExclusiveExclusive);

            var textArea = new TextView(RootLayout.Context)
            {
                TextFormatted = text
            };

            codeArea.AddView(textArea);
            parent.AddView(codeArea);
        }

        protected override void RenderHeader(HeaderBlock element, IRenderContext context)
        {
        }

        protected override void RenderHorizontalRule(IRenderContext context)
        {
        }

        protected override void RenderListElement(ListBlock element, IRenderContext context)
        {
        }

        protected override void RenderParagraph(ParagraphBlock element, IRenderContext context)
        {
            var context_ = context as AndroidRenderContext;
            var parent = context.Parent as ViewGroup;

            var textview = new TextView(RootLayout.Context);

            var subbuilder = new SpannableStringBuilder();
            var subcontext = context.Clone() as AndroidRenderContext;
            subcontext.Builder = subbuilder;
            subcontext.Parent = textview;

            RenderInlineChildren(element.Inlines, subcontext);
            textview.TextFormatted = subbuilder;
            parent.AddView(textview);
        }

        protected override void RenderQuote(QuoteBlock element, IRenderContext context)
        {
            var context_ = context as AndroidRenderContext;
            var parent = context.Parent as ViewGroup;

            var quoteArea = new LinearLayout(RootLayout.Context)
            {
                Orientation = Orientation.Vertical
            };
            quoteArea.SetPadding(5, 5, 5, 5);
            quoteArea.SetBackgroundColor(global::Android.Graphics.Color.Gray);

            var subcontext = context.Clone();
            subcontext.Parent = quoteArea;

            RenderBlocks(element.Blocks, subcontext);

            parent.AddView(quoteArea);
        }

        protected override void RenderTable(TableBlock element, IRenderContext context)
        {
        }
    }
}
