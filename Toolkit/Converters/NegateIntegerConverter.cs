using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Xamarin.Toolkit.Converters
{
    /// <summary>
    /// Negates an integer.
    /// </summary>
    public class NegateIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Invert(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Invert(value);
        }

        int Invert(object value)
        {
            var integer = 0;

            try
            {
                integer = (int)value;
            }
            catch
            {
                return 0;
            }

            return (int)value * -1;
        }
    }
}
