using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.AttachedProperties;
using Xamarin.CommunityToolkit.Sample.Resx;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.AttachedProperties
{
	public class AttachedPropertiesGalleryViewModel : BaseViewModel
	{
		public IEnumerable<SectionModel> Items { get; } = new List<SectionModel>
		{
			new SectionModel(
				typeof(ProgressBarAttachedPropertiesPage),
				"ProgressBarAttachedProperties",
				Color.FromHex("#498205"),
				AppResources.ProgressBarAttachedPropertiesShortDescription)
		};
	}
}