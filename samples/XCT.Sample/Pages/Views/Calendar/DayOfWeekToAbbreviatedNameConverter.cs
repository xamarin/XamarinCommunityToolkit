using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public class DayOfWeekToAbbreviatedNameConverter : IValueConverter
	{
		public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
		{
			if (value == null)
				throw new ArgumentNullException(nameof(value));

			var dayOfWeek = (DayOfWeek)value;
			var abbreviatedDayName = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames[(int)dayOfWeek];

			return abbreviatedDayName;
		}

		public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture) => throw new NotImplementedException();
	}
}