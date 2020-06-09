using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    public class IndexToArrayItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int index))
                throw new ArgumentException("Value is not a valid integer", nameof(value));

            if (!(parameter is Array array))
                throw new ArgumentException("Parameter is not a valid array", nameof(parameter));

            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(value), "Index was out of range");

            return array.GetValue(index);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is Array array))
                throw new ArgumentException("Parameter is not a valid array", nameof(parameter));

            for (var i = 0; i < array.Length; i++)
            {
                var item = array.GetValue(i);
                if (item != null && item.Equals(value) || item == null && value == null)
                    return i;
            }

            throw new ArgumentException("Parameter does not exist in the array", nameof(value));
        }
    }
}
