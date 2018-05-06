using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Graphics;
using Com.Caverock.Androidsvg;
using Microsoft.Toolkit.Parsers.Markdown;
using Xamarin.Toolkit.Droid.Controls.Markdown;
using Xamarin.Toolkit.Droid.Controls.Markdown.Render;
using Xamarin.Toolkit.Droid.Helpers.Models;
using Xamarin.Toolkit.Droid.Helpers.Text;

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
            var renderer = Activator.CreateInstance(renderertype, markdown, this, this, this) as AndroidMarkdownRenderer;
            if (renderer == null)
            {
                throw new Exception("Markdown Renderer was not of the correct type.");
            }

            // Now try to display it
            renderer.Render();
            renderer.FontSize = FontSize;
        }

        public void RegisterNewHyperLink(EventClickableSpan clickSpan, bool isImage = false)
        {
            clickSpan.Clicked += ClickSpan_Clicked;

            listeningLinks.Add((clickSpan, isImage));
        }

        private void ClickSpan_Clicked(object sender, EventArgs e)
        {
            // Determine if an image.
            var isimage = listeningLinks
                .FirstOrDefault(item => item.span == sender)
                .isImage;

            if (sender is EventClickableSpan span)
            {
                var args = new LinkClickedEventArgs(span.Url);
                if (isimage)
                {
                    ImageClicked?.Invoke(this, args);
                }
                else
                {
                    LinkClicked?.Invoke(this, args);
                }
            }
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
                    using (var netstream = await client.GetStreamAsync(imageUrl))
                    {
                        var imagestream = new MemoryStream();
                        await netstream.CopyToAsync(imagestream);
                        imagestream.Seek(0, SeekOrigin.Begin);

                        if (System.IO.Path.GetExtension(imageUrl.AbsolutePath)?.ToLowerInvariant() == ".svg")
                        {
                            // Add SVG Rendering
                            var svg = SVG.GetFromInputStream(imagestream);
                            return new SVGImageSource
                            {
                                Source = svg
                            };
                        }
                        else
                        {
                            var bitmap = BitmapFactory.DecodeStream(imagestream);
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
            foreach (var link in listeningLinks.ToList())
            {
                link.span.Clicked -= ClickSpan_Clicked;
                listeningLinks.Remove(link);
            }

            // Clear everything that exists.
        }
    }
}
