using System;
using System.Globalization;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	public class BoolReverseConverter : IValueConverter
	{
		/// <summary>
		/// Converts a Boolean to a its opposite value.
		/// </summary>
		/// <param name="value">The Boolean to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Given parameter.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>A bool value.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool val)
			{
				return !val;
			}

			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
			throw new NotImplementedException();
	}
}