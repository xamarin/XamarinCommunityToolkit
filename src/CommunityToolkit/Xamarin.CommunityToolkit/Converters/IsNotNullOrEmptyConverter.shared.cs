using System;
using System.Globalization;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts the incoming value to a <see cref="bool"/> indicating whether or not the value is not null and not empty.
	/// </summary>
	public class IsNotNullOrEmptyConverter : IsNullOrEmptyConverter
	{
		/// <summary>
		/// Converts the incoming value to a <see cref="bool"/> indicating whether or not the value is not null and not empty.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. This is not implemented.</param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>A <see cref="bool"/> indicating if the incoming value is not null and not empty.</returns>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			!(bool)base.Convert(value, targetType, parameter, culture);
	}
}