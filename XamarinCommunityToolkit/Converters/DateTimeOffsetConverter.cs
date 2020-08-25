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
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is DateTimeOffset dateTime)
			{
				return dateTime.DateTime;
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is DateTime date)
			{
				return date.Kind switch
				{
					DateTimeKind.Local => new DateTimeOffset(date, DateTimeOffset.Now.Offset),
					DateTimeKind.Utc => new DateTimeOffset(date, DateTimeOffset.UtcNow.Offset),
					_ => new DateTimeOffset(date, TimeSpan.Zero),
				};
			}

			return null;
		}
	}
}