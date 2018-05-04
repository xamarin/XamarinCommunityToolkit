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

using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Microsoft.Toolkit.Parsers.Markdown.Display;
using Microsoft.Toolkit.Parsers.Markdown.Parse;

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
            codeArea.SetBackgroundColor(Droid.Graphics.Color.DodgerBlue);

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
            quoteArea.SetBackgroundColor(Droid.Graphics.Color.Gray);

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