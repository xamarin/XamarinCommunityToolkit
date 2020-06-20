using System;
using System.Collections.Generic;
using System.Linq;
using XamarinCommunityToolkitSample.Models.Behaviors;

namespace XamarinCommunityToolkitSample.ViewModels.Behaviors
{
    public class BehaviorsGalleryViewModel : BaseViewModel
    {
        public IEnumerable<BehaviorSectionModel> Items { get; } =
            ((BehaviorSectionId[])Enum
            .GetValues(typeof(BehaviorSectionId)))
            .Select(id => new BehaviorSectionModel(id));
    }
}
