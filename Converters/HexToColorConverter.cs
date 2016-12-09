using System;
using Xamarin.Forms;
using System.Globalization;

namespace FormsCommunityToolkit.Converters
{
    /// <summary>
    /// Hex to color converter.
    /// </summary>
    public class HexToColorConverter : IValueConverter
    {

        public static HexToColorConverter Instance { get; } = new HexToColorConverter();

        /// <summary>
        /// Init this instance.
        /// </summary>
        public static void Init()
        {
            var time = DateTime.UtcNow;
        }

        public Color DefaultColor = Color.FromHex("#3498db");

        /// <param name="value">To be added.</param>
        /// <param name="targetType">To be added.</param>
        /// <param name="parameter">To be added.</param>
        /// <param name="culture">To be added.</param>
        /// <summary>
        /// Convert the specified value, targetType, parameter and culture.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = value as string;
            try
            {
                return string.IsNullOrEmpty(color) ? DefaultColor : Color.FromHex(color);
            }
            catch
            {
                return DefaultColor;
            }
        }

        /// <param name="value">To be added.</param>
        /// <param name="targetType">To be added.</param>
        /// <param name="parameter">To be added.</param>
        /// <param name="culture">To be added.</param>
        /// <summary>
        /// Converts the back.
        /// </summary>
        /// <returns>The back.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;

    }
}

