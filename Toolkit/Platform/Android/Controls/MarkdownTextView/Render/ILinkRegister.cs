using Xamarin.Toolkit.Droid.Helpers.Text;

namespace Xamarin.Toolkit.Droid.Controls.Markdown.Render
{
    /// <summary>
    /// An interface used to handle links in the markdown.
    /// </summary>
    public interface ILinkRegister
    {
        /// <summary>
        /// Registers a Hyperlink with a LinkUrl.
        /// </summary>
        /// <param name="clickSpan">Click Span to Register.</param>
        /// <param name="isImage">Is the UI an image?</param>
        void RegisterNewHyperLink(EventClickableSpan clickSpan, bool isImage = false);
    }
}
