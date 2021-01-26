using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts text (string, char) to certain case.
	/// </summary>
	[ContentProperty(nameof(Type))]
	public class TextCaseConverter : ValueConverterExtension, IValueConverter
	{
		/// <summary>
		/// The desired text case that the text should be converted to.
		/// </summary>
		public TextCaseType Type { get; set; }

		/// <summary>
		/// Converts text (string, char) to certain case.
		/// </summary>
		/// <param name="value">The text to convert.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The desired text case that the text should be converted to (TextCaseType enum value).</param>
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
				TextCaseType.Lower => value?.ToLowerInvariant(),
				TextCaseType.Upper => value?.ToUpperInvariant(),
				_ => value
			};

		TextCaseType GetParameter(object parameter)
			=> parameter == null
			? Type
			: parameter switch
			{
				TextCaseType type => type,
				string typeString => Enum.TryParse(typeString, out TextCaseType result)
					? result
					: throw new ArgumentException("Cannot parse text case from the string", nameof(parameter)),
				int typeInt => Enum.IsDefined(typeof(TextCaseType), typeInt)
					? (TextCaseType)typeInt
					: throw new ArgumentException("Cannot convert integer to text case enum value", nameof(parameter)),
				_ => TextCaseType.None,
			};
	}
}