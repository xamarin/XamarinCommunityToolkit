using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.Toolkit.Converters
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
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property. Used for all converters.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic all converters.</param>
        /// <param name="culture">The language of the conversion. Used for all converters.</param>
        /// <returns>The value to be passed to the target dependency property. Result of executing a sequence of converters.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
