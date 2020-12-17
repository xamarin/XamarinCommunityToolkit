using System.Collections.Generic;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class ConvertersGalleryViewModel : BaseGalleryViewModel
	{
		public override IEnumerable<SectionModel> Items { get; } = new List<SectionModel>
		{
			new SectionModel(
				typeof(ItemTappedEventArgsPage),
				nameof(ItemTappedEventArgsConverter),
				"A converter that allows you to extract the value from ItemTappedEventArgs that can be used in combination with EventToCommandBehavior"),

			new SectionModel(
				typeof(ItemSelectedEventArgsPage),
				nameof(ItemSelectedEventArgsConverter),
				"A converter that allows you to extract the value from ItemSelectedEventArgs that can be used in combination with EventToCommandBehavior"),

			new SectionModel(
				typeof(ByteArrayToImageSourcePage),
				nameof(ByteArrayToImageSourceConverter),
				Color.FromHex("#498205"),
				"A converter that allows you to convert byte array to an object of a type ImageSource"),

			new SectionModel(
				typeof(MultiConverterPage),
				nameof(MultiConverter),
				"This sample demonstrates how to use Multibinding Converter"),

			new SectionModel(
				typeof(DateTimeOffsetConverterPage),
				nameof(DateTimeOffsetConverter),
				"A converter that allows to convert from a DateTimeOffset type to a DateTime type"),

			new SectionModel(
				typeof(VariableMultiValueConverterPage),
				nameof(VariableMultiValueConverter),
				"A converter that allows you to combine multiple boolean bindings into a single binding."),
		};
	}
}