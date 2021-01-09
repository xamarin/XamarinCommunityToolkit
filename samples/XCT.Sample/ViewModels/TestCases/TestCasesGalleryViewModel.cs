using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.TestCases;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.TestCases
{
	public class TestCasesGalleryViewModel : BaseGalleryViewModel
	{
		public override IEnumerable<SectionModel> Items { get; } = new List<SectionModel>
		{
			new SectionModel(
				typeof(TouchEffectButtonPage),
				"TouchEffect + Button",
				"TouchEffect must automatically invoke button'c command execution."),

			new SectionModel(
				typeof(MediaElementSourcePage),
				"MediaElement with Source as string",
				"MediaElement should reproduce the video."),
		};
	}
}