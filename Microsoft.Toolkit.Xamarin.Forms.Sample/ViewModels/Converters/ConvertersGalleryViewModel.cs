using System.Collections.Generic;
using Xamarin.Forms;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Models;
using Microsoft.Toolkit.Xamarin.Forms.Sample.Pages.Converters;

namespace Microsoft.Toolkit.Xamarin.Forms.Sample.ViewModels.Converters
{
    public class ConvertersGalleryViewModel : BaseViewModel
    {
        public IEnumerable<SectionModel> Items { get; } = new List<SectionModel> {
            new SectionModel(
                typeof(ItemTappedEventArgsPage),
                "ItemTappedEventArgs",
                Color.FromHex("#498205"),
                "A converter that allows you to extract the value from ItemTappedEventArgs that can be used in combination with EventToCommandBehavior."
            ),

            new SectionModel(
                typeof(ItemSelectedEventArgsPage),
                "ItemSelectedEventArgs",
                Color.FromHex("#498205"),
                "A converter that allows you to extract the value from ItemSelectedEventArgs that can be used in combination with EventToCommandBehavior."
            )
        };
    }
}
