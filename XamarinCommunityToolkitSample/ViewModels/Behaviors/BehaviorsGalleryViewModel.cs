using System.Collections.Generic;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Behaviors;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Behaviors
{
	public class BehaviorsGalleryViewModel : BaseGalleryViewModel
	{
		public override IEnumerable<SectionModel> Items { get; } = new List<SectionModel>
		{
			new SectionModel(
				typeof(EmailValidationBehaviorPage),
				nameof(EmailValidationBehavior),
				"Changes an Entry's text color when an invalid e-mail address is provided"),
			new SectionModel(
				typeof(EventToCommandBehaviorPage),
				nameof(EventToCommandBehavior),
				"Turns any event into a command that can be bound to"),
			new SectionModel(
				typeof(NumericValidationBehaviorPage),
				nameof(NumericValidationBehavior),
				"Changes an Entry's text color when an invalid number is provided"),
			new SectionModel(
				typeof(AnimationBehaviorPage),
				nameof(AnimationBehavior),
				"Perform animation when the specified UI element event is triggered"),
			new SectionModel(
				typeof(MaskedBehaviorPage),
				nameof(MaskedBehavior),
				"Masked text in entry with specific pattern"),
			new SectionModel(
				typeof(UriValidationBehaviorPage),
				nameof(UriValidationBehavior),
				"Changes an Entry's text color when an invalid URI is provided"),
			new SectionModel(
				typeof(MultiValidationBehaviorPage),
				nameof(MultiValidationBehavior),
				"Combines multiple validation behavior"),
			new SectionModel(
				typeof(RequiredStringValidationBehaviorPage),
				nameof(RequiredStringValidationBehavior),
				"Changes an Entry's style when its text does not match another string"),
			new SectionModel(
				typeof(UserStoppedTypingBehaviorPage),
				nameof(UserStoppedTypingBehavior),
				"This behavior waits for the user to stop typing and then executes a Command"),
			new SectionModel(
				typeof(ImpliedOrderGridBehaviorPage),
				nameof(ImpliedOrderGridBehavior),
				"This behavior assigns grid row and column based on the order a view is added to the grid"),
			new SectionModel(
				typeof(MaxLengthReachedBehaviorPage),
				nameof(MaxLengthReachedBehavior),
				"This behavior invokes an EventHandler and executes a Command when the MaxLength of an InputView has been reached"),
			new SectionModel(
				typeof(CharactersValidationBehaviorPage),
				nameof(CharactersValidationBehavior),
				"Changes an Entry's text color when an invalid string is provided."),
		};
	}
}