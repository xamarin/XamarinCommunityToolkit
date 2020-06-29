using System;
using System.Collections.Generic;
using System.Linq;
using XamarinCommunityToolkitSample.Models.Views;

namespace XamarinCommunityToolkitSample.ViewModels.Views
{
    public class ViewsGalleryViewModel : BaseViewModel
    {
        public IEnumerable<ViewSectionModel> Items { get; } =
            ((ViewSectionId[])Enum
            .GetValues(typeof(ViewSectionId)))
            .Select(id => new ViewSectionModel(id));
    }
}
