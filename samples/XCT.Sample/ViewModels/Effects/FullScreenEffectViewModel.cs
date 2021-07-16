using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Effects;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Effects
{
	public class FullScreenEffectViewModel : BaseGalleryViewModel
	{
		protected override IEnumerable<SectionModel> CreateItems() => new[]
		{
			new SectionModel(typeof(FullScreenPersistentPage), "FullScreen mode enabled and persistent",
				"Persist when navigating away from page."),

			new SectionModel(typeof(FullScreenImmersivePage), "Android Specific: FullScreen mode Immersive",
				" "),

			new SectionModel(typeof(FullScreenStickyImmersivePage), "Android Specific: FullScreen mode Sticky Immersive",
				" "),

			new SectionModel(typeof(FullScreenLeanBackPage), "Android Specific: FullScreen mode LeanBack",
				" "),

						new SectionModel(typeof(FullScreenDisabledPage), "FullScreen mode Disabled",
				" "),
		};
	}
}