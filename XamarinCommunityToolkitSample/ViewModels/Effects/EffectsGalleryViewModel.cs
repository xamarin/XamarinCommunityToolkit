using System.Collections.Generic;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Effects;
using Xamarin.CommunityToolkit.Sample.Resx;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Effects
{
	public class EffectsGalleryViewModel : BaseViewModel
	{
		public IEnumerable<SectionModel> Items { get; } = new List<SectionModel>
		{
			new SectionModel(
				typeof(SafeAreaEffectPage),
				nameof(SafeAreaEffect),
				AppResources.SafeAreaShortDescription),
			new SectionModel(
				typeof(IconTintColorEffectPage),
				nameof(IconTintColorEffect),
				"With the IconTintColorEffect you set the tint color of an Image or ImageButton."),
		};
	}
}