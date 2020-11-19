using System.Collections.Generic;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Effects;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Effects
{
	public class EffectsGalleryViewModel : BaseGalleryViewModel
	{
		public override IEnumerable<SectionModel> Items { get; } = new List<SectionModel>
		{
			new SectionModel(
				typeof(SafeAreaEffectPage),
				nameof(SafeAreaEffect),
				"The SafeAreaEffect is an effectwill help to make sure that content isn't clipped by rounded device corners, the home indicator, or the sensor housing on an iPhone X (or alike)"
				),

			new SectionModel(
				typeof(RemoveBorderEffectPage),
				nameof(RemoveBorderEffect),
				"The RemoveBorderEffect is an effect that will remove the border from an Entry on iOS and the underline from an entry on Android"
				),

			new SectionModel(
				typeof(SelectAllTextEffectPage),
				nameof(SelectAllTextEffect),
				"The SelectAllTextEffect is an effect that will select all text in an Entry / Editor when it becomes focussed"
				),

			new SectionModel(
				typeof(IconTintColorEffectPage),
				nameof(IconTintColorEffect),
				"With the IconTintColorEffect you set the tint color of an Image or ImageButton."
				),
		};
	}
}