using Android.Graphics;
using Android.Support.Text.Emoji;
using Android.Widget;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Display
{
    internal partial class AndroidMarkdownRenderer
    {
        public static EmojiCompat EmojiCompat { get; internal set; }

        public string CodeFontFamily { get; set; }

        public LinearLayout RootLayout { get; set; }

        public Color? LinkForeground { get; set; }

        public Color? Foreground { get; set; } = Color.Black;

        public Color? InlineCodeBackground { get; set; } = Color.LightGray;
    }
}