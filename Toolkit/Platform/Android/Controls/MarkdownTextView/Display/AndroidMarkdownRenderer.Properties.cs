using Android.Graphics;
using Android.Support.Text.Emoji;
using Android.Widget;
using Xamarin.Toolkit.Droid.Helpers.Models;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Display
{
    public partial class AndroidMarkdownRenderer
    {
        private static EmojiCompat EmojiCompat { get; set; }

        // Handling

        private readonly IImageResolver imageResolver;

        private LinearLayout RootLayout { get; set; }

        // Generic

        public Color Background { get; set; } = Color.White;

        public Color Foreground { get; set; } = Color.Black;

        public string CodeFontFamily { get; set; }

        public Color? LinkForeground { get; set; } = Color.Blue;

        public int? FontSize { get; set; }

        // Rule

        public Color? HorizontalRuleColor { get; set; } = Color.DarkGray;

        public Thickness? HorizontalRuleMargin { get; set; } = new Thickness(10);

        public int HorizontalRuleThickness { get; set; } = 2;

        // Headers
        // h1

        public int H1FontSize { get; set; } = 24;

        public TypefaceStyle H1FontStyle { get; set; } = TypefaceStyle.Bold;

        public Thickness H1Margin { get; set; } = new Thickness(0, 15, 0, 15);

        public Color? H1Foreground { get; set; }

        // h2

        public int H2FontSize { get; set; } = 22;

        public TypefaceStyle H2FontStyle { get; set; }

        public Thickness H2Margin { get; set; } = new Thickness(0, 15, 0, 15);

        public Color? H2Foreground { get; set; }

        // h3

        public int H3FontSize { get; set; } = 20;

        public TypefaceStyle H3FontStyle { get; set; } = TypefaceStyle.Bold;

        public Thickness H3Margin { get; set; } = new Thickness(0, 10, 0, 10);

        public Color? H3Foreground { get; set; }

        // h4

        public int H4FontSize { get; set; } = 18;

        public TypefaceStyle H4FontStyle { get; set; }

        public Thickness H4Margin { get; set; } = new Thickness(0, 10, 0, 10);

        public Color? H4Foreground { get; set; }

        // h5

        public int H5FontSize { get; set; } = 16;

        public TypefaceStyle H5FontStyle { get; set; } = TypefaceStyle.Bold;

        public Thickness H5Margin { get; set; } = new Thickness(0, 10, 0, 5);

        public Color? H5Foreground { get; set; }

        // h6

        public int H6FontSize { get; set; } = 14;

        public TypefaceStyle H6FontStyle { get; set; }

        public Thickness H6Margin { get; set; } = new Thickness(0, 10, 0, 0);

        public Color? H6Foreground { get; set; }

        // Inline Code

        public Color? InlineCodeBackground { get; set; } = Color.LightGray;

        public Color? InlineCodeForeground { get; set; }

        // Code Block

        public Color? CodeBackground { get; set; } = Color.DodgerBlue;

        public Color? CodeForeground { get; set; }

        public Thickness CodePadding { get; set; } = new Thickness(5);

        // Quote

        public Color? QuoteBackground { get; set; } = Color.Gray;

        public Color? QuoteForeground { get; set; }

        public Thickness QuotePadding { get; set; } = new Thickness(5);
    }
}
