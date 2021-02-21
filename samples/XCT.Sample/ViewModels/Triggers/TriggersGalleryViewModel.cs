using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Triggers;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Triggers
{
	public class TriggersGalleryViewModel : BaseGalleryViewModel
	{
		protected override IEnumerable<SectionModel> CreateItems() => new[]
		{
			new SectionModel(
				typeof(AnimationTriggersPage),
				"Animation Triggers",
				"Allows to invoke animations from triggers in XAML")
		};
	}
}