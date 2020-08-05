using System.Collections.Generic;
using Xamarin.Forms;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.Converters;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Resx;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Converters
{
    public class ConvertersGalleryViewModel : BaseViewModel
    {
        public IEnumerable<SectionModel> Items { get; } = new List<SectionModel> {
            new SectionModel(
                typeof(ItemTappedEventArgsPage),
                "ItemTappedEventArgs",
                Color.FromHex("#498205"),
                AppResources.ItemTappedEventArgsShortDescription
            ),

            new SectionModel(
                typeof(ItemSelectedEventArgsPage),
                "ItemSelectedEventArgs",
                Color.FromHex("#498205"),
                AppResources.ItemTappedSelectedEventArgsShortDescription
            )
        };
    }
}
