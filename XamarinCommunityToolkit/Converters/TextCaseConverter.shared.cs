using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Converters
{
    /// <summary>
    /// Converts text (string, char) to certain case.
    /// </summary>
    [ContentProperty(nameof(Type))]
    public class TextCaseConverter : ValueConverterMarkupExtension, IValueConverter
    {
        /// <summary>
        /// The desired text case that the text should be converted to.
        /// </summary>
        public TextCaseConverterType Type { get; set; }

        /// <summary>
        /// Converts text (string, char) to certain case.
        /// </summary>
        /// <param name="value">The text to convert.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The desired text case that the text should be converted to (TextCaseConverterType enum value).</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>The lowercase text representation.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value == null || value is string || value is char
                ? Convert(value?.ToString(), parameter)
                : throw new ArgumentException("Value is neither a string nor a char", nameof(value));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();

        object Convert(string value, object parameter)
            => GetParameter(parameter) switch
            {
                TextCaseConverterType.Lower => value?.ToLowerInvariant(),
                TextCaseConverterType.Upper => value?.ToUpperInvariant(),
                _ => value
            };

        TextCaseConverterType GetParameter(object parameter)
            => parameter == null
            ? Type
            : parameter switch
            {
                TextCaseConverterType type => type,
                string typeString => Enum.TryParse(typeString, out TextCaseConverterType result)
                    ? result
                    : throw new ArgumentException("Cannot parse text case from the string", nameof(parameter)),
                int typeInt => Enum.IsDefined(typeof(TextCaseConverterType), typeInt)
                    ? (TextCaseConverterType)typeInt
                    : throw new ArgumentException("Cannot convert integer to text case enum value", nameof(parameter)),
                _ => TextCaseConverterType.None,
            };
    }
}
