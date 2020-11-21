using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts DateTimeFormat to DateTime and back
	/// </summary>
	public class DateTimeOffsetConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Converts DatetimeOffset to DateTime
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter. Not used.</param>
		/// <returns>The DateTime value.</returns>
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			=> value is DateTimeOffset dateTimeOffset
				? dateTimeOffset.DateTime
				: throw new ArgumentException("Value is not a valid DateTimeOffset", nameof(value));

		/// <summary>
		/// Converts Datetime back to DateTimeOffset
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter. Not used.</param>
		/// <returns>The DateTimeOffset value.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			=> value is DateTime dateTime
				? dateTime.Kind switch
				{
					DateTimeKind.Local => new DateTimeOffset(dateTime, DateTimeOffset.Now.Offset),
					DateTimeKind.Utc => new DateTimeOffset(dateTime, DateTimeOffset.UtcNow.Offset),
					_ => new DateTimeOffset(dateTime, TimeSpan.Zero),
				}
				: throw new ArgumentException("Value is not a valid DateTime", nameof(value));
	}
}