using Android.Graphics;
using Android.Text;
using Microsoft.Toolkit.Parsers.Markdown.Render;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Render
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
