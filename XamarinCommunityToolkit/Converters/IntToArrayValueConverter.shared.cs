using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    public class IntToArrayValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var array = (Array)parameter;
            return array.GetValue((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var array = (Array)parameter;
            for (var i = 0; i < array.Length; i++)
            {
                var item = array.GetValue(i);
                if (item == null && value == null || (item != null && item.Equals(value)))
                {
                    return i;
                }
            }
            throw new ArgumentException();
        }
    }
}
