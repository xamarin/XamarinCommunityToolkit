using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// The <see cref="CalendarWeekDayHeader"/> represents specific week day in top header of <see cref="Calendar"/>.
	/// </summary>
	public class CalendarWeekDayHeader : TemplatedView
	{
		/// <summary>
		/// Gets day of week.
		/// </summary>
		public DayOfWeek DayOfWeek { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="CalendarWeekDayHeader"/> class.
		/// </summary>
		/// <param name="dayOfWeek">day of the week</param>
		public CalendarWeekDayHeader(DayOfWeek dayOfWeek)
		{
			DayOfWeek = dayOfWeek;
			var a = DayOfWeek; // Without this line ControlTemplate doesn't work.
		}
	}
}