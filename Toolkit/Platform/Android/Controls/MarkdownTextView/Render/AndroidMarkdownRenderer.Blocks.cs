using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Microsoft.Toolkit.Parsers.Markdown.Blocks;
using Microsoft.Toolkit.Parsers.Markdown.Enums;
using Microsoft.Toolkit.Parsers.Markdown.Render;
using Xamarin.Toolkit.Droid.Helpers;
using Xamarin.Toolkit.Droid.Helpers.Models;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Render
{
    public partial class AndroidMarkdownRenderer
    {
        protected override void RenderCode(CodeBlock element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var parent = context.Parent as ViewGroup;

            var codeArea = new LinearLayout(androidContext)
            {
                Orientation = Orientation.Vertical
            };
            codeArea.SetPadding(CodePadding);
            if (CodeBackground != null)
            {
                codeArea.SetBackgroundColor(CodeBackground.Value);
            }

            var subcontext = localcontext.Clone();
            subcontext.TrimLeadingWhitespace = true;

            var text = new SpannableString(CollapseWhitespace(subcontext, element.Text));
            var length = text.Length();
            text.SetSpanAll(new TypefaceSpan(CodeFontFamily));
            if (FontSize != null)
            {
                text.SetSpanAll(new AbsoluteSizeSpan(FontSize.Value));
            }

            var textArea = CreateTextView();
            SetText(textArea, text);

            codeArea.AddView(textArea);
            parent.AddView(codeArea);
        }

        protected override void RenderHeader(HeaderBlock element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var parent = context.Parent as ViewGroup;

            var subbuilder = new SpannableStringBuilder();
            var subcontext = context.Clone() as AndroidRenderContext;
            subcontext.Builder = subbuilder;

            subcontext.TrimLeadingWhitespace = true;
            RenderInlineChildren(element.Inlines, subcontext);

            var fsize = FontSize;
            var style = TypefaceStyle.Normal;
            Color? color = null;
            Thickness? margin = null;

            switch (element.HeaderLevel)
            {
                case 1:
                    fsize = H1FontSize;
                    style = H1FontStyle;
                    color = H1Foreground;
                    margin = H1Margin;
                    break;

                case 2:
                    fsize = H2FontSize;
                    style = H2FontStyle;
                    color = H2Foreground;
                    margin = H2Margin;
                    break;

                case 3:
                    fsize = H3FontSize;
                    style = H3FontStyle;
                    color = H3Foreground;
                    margin = H3Margin;
                    break;

                case 4:
                    fsize = H4FontSize;
                    style = H4FontStyle;
                    color = H4Foreground;
                    margin = H4Margin;
                    break;

                case 5:
                    fsize = H5FontSize;
                    style = H5FontStyle;
                    color = H5Foreground;
                    margin = H5Margin;
                    break;

                case 6:
                    fsize = H6FontSize;
                    style = H6FontStyle;
                    color = H6Foreground;
                    margin = H6Margin;

                    subbuilder.SetSpanAll(new UnderlineSpan());
                    break;
            }

            if (fsize != null)
            {
                subbuilder.SetSpanAll(new AbsoluteSizeSpan(fsize.Value, true));
            }

            subbuilder.SetSpanAll(new StyleSpan(style));
            subbuilder.SetSpanAll(new ForegroundColorSpan(color ?? Foreground));

            if (margin != null)
            {
                subbuilder.SetMarginSpanAll(margin.Value);
            }

            var textview = CreateTextView();
            SetText(textview, subbuilder);
            parent.AddView(textview);
        }

        protected override void RenderHorizontalRule(IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var parent = context.Parent as ViewGroup;

            var height = HorizontalRuleThickness.GetDisplayPixels();
            var layout = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, height);

            if (HorizontalRuleMargin != null)
            {
                layout.SetMargin(HorizontalRuleMargin.Value);
            }

            var view = new View(androidContext)
            {
                LayoutParameters = layout
            };

            if (HorizontalRuleColor != null)
            {
                view.SetBackgroundColor(HorizontalRuleColor.Value);
            }

            parent.AddView(view);
        }

        protected override void RenderListElement(ListBlock element, IRenderContext context)
        {
        }

        protected override void RenderParagraph(ParagraphBlock element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var parent = context.Parent as ViewGroup;

            var subbuilder = new SpannableStringBuilder();
            var subcontext = context.Clone() as AndroidRenderContext;
            subcontext.Builder = subbuilder;
            subcontext.TrimLeadingWhitespace = true;
            RenderInlineChildren(element.Inlines, subcontext);

            var textview = CreateTextView();
            SetText(textview, subbuilder);
            parent.AddView(textview);
        }

        protected override void RenderQuote(QuoteBlock element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var parent = context.Parent as ViewGroup;

            var quoteArea = new LinearLayout(androidContext)
            {
                Orientation = Orientation.Vertical
            };
            quoteArea.SetPadding(QuotePadding);
            if (QuoteBackground != null)
            {
                quoteArea.SetBackgroundColor(QuoteBackground.Value);
            }

            var subcontext = context.Clone();
            subcontext.Parent = quoteArea;
            subcontext.TrimLeadingWhitespace = true;

            RenderBlocks(element.Blocks, subcontext);

            parent.AddView(quoteArea);
        }

        protected override void RenderTable(TableBlock element, IRenderContext context)
        {
            var localcontext = context as AndroidRenderContext;
            var parent = context.Parent as ViewGroup;

            TableLayout.LayoutParams RowMargin(Thickness margin)
            {
                var paramSet =
                  new TableLayout.LayoutParams(
                  ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

                paramSet.SetMargin(margin);

                return paramSet;
            }

            var tableRowMargin = new Thickness(0, 0, -TableBorderThickness, -TableBorderThickness);

            var background = new ShapeDrawable(new RectShape());
            background.Paint.Color = TableBorderColor ?? Background;
            background.Paint.SetStyle(Paint.Style.Stroke);
            background.Paint.StrokeWidth = TableBorderThickness;

            var table = new TableLayout(androidContext)
            {
                StretchAllColumns = true
            };

            table.SetPadding(TablePadding);

            for (var r = 0; r < element.Rows.Count; r++)
            {
                var row = element.Rows[r];
                var tbrow = new TableRow(androidContext);

                for (var c = 0; c < row.Cells.Count; c++)
                {
                    var cell = row.Cells[c];
                    var column = element.ColumnDefinitions[c];

                    var colgravity = GravityFlags.Fill;
                    switch (column.Alignment)
                    {
                        case ColumnAlignment.Center:
                            colgravity = GravityFlags.Center;
                            break;

                        case ColumnAlignment.Left:
                            colgravity = GravityFlags.Left;
                            break;

                        case ColumnAlignment.Right:
                            colgravity = GravityFlags.Right;
                            break;
                    }

                    var subbuilder = new SpannableStringBuilder();
                    var subcontext = context.Clone() as AndroidRenderContext;
                    subcontext.Builder = subbuilder;
                    subcontext.TrimLeadingWhitespace = true;
                    RenderInlineChildren(cell.Inlines, subcontext);

                    if (FontSize != null)
                    {
                        subbuilder.SetSpanAll(new AbsoluteSizeSpan(FontSize.Value, true));
                    }

                    var textview = CreateTextView();
                    SetText(textview, subbuilder);

                    textview.Gravity = colgravity;
                    textview.Background = background;
                    tbrow.AddView(textview);
                }

                if (r == element.Rows.Count - 1)
                {
                    tableRowMargin.Bottom = 0;
                }

                table.AddView(tbrow, RowMargin(tableRowMargin));
            }

            parent.AddView(table);
        }
    }
}
