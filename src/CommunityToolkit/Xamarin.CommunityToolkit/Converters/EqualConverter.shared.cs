using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Checks whether the incoming value equals the provided parameter.
	/// </summary>
	public class EqualConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// Checks whether the incoming value equals the provided parameter.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>An inverted boolean from the one coming in.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> (value != null && value.Equals(parameter)) || (value == null && parameter == null);

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}
}