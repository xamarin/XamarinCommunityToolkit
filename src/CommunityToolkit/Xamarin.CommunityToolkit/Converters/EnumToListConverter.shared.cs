using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// A converter that retrieves an array of the values of the constants in a specified enumeration.
	/// </summary>
	public class EnumToListConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Retrieves an array of the values of the constants in a specified enumeration.
		/// </summary>
		/// <param name="value">The Enum.</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. This is not implemented.</param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>An array that contains the values of the constants in <paramref name="value"/>.</returns>
		public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			if (value is Type type)
				return Enum.GetValues(type);
			return value;
		}

		/// <summary>
		/// This method is not implemented and will throw a <see cref="NotImplementedException"/>.
		/// </summary>
		/// <param name="value">N/A</param>
		/// <param name="targetType">N/A</param>
		/// <param name="parameter">N/A</param>
		/// <param name="culture">N/A</param>
		/// <returns>N/A</returns>
		public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
			=> throw new NotImplementedException();
	}
}