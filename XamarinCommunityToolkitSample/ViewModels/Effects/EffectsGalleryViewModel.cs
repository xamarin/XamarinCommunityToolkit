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
				"The SafeAreaEffect is an effectwill help to make sure that content isn't clipped by rounded device corners, the home indicator, or the sensor housing on an iPhone X (or alike)")
		};
	}
}