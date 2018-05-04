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

using Android.Content;
using Android.Util;
using Android.Widget;

namespace Xamarin.Toolkit.Droid.Controls
{
    public partial class MarkdownTextView : LinearLayout
    {
        public MarkdownTextView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            Orientation = Orientation.Vertical;
            SetBackgroundColor(Droid.Graphics.Color.White);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            RenderMarkdown();
        }
    }
}