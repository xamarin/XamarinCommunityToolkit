using System;
using System.Globalization;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Checks whether the incoming value doesn't equal the provided parameter.
	/// </summary>
	public class NotEqualConverter : EqualConverter
	{
		/// <summary>
		/// Checks whether the incoming value doesn't equal the provided parameter.
		/// </summary>
		/// <param name="value">The first object to compare.</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">The second object to compare.</param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>True if <paramref name="value"/> and <paramref name="parameter"/> are not equal, False if they are equal.</returns>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
			!(bool)base.Convert(value, targetType, parameter, culture);
	}
}