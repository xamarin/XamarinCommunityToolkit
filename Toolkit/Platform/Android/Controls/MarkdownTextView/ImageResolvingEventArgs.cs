using System;
using System.Threading.Tasks;
using Xamarin.Toolkit.Droid.Helpers.Models;

namespace Xamarin.Toolkit.Droid.Controls.Markdown
{
    /// <summary>
    /// Arguments for the <see cref="MarkdownTextView.ImageResolving"/> event which is called when a url needs to be resolved to a <see cref="ImageSource"/>.
    /// </summary>
    public class ImageResolvingEventArgs : EventArgs
    {
        internal ImageResolvingEventArgs(string url, string tooltip)
        {
            Url = url;
            Tooltip = tooltip;
        }

        /// <summary>
        /// Gets the Task Waiter indicating if a long running task needs to be waited for.
        /// </summary>
        /// <returns></returns>
        public TaskCompletionSource<object> GetWaiter()
        {
            return TaskWaiter ?? (TaskWaiter = new TaskCompletionSource<object>());
        }

        /// <summary>
        /// Gets the url of the image in the markdown document.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Gets the tooltip of the image in the markdown document.
        /// </summary>
        public string Tooltip { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this event was handled successfully.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets or sets the image to display in the <see cref="MarkdownTextView"/>.
        /// </summary>
        public ImageSource Image { get; set; }

        /// <summary>
        /// Informs the <see cref="MarkdownTextView"/> that the event handler might run asynchronously.
        /// </summary>
        /// <returns>Deferral</returns>
        internal TaskCompletionSource<object> TaskWaiter { get; set; }
    }
}
