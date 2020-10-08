using System.Collections.Generic;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Views;
using Xamarin.CommunityToolkit.Sample.Resx;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class ViewsGalleryViewModel : BaseViewModel
	{
		public IEnumerable<SectionModel> Items { get; } = new List<SectionModel>
		{
			new SectionModel(typeof(AvatarViewPage), AppResources.AvatarViewTitle, AppResources.AvatarViewDescription),
			new SectionModel(typeof(GravatarImagePage), AppResources.GravatarImageSourceTitle, AppResources.GravatarImageSourceDescription),
			new SectionModel(typeof(RangeSliderPage), AppResources.RangeSliderTitle, AppResources.RangeSliderDescription),
			new SectionModel(typeof(SideMenuViewPage), AppResources.SideMenuViewTitle, AppResources.SideMenuViewDescription),
			new SectionModel(typeof(CameraViewPage), AppResources.CameraViewTitle, AppResources.CameraViewDescription),
			new SectionModel(typeof(ExpanderPage), AppResources.ExpanderTitle, AppResources.ExpanderDescription),
			new SectionModel(typeof(ActionsPage), AppResources.ActionsPageTitle, AppResources.ActionsPageDescription),
			new SectionModel(typeof(MediaElementPage), AppResources.MediaElementTitle, AppResources.MediaElementDescription),
			new SectionModel(typeof(RangeSliderPage), AppResources.RangeSliderTitle, AppResources.RangeSliderDescription),
			new SectionModel(typeof(SideMenuViewPage), AppResources.SideMenuViewTitle, AppResources.SideMenuViewDescription)
		};
	}
}