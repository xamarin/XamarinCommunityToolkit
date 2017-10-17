using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace FormsCommunityToolkit.Converters
{
    /// <summary>
    /// Conversion values using the sequential conversion of each of the converters from the first to the last.
    /// </summary>
    [ValueConversion(typeof(IEnumerator<IValueConverter>), typeof(IEnumerator<IValueConverter>))]
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        public static ValueConverterGroup Instance { get; } = new ValueConverterGroup();

        /// <summary>
        /// Convert the value using a sequence of converters, from first to last.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>Result of executing a sequence of converters.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
