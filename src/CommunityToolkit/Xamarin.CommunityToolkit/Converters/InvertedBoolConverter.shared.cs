using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts true to false and false to true.
	/// </summary>
	public class InvertedBoolConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Converts a boolean to its inverse value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>An inverted boolean from the one coming in.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> InverseBool(value);

		/// <summary>
		/// Converts a boolean to its inverse value.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>An inverted boolean from the one coming in.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> InverseBool(value);

		/// <summary>
		/// Inverses an incoming boolean.
		/// </summary>
		/// <param name="value">The value to inverse.</param>
		/// <returns>The inverted value of the incoming boolean.</returns>
		bool InverseBool(object value)
		{
			if (value is bool result)
				return !result;

			throw new ArgumentException("Value is not a valid boolean", nameof(value));
		}
	}
}