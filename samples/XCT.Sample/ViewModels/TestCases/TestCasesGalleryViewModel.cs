using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.TestCases;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.TestCases
{
	public class TestCasesGalleryViewModel : BaseGalleryViewModel
	{
		protected override IEnumerable<SectionModel> CreateItems() => new[]
		{
			new SectionModel(
				typeof(TouchEffectButtonPage),
				"TouchEffect + Button",
				"TouchEffect must automatically invoke button'c command execution."),

			new SectionModel(
				typeof(TouchEffectCollectionViewPage),
				"TouchEffect + CollectionView",
				"Using TouchEffect's LongPress should allow still use items selection."),

			new SectionModel(
				typeof(MediaElementSourcePage),
				"MediaElement with Source as string",
				"MediaElement should reproduce the video."),

			new SectionModel(
				typeof(MaskedBehaviorPage),
				"Masked behaviour with numbers in mask",
				"User should be able to enter numbers"),
		};
	}
}