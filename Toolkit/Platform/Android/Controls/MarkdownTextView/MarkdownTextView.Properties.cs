using System;
using System.Collections.Generic;
using Xamarin.Toolkit.Droid.Controls.Markdown.Render;

namespace Xamarin.Toolkit.Droid.Controls
{
    public partial class MarkdownTextView
    {
        /// <summary>
        /// Holds a list of hyperlinks we are listening to.
        /// </summary>
        private readonly List<object> listeningLinks = new List<object>();

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
