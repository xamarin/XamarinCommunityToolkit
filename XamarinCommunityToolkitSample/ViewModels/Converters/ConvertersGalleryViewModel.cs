using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models.Converters;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Converters
{
    public class ConvertersGalleryViewModel : BaseViewModel
    {
        public IEnumerable<ConverterSectionModel> Items { get; } =
            ((ConverterSectionId[])Enum
            .GetValues(typeof(ConverterSectionId)))
            .Select(id => new ConverterSectionModel(id));
    }
}
