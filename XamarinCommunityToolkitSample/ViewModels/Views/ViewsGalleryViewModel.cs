using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Views;
using Xamarin.CommunityToolkit.Sample.Resx;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class ViewsGalleryViewModel : BaseViewModel
	{
		public IEnumerable<SectionModel> Items { get; } = new List<SectionModel> {
			new SectionModel(typeof(AvatarViewPage), AppResources.AvatarViewTitle, Color.FromHex("#498205"), AppResources.AvatarViewDescription),
			new SectionModel(typeof(RangeSliderPage), AppResources.RangeSliderTitle, Color.Red, AppResources.RangeSliderDescription),
		};
	}
}