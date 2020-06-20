using XamarinCommunityToolkitSample.Models.Views;

namespace XamarinCommunityToolkitSample.ViewModels.Views
{
    public class ViewsGalleryViewModel: BaseViewModel
    {
        public ViewSectionModel[] Items { get; } =
        {
            new ViewSectionModel(ViewSectionId.AvatarView),
        };
    }
}
