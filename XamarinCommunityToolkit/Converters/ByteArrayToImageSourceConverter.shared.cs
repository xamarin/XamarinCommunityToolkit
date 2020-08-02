using Microsoft.Toolkit.Xamarin.Forms.Extensions;
using System.Globalization;
using Xamarin.Forms;
using System.IO;
using System;

namespace Microsoft.Toolkit.Xamarin.Forms.Converters
{
    /// <summary>
    /// Converts the incoming value from byte array and returns the object of a type ImageSource.
    /// </summary>
    public class ByteArrayToImageSourceConverter : ValueConverterExtension, IValueConverter
    {
        /// <summary>
        /// Converts the incoming value from byte array and returns the object of a type ImageSource.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">Additional parameter for the converter to handle. Not implemented.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An object of type ImageSource.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
             => value is byte[] imageBytes
               ? ImageSource.FromStream(() => new MemoryStream(imageBytes))
               : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}