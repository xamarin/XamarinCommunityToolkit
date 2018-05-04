using System;
using Xamarin.Toolkit.Droid.Controls.Markdown.Display;

namespace Xamarin.Toolkit.Droid.Controls
{
    public partial class MarkdownTextView
    {
        public int? FontSize
        {
            get
            {
                return fontsize;
            }

            set
            {
                fontsize = value;
                Update();
            }
        }

        private int? fontsize;

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                Update();
            }
        }

        private string text;

        private Type renderertype = typeof(AndroidMarkdownRenderer);
    }
}
