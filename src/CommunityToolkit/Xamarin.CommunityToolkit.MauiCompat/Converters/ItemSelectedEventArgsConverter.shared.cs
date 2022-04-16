using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts/Extracts the incoming value from <see cref="SelectedItemChangedEventArgs"/> object and returns the value of <see cref="SelectedItemChangedEventArgs.SelectedItem"/> property from it.
	/// </summary>
	public class ItemSelectedEventArgsConverter : BaseNullableConverterOneWay<SelectedItemChangedEventArgs, object>
	{
		/// <summary>
		/// Converts/Extracts the incoming value from <see cref="SelectedItemChangedEventArgs"/> object and returns the value of <see cref="SelectedItemChangedEventArgs.SelectedItem"/> property from it.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <returns>A <see cref="SelectedItemChangedEventArgs.SelectedItem"/> object from object of type <see cref="SelectedItemChangedEventArgs"/>.</returns>
		public override object? ConvertFrom(SelectedItemChangedEventArgs? value) => value?.SelectedItem;
	}
}