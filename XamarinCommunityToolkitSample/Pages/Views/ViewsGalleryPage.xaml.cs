using System;
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
            => Navigation.PushAsync(PreparePage((ViewSectionId)parameter)));

        Page PreparePage(ViewSectionId id)
        {
            var page = GetPage(id);
            page.Title = id.GetTitle();
            return page;
        }

        Page GetPage(ViewSectionId id)
            => id switch
            {
                ViewSectionId.AvatarView => new AvatarViewPage(),
                _ => throw new NotImplementedException()
            };
    }
}
