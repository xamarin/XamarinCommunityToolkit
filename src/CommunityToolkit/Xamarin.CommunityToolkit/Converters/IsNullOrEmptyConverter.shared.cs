using System;
using System.Collections;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts the incoming value to a <see cref="bool"/> indicating whether or not the value is null or empty.
	/// </summary>
	public class IsNullOrEmptyConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// A value that specifies whether or not invert the result.
		/// </summary>
		public bool InvertCheck { get; set; }

		/// <summary>
		/// Converts the incoming value to a <see cref="bool"/> indicating whether or not the value is null or empty.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. This is not implemented.</param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>A <see cref="bool"/> indicating if the incoming value is null or empty.</returns>
		public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
			=> InvertCheck != ConvertInternal(value);

		internal static bool ConvertInternal(object? value, bool isListCheck = false)
		{
			if (value == null)
				return true;

			if (value is IEnumerable list)
				return !list.GetEnumerator().MoveNext();

			if (isListCheck)
				throw new ArgumentException("Value cannot be casted to IEnumerable or null", nameof(value));

			if (value is string str)
				return string.IsNullOrWhiteSpace(str);

			return false;
		}

		/// <summary>
		/// This method is not implemented and will throw a <see cref="NotImplementedException"/>.
		/// </summary>
		/// <param name="value">N/A</param>
		/// <param name="targetType">N/A</param>
		/// <param name="parameter">N/A</param>
		/// <param name="culture">N/A</param>
		/// <returns>N/A</returns>
		public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
			=> throw new NotImplementedException();
	}
}