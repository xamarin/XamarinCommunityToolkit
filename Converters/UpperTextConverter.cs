using System;
using Xamarin.Forms;
using System.Globalization;

namespace FormsCommunityToolkit.Converters
{
    public class UpperTextConverter : IValueConverter
    {
        public static UpperTextConverter Instance { get; } = new UpperTextConverter();

        /// <summary>
        /// Init this instance.
        /// </summary>
        public static void Init()
        {
            var time = DateTime.UtcNow;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            var text = ((string)value);

            return text.ToUpperInvariant();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

