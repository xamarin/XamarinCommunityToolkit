using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    public class BoolToObjectConverter<T> : IValueConverter
    {
        public T TrueObject { set; get; }

        public T FalseObject { set; get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value ? TrueObject : FalseObject;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value == null && TrueObject == null || ((T)value).Equals(TrueObject);
    }
}
