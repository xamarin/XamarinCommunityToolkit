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
using Android.Support.Text.Emoji;
using Android.Support.Text.Emoji.Bundled;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Microsoft.Toolkit.Parsers.Markdown.Display;
using Microsoft.Toolkit.Parsers.Markdown.Parse;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Display
{
    internal partial class AndroidMarkdownRenderer : MarkdownRendererBase
    {
        public AndroidMarkdownRenderer(LinearLayout RootLayout, MarkdownDocument document)
            : base(document)
        {
            if (EmojiCompat == null)
            {
                try
                {
                    EmojiCompat = EmojiCompat.Get();
                }
                catch
                {
                    EmojiCompat = EmojiCompat.Init(new BundledEmojiCompatConfig(Application.Context));
                }
            }

            this.RootLayout = RootLayout;
        }

        public void Render()
        {
            Render(new AndroidRenderContext { Foreground = Foreground, Parent = RootLayout });
        }

        private void MakeHyperlinkSpan(string url, SpannableString span, IRenderContext context)
        {
            var context_ = context as AndroidRenderContext;
            var foreground = LinkForeground ?? context_.Foreground;

            var length = span.Length();
            span.SetSpan(new URLSpan(url), 0, length, SpanTypes.ExclusiveExclusive);
            span.SetSpan(new MarkdownClickSpan(url), 0, length, SpanTypes.ExclusiveExclusive);
            if (LinkForeground != null)
            {
                span.SetSpan(new ForegroundColorSpan(LinkForeground.Value), 0, length, SpanTypes.ExclusiveExclusive);
            }
        }

        private class MarkdownClickSpan : ClickableSpan
        {
            public MarkdownClickSpan(string Url)
            {
                this.Url = Url;
            }

            public override void OnClick(View widget)
            {
                Intent viewLink = new Intent(Intent.ActionView, Droid.Net.Uri.Parse(Url));
                var activity = widget.Context as Activity;
                activity.StartActivity(viewLink);
            }

            public string Url { get; }
        }
    }
}