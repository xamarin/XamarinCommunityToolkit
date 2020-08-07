using System.Collections.Generic;
using Xamarin.Forms;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.Views;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Resx;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Views
{
    public class ViewsGalleryViewModel : BaseViewModel
    {
        public IEnumerable<SectionModel> Items { get; } = new List<SectionModel> {
            new SectionModel(typeof(AvatarViewPage), AppResources.AvatarViewTitle, Color.Default, AppResources.AvatarViewDescription),
            new SectionModel(typeof(RangeSliderPage), AppResources.RangeSliderTitle, Color.Default, AppResources.RangeSliderDescription),
            new SectionModel(typeof(SideMenuViewPage), AppResources.SideMenuViewTitle, Color.Default, AppResources.SideMenuViewDescription),
        };
    }
}
