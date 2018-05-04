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