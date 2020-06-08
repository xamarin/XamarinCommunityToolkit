using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    public class DoubleToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (int)Math.Round((double)value * GetParameter(parameter));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => (int)value / GetParameter(parameter);

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
