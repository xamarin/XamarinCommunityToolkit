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

			new SectionModel(typeof(FullScreenImmersivePage), "FullScreen mode Immersive",
				" "),

			new SectionModel(typeof(FullScreenStickyImmersivePage), "FullScreen mode Sticky Immersive",
				" "),

			new SectionModel(typeof(FullScreenLeanBackPage), "FullScreen mode LeanBack",
				" "),

		};
	}
}