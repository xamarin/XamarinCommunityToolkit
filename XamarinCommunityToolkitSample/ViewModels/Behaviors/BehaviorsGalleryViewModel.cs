using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Behaviors;
using Xamarin.CommunityToolkit.Sample.Resx;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
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
