using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace Xamarin.Toolkit.Converters
{
    [ValueConversion(typeof(object), typeof(bool))]
    public class HasDataConverter : IValueConverter
    {
        public static HasDataConverter Instance { get; } = new HasDataConverter();

        /// <summary>
        /// Init this instance.
        /// </summary>
        public static void Init()
        {
            var time = DateTime.UtcNow;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if null then not visible
            if (value == null)
                return false;

            // if empty string then not visible
            if (value is string)
                return !string.IsNullOrWhiteSpace((string)value);

            // if blank list not visible
            if (value is IList)
                return ((IList)value).Count > 0;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
