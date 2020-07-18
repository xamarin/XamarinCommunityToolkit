using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public IEnumerable<WelcomeSectionModel> Items { get; } =
            ((WelcomeSectionId[])Enum
            .GetValues(typeof(WelcomeSectionId)))
            .Select(id => new WelcomeSectionModel(id));
    }
}
