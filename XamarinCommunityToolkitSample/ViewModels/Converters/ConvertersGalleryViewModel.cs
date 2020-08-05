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
            ),

            new SectionModel(
                typeof(ByteArrayToImageSourcePage),
                "ByteArrayToImageSource",
                Color.FromHex("#498205"),
                "A converter that allows you to convert byte array to an object of a type ImageSource."
            )
        };
    }
}
