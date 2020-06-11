using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    /// <summary>
    /// Converts text (string, char) to upper case.
    /// </summary>
    public class UpperCaseTextConverter : IValueConverter
    {
        /// <summary>
        /// Converts text (string, char) to upper case.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The uppercase text representation.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value == null || value is string || value is char
                ? value?.ToString().ToUpperInvariant()
                : throw new ArgumentException("Value is neither a string nor a char", nameof(value));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
