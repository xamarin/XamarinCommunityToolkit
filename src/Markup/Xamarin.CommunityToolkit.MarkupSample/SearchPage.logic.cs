using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.MarkupSample
{
    public partial class SearchPage : BaseContentPage
    {
        readonly SearchViewModel vm;
        View header;

        public SearchPage(SearchViewModel vm)
        {
            BindingContext = this.vm = vm;
            Build();
        }

        void Search_FocusChanged(object sender, FocusEventArgs e)
        {
            ViewExtensions.CancelAnimations(header);
            header.TranslateTo(e.IsFocused ? -56 : 0, 0, 250, Easing.CubicOut);
        }
    }
}