using System;
using System.Globalization;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	public class StateToBooleanConverter : IValueConverter
	{
		public LayoutState StateToCompare { get; set; }

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is LayoutState state))
				throw new ArgumentException("Value is not a valid State", nameof(value));

			if (parameter is LayoutState stateToCompare)
				return state == stateToCompare;

			return state == StateToCompare;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}
}