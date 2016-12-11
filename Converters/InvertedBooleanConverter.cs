using System;
using Xamarin.Forms;
using System.Globalization;

namespace FormsCommunityToolkit.Converters
{
    /// <summary>
    /// Inverted boolen converter.
    /// </summary>
    [ValueConversion (typeof (bool), typeof (bool))]
    public class InvertedBooleanConverter : IValueConverter
    {

        public static InvertedBooleanConverter Instance { get; } = new InvertedBooleanConverter();

        /// <summary>
        /// Init this instance.
        /// </summary>
        public static void Init()
        {
            var time = DateTime.UtcNow;
        }

        /// <param name="value">To be added.</param>
        /// <param name="targetType">To be added.</param>
        /// <param name="parameter">To be added.</param>
        /// <param name="culture">To be added.</param>
        /// <summary>
        /// Convert the specified value, targetType, parameter and culture.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
                return !(bool)value;

            return value;
        }

        /// <param name="value">To be added.</param>
        /// <param name="targetType">To be added.</param>
        /// <param name="parameter">To be added.</param>
        /// <param name="culture">To be added.</param>
        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <returns>The back.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

