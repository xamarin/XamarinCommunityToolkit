using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models.Views;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Views
{
    public class ViewsGalleryViewModel : BaseViewModel
    {
        public IEnumerable<ViewSectionModel> Items { get; } =
            ((ViewSectionId[])Enum
            .GetValues(typeof(ViewSectionId)))
            .Select(id => new ViewSectionModel(id));
    }
}
