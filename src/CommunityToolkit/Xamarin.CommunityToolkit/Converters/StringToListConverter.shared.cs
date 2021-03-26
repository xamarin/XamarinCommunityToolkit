using System;
using System.Globalization;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Returns a string array that contains the substrings in this string that are delimited by <see cref="Separator"/>.
	/// </summary>
	public class StringToListConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// A string that delimits the substrings in this string.
		/// </summary>
		public string Separator { get; set; } = string.Empty;

		/// <summary>
		/// Returns a string array that contains the substrings in this string that are delimited by <see cref="Separator"/>.
		/// </summary>
		/// <param name="value">The string to split.</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">A string that delimits the substrings in this string. This overrides the value in <see cref="Separator"/>.</param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>An array whose elements contain the substrings in this string that are delimited by <see cref="Separator"/> or, if set, <paramref name="parameter"/>.</returns>
		public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			if (value == null)
				return Enumerable.Empty<string>();

			if (value is not string enumerable)
				throw new ArgumentException("Value cannot be casted to string", nameof(value));

			if ((parameter ?? Separator ?? string.Empty) is not string separator)
				throw new ArgumentException("Parameter cannot be casted to string", nameof(parameter));

			return enumerable.Split(new[] { separator }, StringSplitOptions.None);
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