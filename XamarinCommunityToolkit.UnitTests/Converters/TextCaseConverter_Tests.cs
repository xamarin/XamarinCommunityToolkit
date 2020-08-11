using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class TextCaseConverter_Tests
	{
		const string test = nameof(test);
		const string t = nameof(t);

		[Theory]
		[InlineData(test, TextCaseType.Lower, test)]
		[InlineData(test, TextCaseType.Upper, "TEST")]
		[InlineData(test, TextCaseType.None, test)]
		[InlineData(t, TextCaseType.Upper, "T")]
		[InlineData(t, TextCaseType.Lower, t)]
		[InlineData(t, TextCaseType.None, t)]
		[InlineData(null, null, null)]
		public void TextCaseConverter(object value, object comparedValue, object expectedResult)
		{
			var textCaseConverter = new TextCaseConverter();

			var result = textCaseConverter.Convert(value, typeof(TextCaseConverter_Tests), comparedValue, CultureInfo.CurrentCulture);

			Assert.Equal(result, expectedResult);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(int.MinValue)]
		[InlineData(double.MaxValue)]
		public void InValidConverterValuesThrowArgumenException(object value)
		{
			var textCaseConverter = new TextCaseConverter();

			Assert.Throws<ArgumentException>(() => textCaseConverter.Convert(value, typeof(TextCaseConverter_Tests), null, CultureInfo.CurrentCulture));
		}
	}
}