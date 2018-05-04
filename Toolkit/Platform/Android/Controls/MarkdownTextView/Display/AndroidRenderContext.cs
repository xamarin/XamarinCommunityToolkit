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
using Android.Text;
using Microsoft.Toolkit.Parsers.Markdown.Display;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Display
{
    public class AndroidRenderContext : IRenderContext
    {
        public Color? Foreground { get; set; }

        public bool TrimLeadingWhitespace { get; set; }

        public bool WithinHyperlink { get; set; }

        public object Parent { get; set; }

        public SpannableStringBuilder Builder { get; set; }

        public IRenderContext Clone()
        {
            return (IRenderContext)MemberwiseClone();
        }
    }
}