using System;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class CalendarDayUpdatedEventArgs : EventArgs
	{
		/// <summary>
		/// The <see cref="CalendarDay"/> that was updated.
		/// </summary>
		public CalendarDay CalendarDay { get; }

		public CalendarDayUpdatedEventArgs(CalendarDay calendarDay)
		{
			CalendarDay = calendarDay;
		}
	}
}