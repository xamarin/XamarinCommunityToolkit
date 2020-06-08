using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    public class BoolToArrayValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var array = (Array)parameter;
            return (bool)value ? array.GetValue(1) : array.GetValue(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var array = (Array)parameter;
            if (Equals(array, 0, value))
            {
                return false;
            }
            if (Equals(array, 1, value))
            {
                return true;
            }
            throw new ArgumentException();
        }

        bool Equals(Array array, int index, object value)
        {
            var item = array.GetValue(index);
            return item == null && value == null || (item != null && item.Equals(value));
        }
    }
}
