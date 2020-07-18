using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models.Behaviors;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Behaviors
{
    public class BehaviorsGalleryViewModel : BaseViewModel
    {
        public IEnumerable<BehaviorSectionModel> Items { get; } =
            ((BehaviorSectionId[])Enum
            .GetValues(typeof(BehaviorSectionId)))
            .Select(id => new BehaviorSectionModel(id));
    }
}
