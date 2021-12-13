using System.Collections.Generic;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Effects;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Effects
{
	public class ExtensionsGalleryViewModel : BaseGalleryViewModel
	{
		protected override IEnumerable<SectionModel> CreateItems() => new[]
		{
			new SectionModel(
				typeof(ImageResourceExtensionPage),
				nameof(ImageResourceExtension),
				"A XAML extension that helps to display images from embedded resources"),

			new SectionModel(
				typeof(SemanticExtensionsPage),
				nameof(SemanticExtensions),
				"An extension that offers further semantic APIs for screen reader accessibility"),
		};
	}
}