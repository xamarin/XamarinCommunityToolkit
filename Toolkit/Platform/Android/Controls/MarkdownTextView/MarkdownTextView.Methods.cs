using Microsoft.Toolkit.Parsers.Markdown;
using Xamarin.Toolkit.Droid.Controls.Markdown.Display;

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

            // Try to parse the markdown.
            var markdown = new MarkdownDocument();
            markdown.Parse(Text);

            // Now try to display it
            var renderer = new AndroidMarkdownRenderer(this, markdown);
            renderer.Render();
        }

        private void UnhookListeners()
        {
            // Clear any hyper link events if we have any

            // Clear everything that exists.
        }
    }
}
