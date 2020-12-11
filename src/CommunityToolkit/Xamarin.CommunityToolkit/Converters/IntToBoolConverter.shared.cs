using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts an integer to corresponding boolean and vice versa.
	/// </summary>
	public class IntToBoolConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Converts an integer to corresponding boolean.
		/// </summary>
		/// <param name="value">Integer value.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>False if the value is zero, otherwice True.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is int result
				? result != 0
				: throw new ArgumentException("Value is not a valid integer", nameof(value));

		/// <summary>
		/// Converts back boolean to corresponding integer.
		/// </summary>
		/// <param name="value">Boolean value.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>0 if the value is False, otherwice 1.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool result)
				return result ? 1 : 0;

			throw new ArgumentException("Value is not a valid boolean", nameof(value));
		}
	}
}