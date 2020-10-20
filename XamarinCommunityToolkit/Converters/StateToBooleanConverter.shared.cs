using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xamarin.CommunityToolkit.Converters
{
	public class StateToBooleanConverter : IValueConverter
	{
		public State StateToCompare { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is State state))
				throw new ArgumentException("Value is not a valid State", nameof(value));

			if (parameter is State stateToCompare)
				return state == stateToCompare;

			return state == StateToCompare;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}
}
