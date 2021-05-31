using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public class DateTimeOffsetToDayConverter : IValueConverter
	{
		public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			if (value == null)
				return string.Empty;

			var dateTime = (DateTimeOffset)value;
			var day = dateTime.Day.ToString();

			return day;
		}

		public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) => throw new NotImplementedException();
	}
}