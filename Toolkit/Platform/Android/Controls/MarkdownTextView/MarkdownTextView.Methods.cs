using System;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Graphics;
using Microsoft.Toolkit.Parsers.Markdown;
using Xamarin.Toolkit.Droid.Controls.Markdown;
using Xamarin.Toolkit.Droid.Controls.Markdown.Display;
using Xamarin.Toolkit.Droid.Helpers.Models;

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

        public async Task<ImageSource> ResolveImageAsync(string url, string tooltip)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                if (!string.IsNullOrEmpty(UriPrefix))
                {
                    url = string.Format("{0}{1}", UriPrefix, url);
                }
            }

            var eventArgs = new ImageResolvingEventArgs(url, tooltip);
            ImageResolving?.Invoke(this, eventArgs);

            if (eventArgs.TaskWaiter != null)
            {
                await eventArgs.TaskWaiter.Task;
            }

            try
            {
                return eventArgs.Handled
                    ? eventArgs.Image
                    : await GetImageSource(new Uri(url));
            }
            catch (Exception)
            {
                return null;
            }

            async Task<ImageSource> GetImageSource(Uri imageUrl)
            {
                using (var client = new HttpClient())
                {
                    using (var imagestream = await client.GetStreamAsync(imageUrl))
                    {
                        if (System.IO.Path.GetExtension(imageUrl.AbsolutePath)?.ToLowerInvariant() == ".svg")
                        {
                            // Add SVG Rendering
                            return null;
                        }
                        else
                        {
                            var bitmap = await BitmapFactory.DecodeStreamAsync(imagestream);
                            return new BitmapImageSource
                            {
                                Source = bitmap
                            };
                        }
                    }
                }
            }
        }

        private void UnhookListeners()
        {
            // Clear any hyper link events if we have any

            // Clear everything that exists.
        }
    }
}
