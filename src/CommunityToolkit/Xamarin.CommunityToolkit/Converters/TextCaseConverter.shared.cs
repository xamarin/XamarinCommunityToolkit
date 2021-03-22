using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts text (string, char) to certain case as specified with <see cref="Type"/> or the parameter of the Convert method.
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
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">The desired text case that the text should be converted to. Must match <see cref="TextCaseType"/> enum value.</param>
		/// <param name="culture">The culture to use in the converter. This is not implemented.</param>
		/// <returns>The converted text representation with the desired casing.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> value == null || value is string || value is char
				? Convert(value?.ToString(), parameter)
				: throw new ArgumentException("Value is neither a string nor a char", nameof(value));

		/// <summary>
		/// This method is not implemented and will throw a <see cref="NotImplementedException"/>.
		/// </summary>
		/// <param name="value">N/A</param>
		/// <param name="targetType">N/A</param>
		/// <param name="parameter">N/A</param>
		/// <param name="culture">N/A</param>
		/// <returns>N/A</returns>
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