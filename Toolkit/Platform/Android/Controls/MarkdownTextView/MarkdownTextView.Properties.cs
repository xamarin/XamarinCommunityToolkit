namespace Xamarin.Toolkit.Droid.Controls
{
    public partial class MarkdownTextView
    {
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                if (IsAttachedToWindow)
                {
                    RenderMarkdown();
                }
            }
        }

        private string text;
    }
}
