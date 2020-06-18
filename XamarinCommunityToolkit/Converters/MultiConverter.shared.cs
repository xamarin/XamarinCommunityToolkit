using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    /// <summary>
    /// Converts an incoming value using all of the incoming converters in sequence.
    /// </summary>
    public class MultiConverter : List<IValueConverter>, IValueConverter
    {
        /// <summary>
        /// Uses the incoming converters to convert the value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Parameter to pass into subsequent converters.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The converted value.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // If it's a list of MultiConverterParameter, use those and propagate them into the converters.
            if (parameter is IList<MultiConverterParameter> parameters)
            {
                return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType,
                    parameters.FirstOrDefault(x => x.ConverterType == converter.GetType())?.Value, culture));
            }

            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => throw new NotImplementedException();
    }

    /// <summary>
    /// Represents a parameter to be used in the MultiConverter.
    /// </summary>
    public class MultiConverterParameter : BindableObject
    {
        public Type ConverterType { get; set; }

        public object Value { get; set; }
    }
}
