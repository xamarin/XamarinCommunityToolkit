using System.Collections.Generic;
using CommunityToolkit.Maui.Sample.Models;
using CommunityToolkit.Maui.Sample.Pages.TestCases;

namespace CommunityToolkit.Maui.Sample.ViewModels.TestCases
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
		};
	}
}