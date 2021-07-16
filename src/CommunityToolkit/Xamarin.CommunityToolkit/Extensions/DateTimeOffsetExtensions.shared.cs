using System;
using System.Globalization;

namespace Xamarin.CommunityToolkit.Extensions
{
	static class DateTimeOffsetExtensions
	{
		internal static int DaysInMonth(this DateTimeOffset dateTime)
		{
			var daysInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);

			return daysInMonth;
		}

		internal static DateTimeOffset FirstDayOfMonth(this DateTimeOffset dateTime)
		{
			var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);

			return firstDayOfMonth;
		}

		internal static int WeeksInMonth(this DateTimeOffset dateTime, DayOfWeek firstDayOfWeek)
		{
			var daysInMonth = DaysInMonth(dateTime);
			var date = new DateTime(dateTime.Year, dateTime.Month, daysInMonth);
			var lastWeekOfMonth = WeekOfMonth(date, firstDayOfWeek);

			return lastWeekOfMonth;
		}

		internal static int WeekOfMonth(this DateTimeOffset date, DayOfWeek firstDayOfWeek)
		{
			var weekOfYear = date.WeekOfYear(firstDayOfWeek);
			var weekOfYearForFirstDayOfMonth = date.FirstDayOfMonth().WeekOfYear(firstDayOfWeek);
			var weekOfMonth = weekOfYear - weekOfYearForFirstDayOfMonth + 1;

			return weekOfMonth;
		}

		internal static int DayOfWeek(this DateTimeOffset dateTime, DayOfWeek firstDayOfWeek, bool includeWeekends)
		{
			var currentDayOfWeek = firstDayOfWeek;
			var dayOfWeek = 1;
			var daysInWeek = 7;

			if (!includeWeekends)
			{
				daysInWeek = 5;
			}

			for (var i = 1; i <= daysInWeek; i++)
			{
				if (currentDayOfWeek == dateTime.DayOfWeek)
				{
					break;
				}

				currentDayOfWeek = currentDayOfWeek.NextOrFirst();

				if (!includeWeekends &&
					(currentDayOfWeek == System.DayOfWeek.Saturday || currentDayOfWeek == System.DayOfWeek.Sunday))
				{
					currentDayOfWeek = System.DayOfWeek.Monday;
				}

				dayOfWeek++;
			}

			return dayOfWeek;
		}

		internal static bool IsSameCalendarDay(this DateTimeOffset dateTimeOffset, in DateTimeOffset dateTimeOffsetToCompare)
		{
			return dateTimeOffset.Day == dateTimeOffsetToCompare.Day
					&& dateTimeOffset.Month == dateTimeOffsetToCompare.Month
					&& dateTimeOffset.Year == dateTimeOffsetToCompare.Year;
		}

		static int WeekOfYear(this DateTimeOffset dateTime, DayOfWeek firstDayOfWeek)
		{
			var weekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
				dateTime.Date,
				rule: CalendarWeekRule.FirstDay,
				firstDayOfWeek: firstDayOfWeek);

			return weekOfYear;
		}
	}
}