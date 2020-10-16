using System;
using System.Globalization;

namespace Xamarin.Forms.StateSquid
{
	public class StateToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is State state && parameter is State stateToCompare && state == stateToCompare;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> State.None;
	}
}
