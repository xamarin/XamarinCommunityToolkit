using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    /// <summary>
    /// Checks whether the incoming value equals the provided parameter.
    /// </summary>
    public class EqualsConverter : IValueConverter
    {
        /// <summary>
        /// Checks whether the incoming value equals the provided parameter.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An inverted boolean from the one coming in.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value.Equals(parameter);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
