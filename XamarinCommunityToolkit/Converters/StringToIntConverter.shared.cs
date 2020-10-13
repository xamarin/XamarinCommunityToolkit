using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts The Provided string into the equivlant intger.
	/// </summary>
	public class StringToIntConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Converts The Provided string into the equivlant intger.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>The Intger value of the provided string</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is string numstr && !string.IsNullOrWhiteSpace(numstr) ? System.Convert.ToInt32(numstr) : throw new ArgumentException("Value is not a valid integer", nameof(value));

		/// <summary>
		/// Converts The Provided intger into the equivlant string.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>The string version of the provided int</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is int num ? $"{num}" : throw new ArgumentException("Value is not a valid integer", nameof(value));
	}
}
