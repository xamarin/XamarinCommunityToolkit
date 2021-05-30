using System.Collections.Generic;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Sample.Models;
using CommunityToolkit.Maui.Sample.Pages.Effects;

namespace CommunityToolkit.Maui.Sample.ViewModels.Effects
{
	public class ExtensionsGalleryViewModel : BaseGalleryViewModel
	{
		protected override IEnumerable<SectionModel> CreateItems() => new[]
		{
			new SectionModel(
				typeof(ImageResourceExtensionPage),
				nameof(ImageResourceExtension),
				"A XAML extension that helps to display images from embedded resources"),
		};
	}
}
