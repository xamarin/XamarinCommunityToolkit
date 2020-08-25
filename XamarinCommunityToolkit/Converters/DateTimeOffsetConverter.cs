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
				if (date == DateTime.MinValue) date = date.Add(DateTimeOffset.Now.Offset);
				return new DateTimeOffset(date);
			}

			return null;
		}
	}
}