using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    /// <summary>
    /// Converts true to false and false to true.
    /// </summary>
    public class InvertedBoolConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean to its inverse value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An inverted boolean from the one coming in.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !(value as bool?);

        /// <summary>
        /// Converts a boolean to its inverse value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An inverted boolean from the one coming in.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => !(value as bool?);
    }
}
