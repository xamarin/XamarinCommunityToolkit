using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Concatenates the members of a collection, using the specified separator between each member.
	/// </summary>
	public class ListToStringConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// The separator that should be between each item in the collection
		/// </summary>
		public string Separator { get; set; }

		/// <summary>
		/// Concatenates the items of a collection, using the specified separator between each item.
		/// </summary>
		/// <param name="value">The collection to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The separator that should be between each collection item.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>Concatenated members string.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return string.Empty;

			if (!(value is IEnumerable enumerable))
				throw new ArgumentException("Value cannot be casted to IEnumerable", nameof(value));

			if (!((parameter ?? Separator ?? string.Empty) is string separator))
				throw new ArgumentException("Parameter cannot be casted to string", nameof(parameter));

			var collection = enumerable
				.OfType<object>()
				.Select(x => x.ToString())
				.Where(x => !string.IsNullOrWhiteSpace(x));

			return string.Join(separator, collection);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}
}