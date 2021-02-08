using System.Collections.Generic;
using Xamarin.CommunityToolkit.AttachedProperties;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.AttachedProperties;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.AttachedProperties
{
	public class AttachedPropertiesGalleryViewModel : BaseGalleryViewModel
	{
		protected override IEnumerable<SectionModel> CreateItems() => new[]
		{
			new SectionModel(
				typeof(ProgressBarAttachedPropertiesPage),
				nameof(ProgressBarAttachedProperties),
				"Attached properties that are applicable for the ProgressBar")
		};
	}
}