using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class StateToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value is State state && parameter is State stateToCompare && state == stateToCompare;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> State.None;
	}
}
