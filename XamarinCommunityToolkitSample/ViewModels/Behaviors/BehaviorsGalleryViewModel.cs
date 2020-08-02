using System.Collections.Generic;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.Behaviors;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Behaviors
{
    public class BehaviorsGalleryViewModel : BaseViewModel
    {
        public IEnumerable<SectionModel> Items { get; } = new List<SectionModel> {
            new SectionModel(
                typeof(EmailValidationBehaviorPage),
                "EmailValidationBehavior",
                "Changes an Entry's text color when an invalid e-mail address is provided."
            ),
            new SectionModel(
                typeof(EventToCommandBehaviorPage),
                "EventToCommandBehavior",
                "Turns any event into a command that can be bound to."
            ),
            new SectionModel(
                typeof(NumericValidationBehaviorPage),
                "NumericValidationBehavior",
                "Changes an Entry's text color when an invalid numeric value is provided."
            ),
            new SectionModel(
                typeof(AnimationBehaviorPage),
                "AnimationBehavior",
                "Perform animation when the specified UI element event is triggered"
            ),
            new SectionModel(
                typeof(MaskedBehaviorPage),
                "MaskedBehavior",
                "Masked text in entry with specific patter"
            )
        };
    }
}
