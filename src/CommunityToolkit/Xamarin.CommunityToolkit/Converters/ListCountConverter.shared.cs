using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts the incoming value to an <see cref="int"/> indicating the number of elements in a sequence.
	/// </summary>
	public class ListCountConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Converts the incoming value to an <see cref="int"/> indicating the number of elements in a sequence.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. This is not implemented.</param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>An <see cref="int"/> indicating the number of elements in a sequence.</returns>
		public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture) => ConvertInternal(value);

		internal static int ConvertInternal(object? value)
		{
			if (value == null)
				return 0;

			if (value is IEnumerable list)
				return list.Cast<object>().Count();

			throw new ArgumentException("Value is not a valid IEnumerable or null", nameof(value));
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