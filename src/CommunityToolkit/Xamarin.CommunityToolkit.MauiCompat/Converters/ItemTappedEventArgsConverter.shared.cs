using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts/Extracts the incoming value from <see cref="ItemTappedEventArgs"/> object and returns the value of <see cref="ItemTappedEventArgs.Item"/> property from it.
	/// </summary>
	public class ItemTappedEventArgsConverter : BaseNullableConverterOneWay<ItemTappedEventArgs, object>
	{
		/// <summary>
		/// Converts/Extracts the incoming value from <see cref="ItemTappedEventArgs"/> object and returns the value of <see cref="ItemTappedEventArgs.Item"/> property from it.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A <see cref="ItemTappedEventArgs.Item"/> object from object of type <see cref="ItemTappedEventArgs"/>.</returns>
		public override object? ConvertFrom(ItemTappedEventArgs? value) => value?.Item;
	}
}