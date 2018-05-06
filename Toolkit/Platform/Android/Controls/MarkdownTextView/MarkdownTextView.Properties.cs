using System;
using System.Collections.Generic;
using Xamarin.Toolkit.Droid.Controls.Markdown.Render;
using Xamarin.Toolkit.Droid.Helpers.Text;

namespace Xamarin.Toolkit.Droid.Controls
{
    public partial class MarkdownTextView
    {
        /// <summary>
        /// Holds a list of hyperlinks we are listening to.
        /// </summary>
        private readonly List<(EventClickableSpan span, bool isImage)> listeningLinks = new List<(EventClickableSpan span, bool isImage)>();

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

        public string UriPrefix { get; set; }

        private Type renderertype = typeof(AndroidMarkdownRenderer);
    }
}
