using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is int result
            ? result != 0
            : throw new ArgumentException("Value is not a valid integer", nameof(value));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool result)
                return result ? 1 : 0;

            throw new ArgumentException("Value is not a valid boolean", nameof(value));
        }
    }
}
