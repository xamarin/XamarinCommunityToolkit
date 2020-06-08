using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    /// <summary>
    /// Converts the incoming value to a boolean indicating whether or not the value is null or empty.
    /// </summary>
    public class ListIsNullOrEmptyConverter : IValueConverter
    {
        /// <summary>
        /// Converts the incoming value to a boolean indicating whether or not the value is null or empty.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A boolean indicating if the incoming value is null or empty.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null || (value is IEnumerable list && !list.GetEnumerator().MoveNext());

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
