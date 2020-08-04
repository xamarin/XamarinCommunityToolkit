using System.Collections.Generic;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.Behaviors;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Resx;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Behaviors
{
    public class BehaviorsGalleryViewModel : BaseViewModel
    {
        public IEnumerable<SectionModel> Items { get; } = new List<SectionModel> {
            new SectionModel(
                typeof(EmailValidationBehaviorPage),
                "EmailValidationBehavior",
                AppResources.EmailValidationShortDescription
            ),
            new SectionModel(
                typeof(EventToCommandBehaviorPage),
                "EventToCommandBehavior",
                AppResources.EventToCommandShortDescription
            ),
            new SectionModel(
                typeof(NumericValidationBehaviorPage),
                "NumericValidationBehavior",
                AppResources.NumericValidationShortDescription
            ),
            new SectionModel(
                typeof(AnimationBehaviorPage),
                "AnimationBehavior",
                AppResources.AnimatioShortDescription
            )
        };
    }
}
