using System;
using Microsoft.Toolkit.Parsers.Markdown;

namespace Xamarin.Toolkit.Droid.Controls
{
    public partial class MarkdownTextView
    {
        /// <summary>
        /// Fired when the text is done parsing and formatting. Fires each time the markdown is rendered.
        /// </summary>
        public event EventHandler<MarkdownRenderedEventArgs> MarkdownRendered;
    }
}