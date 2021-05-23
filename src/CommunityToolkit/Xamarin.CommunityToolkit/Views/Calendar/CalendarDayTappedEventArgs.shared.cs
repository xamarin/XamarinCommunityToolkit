using System;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class CalendarDayTappedEventArgs : EventArgs
	{
		/// <summary>
		/// The <see cref="CalendarDay"/> that was tapped.
		/// </summary>
		public CalendarDay CalendarDay { get; }

		public CalendarDayTappedEventArgs(CalendarDay calendarDay)
		{
			CalendarDay = calendarDay;
		}
	}
}