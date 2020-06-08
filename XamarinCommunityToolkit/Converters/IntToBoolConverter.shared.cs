using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (int)value != 0;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value ? 1 : 0;
    }
}
