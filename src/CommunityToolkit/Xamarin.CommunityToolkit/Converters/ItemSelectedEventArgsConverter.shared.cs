using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts/Extracts the incoming value from SelectedItemChangedEventArgs object and returns the value of SelectedItem property from it.
	/// </summary>
	public class ItemSelectedEventArgsConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Converts/Extracts the incoming value from SelectedItemChangedEventArgs object and returns the value of SelectedItem property from it.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A SelectedItem object from object of type SelectedItemChangedEventArgs.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		   => value is SelectedItemChangedEventArgs selectedItemChangedEventArgs
			   ? selectedItemChangedEventArgs.SelectedItem
			   : throw new ArgumentException("Expected value to be of type SelectedItemChangedEventArgs", nameof(value));

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}
}