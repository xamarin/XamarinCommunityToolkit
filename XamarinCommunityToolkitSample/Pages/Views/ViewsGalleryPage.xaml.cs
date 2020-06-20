using System.Windows.Input;
using Xamarin.Forms;
using XamarinCommunityToolkitSample.Models.Views;

namespace XamarinCommunityToolkitSample.Pages.Views
{
    public partial class ViewsGalleryPage : ContentPage
    {
        ICommand navigateCommand;

        public ViewsGalleryPage()
            => InitializeComponent();

        public ICommand NavigateCommand => navigateCommand ??= new Command(parameter
            => Navigation.PushAsync(GetPage((ViewSectionId)parameter)));

        Page GetPage(ViewSectionId id)
        {
            var page = id switch
            {
                ViewSectionId.AvatarView => new AvatarViewPage(),
                _ => throw new System.NotImplementedException()
            };
            page.Title = id.GetTitle();
            return page;
        }
    }
}
