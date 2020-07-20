using System.Collections.Generic;
using Xamarin.Forms;
using XamarinCommunityToolkitSample.Models;
using XamarinCommunityToolkitSample.Pages.Converters;

namespace XamarinCommunityToolkitSample.ViewModels.Converters
{
    public class ConvertersGalleryViewModel : BaseViewModel
    {
        public IEnumerable<SectionModel> Items { get; } = new List<SectionModel> {
            new SectionModel(
                typeof(ItemTappedEventArgsPage),
                "ItemTappedEventArgs",
                Color.FromHex("#498205"),
                "A converter that allows you to extract the value from ItemTappedEventArgs that can be used in combination with EventToCommandBehavior."
            )
        };
    }
}
