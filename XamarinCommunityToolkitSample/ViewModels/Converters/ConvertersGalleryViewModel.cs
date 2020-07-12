using System;
using System.Collections.Generic;
using System.Linq;
using XamarinCommunityToolkitSample.Models.Converters;

namespace XamarinCommunityToolkitSample.ViewModels.Converters
{
    public class ConvertersGalleryViewModel : BaseViewModel
    {
        public IEnumerable<ConverterSectionModel> Items { get; } =
            ((ConverterSectionId[])Enum
            .GetValues(typeof(ConverterSectionId)))
            .Select(id => new ConverterSectionModel(id));
    }
}
