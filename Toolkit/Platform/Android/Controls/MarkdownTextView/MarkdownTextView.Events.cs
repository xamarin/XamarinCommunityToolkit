using System;
using Xamarin.Toolkit.Droid.Controls.Markdown;

namespace Xamarin.Toolkit.Droid.Controls
{
#pragma warning disable CS0067

    public partial class MarkdownTextView
    {
        /// <summary>
        /// Fired when the text is done parsing and formatting. Fires each time the markdown is rendered.
        /// </summary>
        public event EventHandler<MarkdownRenderedEventArgs> MarkdownRendered;

        /// <summary>
        /// Fired when a link element in the markdown was tapped.
        /// </summary>
        public event EventHandler<LinkClickedEventArgs> LinkClicked;

        /// <summary>
        /// Fired when an image element in the markdown was tapped.
        /// </summary>
        public event EventHandler<LinkClickedEventArgs> ImageClicked;

        /// <summary>
        /// Fired when an image from the markdown document needs to be resolved.
        /// The default implementation is basically <code>new BitmapImage(new Uri(e.Url));</code>.
        /// <para/>You must set <see cref="ImageResolvingEventArgs.Handled"/> to true in order to process your changes.
        /// </summary>
        public event EventHandler<ImageResolvingEventArgs> ImageResolving;
    }

#pragma warning restore CS0067
}
