using System.Collections.Generic;
using Xamarin.Forms;
using XamarinCommunityToolkitSample.Models;
using XamarinCommunityToolkitSample.Pages.Behaviors;

namespace XamarinCommunityToolkitSample.ViewModels.Behaviors
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
        };
    }
}
