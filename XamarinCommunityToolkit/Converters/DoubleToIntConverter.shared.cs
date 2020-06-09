using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    public class DoubleToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is double result
            ? (int)Math.Round(result * GetParameter(parameter))
            : throw new ArgumentException("Value is not a valid double", nameof(value));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is int result
            ? result / GetParameter(parameter)
            : throw new ArgumentException("Value is not a valid integer", nameof(value));

        double GetParameter(object parameter)
            => parameter switch
            {
                double d => d,
                int i => i,
                string s => double.Parse(s),
                _ => 1,
            };
    }
}
