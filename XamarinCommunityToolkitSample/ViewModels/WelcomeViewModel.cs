using System;
using System.Collections.Generic;
using System.Linq;
using XamarinCommunityToolkitSample.Models;

namespace XamarinCommunityToolkitSample.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public IEnumerable<WelcomeSectionModel> Items { get; } =
            ((WelcomeSectionId[])Enum
            .GetValues(typeof(WelcomeSectionId)))
            .Select(id => new WelcomeSectionModel(id));
    }
}
