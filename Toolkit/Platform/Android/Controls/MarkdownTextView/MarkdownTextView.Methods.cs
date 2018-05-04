using System;
using Android.Widget;
using Microsoft.Toolkit.Parsers.Markdown;
using Xamarin.Toolkit.Droid.Controls.Markdown.Display;

namespace Xamarin.Toolkit.Droid.Controls
{
    public partial class MarkdownTextView
    {
        /// <summary>
        /// Sets the Markdown Renderer for Rendering the UI.
        /// </summary>
        /// <typeparam name="T">The Inherited Markdown Render</typeparam>
        public void SetRenderer<T>()
            where T : AndroidMarkdownRenderer
        {
            renderertype = typeof(T);
        }

        public void Update()
        {
            if (IsAttachedToWindow)
            {
                RenderMarkdown();
            }
        }

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

            // Create the Markdown Renderer.
            var renderer = Activator.CreateInstance(renderertype, this, markdown) as AndroidMarkdownRenderer;
            if (renderer == null)
            {
                throw new Exception("Markdown Renderer was not of the correct type.");
            }

            // Now try to display it
            renderer.Render();
            renderer.FontSize = FontSize;
        }

        private void UnhookListeners()
        {
            // Clear any hyper link events if we have any

            // Clear everything that exists.
        }
    }
}
