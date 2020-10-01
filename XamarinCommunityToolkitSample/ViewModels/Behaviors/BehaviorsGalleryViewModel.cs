using System.Collections.Generic;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Behaviors;
using Xamarin.CommunityToolkit.Sample.Resx;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
{
	public class BehaviorsGalleryViewModel : BaseViewModel
	{
		public IEnumerable<SectionModel> Items { get; } = new List<SectionModel>
		{
			new SectionModel(
				typeof(EmailValidationBehaviorPage),
				nameof(EmailValidationBehavior),
				AppResources.EmailValidationShortDescription),
			new SectionModel(
				typeof(EventToCommandBehaviorPage),
				nameof(EventToCommandBehavior),
				AppResources.EventToCommandShortDescription),
			new SectionModel(
				typeof(NumericValidationBehaviorPage),
				nameof(NumericValidationBehavior),
				AppResources.NumericValidationShortDescription),
			new SectionModel(
				typeof(AnimationBehaviorPage),
				nameof(AnimationBehavior),
				AppResources.AnimatioShortDescription),
			new SectionModel(
				typeof(MaskedBehaviorPage),
				nameof(MaskedBehavior),
				AppResources.MaskedShortDescription),
			new SectionModel(
				typeof(UriValidationBehaviorPage),
				nameof(UriValidationBehavior),
				AppResources.UriValidationShortDescription),
			new SectionModel(
				typeof(MultiValidationBehaviorPage),
				nameof(MultiValidationBehavior),
				AppResources.MultiValidationShortDescription),
			new SectionModel(
				typeof(UserStoppedTypingBehaviorPage),
				nameof(UserStoppedTypingBehavior),
				AppResources.UserStoppedTypingBehaviorShortDescription),
			new SectionModel(
				typeof(ImpliedOrderGridBehaviorPage),
				nameof(ImpliedOrderGridBehavior),
				AppResources.AutoGridLayoutBehaviorDescription)
		};
	}
}