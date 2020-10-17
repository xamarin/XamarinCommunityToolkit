using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Behaviors;
using Xamarin.CommunityToolkit.Sample.Pages.Converters;
using Xamarin.CommunityToolkit.Sample.Pages.Effects;
using Xamarin.CommunityToolkit.Sample.Pages.Extensions;
using Xamarin.CommunityToolkit.Sample.Pages.TestCases;
using Xamarin.CommunityToolkit.Sample.Pages.Views;
using Xamarin.CommunityToolkit.Sample.Resx;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels
{
	public class WelcomeViewModel : BaseViewModel
	{
		public IEnumerable<SectionModel> Items { get; } = new List<SectionModel>
		{
			new SectionModel(typeof(BehaviorsGalleryPage), AppResources.BehaviorsTitle, Color.FromHex("#8E8CD8"), AppResources.BehaviorsDescription),
			new SectionModel(typeof(ConvertersGalleryPage), AppResources.ConvertersTitle, Color.FromHex("#EA005E"), AppResources.ConvertersDescription),
			new SectionModel(typeof(ExtensionsGalleryPage), AppResources.ExtensionsTitle, Color.FromHex("#00CC6A"), AppResources.ExtensionsDescription),
			new SectionModel(typeof(TestCasesGalleryPage), AppResources.TestCasesTitle, Color.FromHex("#FF8C00"), AppResources.TestCasesDescription),
			new SectionModel(typeof(ViewsGalleryPage), AppResources.ViewsTitle, Color.FromHex("#EF6950"), AppResources.ViewsDescription),
			new SectionModel(typeof(EffectsGalleryPage), AppResources.EffectsTitle, Color.FromHex("#EFB950"), AppResources.Effects_Description)
		};
	}
}