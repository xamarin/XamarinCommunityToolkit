using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    public class BoolToObjectConverter<TObject> : IValueConverter
    {
        public TObject TrueObject { set; get; }

        public TObject FalseObject { set; get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool result)
                return result ? TrueObject : FalseObject;

            throw new ArgumentException("Value is not a valid boolean", nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TObject result)
                return result.Equals(TrueObject);

            if (default(TObject) == null && value == null && TrueObject == null)
                return true;

            throw new ArgumentException($"Value is not a valid {typeof(TObject).Name}", nameof(value));
        }
    }
}
