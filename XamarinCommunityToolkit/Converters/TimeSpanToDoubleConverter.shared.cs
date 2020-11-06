using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	public class TimeSpanToDoubleConverter : IValueConverter
	{
		/// <summary>
		/// Converts a timespan to a double value in seconds.
		/// </summary>
		/// <param name="value">The TimeSpan to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter"></param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A double value in seconds.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is TimeSpan timespan)
			{
				return timespan.TotalSeconds;
			}

			return 1.0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double doubleValue)
			{
				return TimeSpan.FromSeconds(doubleValue);
			}

			return TimeSpan.Zero;
		}
	}
}