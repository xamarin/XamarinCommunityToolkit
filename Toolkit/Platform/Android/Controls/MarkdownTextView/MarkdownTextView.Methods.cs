using System;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;
using Microsoft.Toolkit.Parsers.Markdown.Parse;
using Toolkit.Droid.UI.Controls.Markdown.Display;

namespace Xamarin.Toolkit.Droid.Controls
{
    public partial class MarkdownTextView
    {
        /// <summary>
        /// Called to preform a render of the current Markdown.
        /// </summary>
        private void RenderMarkdown()
        {
            // Make sure we have something to parse.
            if (string.IsNullOrWhiteSpace(Text))
            {
                return;
            }

            // Disconnect from OnClick handlers.
            UnhookListeners();

            var markdownRenderedArgs = new Microsoft.Toolkit.Parsers.Markdown.MarkdownRenderedEventArgs(null);
            try
            {
                // Try to parse the markdown.
                MarkdownDocument markdown = new MarkdownDocument();
                markdown.Parse(Text);

                // Now try to display it
                var renderer = new AndroidMarkdownRenderer(this, markdown)
                {
                };
                renderer.Render();
            }
            catch (Exception ex)
            {
                DebuggingReporter.ReportCriticalError("Error while parsing and rendering: " + ex.Message);
                markdownRenderedArgs = new Microsoft.Toolkit.Parsers.Markdown.MarkdownRenderedEventArgs(ex);
            }

            // Indicate that the parse is done.
            MarkdownRendered?.Invoke(this, markdownRenderedArgs);
        }

        private void UnhookListeners()
        {
            // Clear any hyper link events if we have any

            // Clear everything that exists.
        }
    }
}