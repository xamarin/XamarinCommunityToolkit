using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts The Provided string into the equivalent Double.
	/// </summary>
	public class StringToDoubleConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Converts The Provided string into the equivalent double.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>The double value of the provided string</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> double.TryParse(value as string, out var doubleValue)
				? doubleValue
				: throw new ArgumentException("Value is not a valid double", nameof(value));

		/// <summary>
		/// Converts The Provided double into the equivalent string.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>The string version of the provided double</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is double doubleValue
				? doubleValue.ToString()
				: throw new ArgumentException("Value is not a valid double", nameof(value));
	}
}
