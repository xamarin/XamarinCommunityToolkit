namespace Xamarin.Toolkit.Droid.Controls
{
    public partial class MarkdownTextView
    {
        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
                if (IsAttachedToWindow)
                {
                    RenderMarkdown();
                }
            }
        }

        private string _Text;
    }
}