using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class TextCaseConverter_Tests
	{
		const string test = nameof(test);
		const string t = nameof(t);

		[TestCase(test, TextCaseType.Lower, test)]
		[TestCase(test, TextCaseType.Upper, "TEST")]
		[TestCase(test, TextCaseType.None, test)]
		[TestCase(test, TextCaseType.FirstUpperRestLower, "Test")]
		[TestCase(t, TextCaseType.Upper, "T")]
		[TestCase(t, TextCaseType.Lower, t)]
		[TestCase(t, TextCaseType.None, t)]
		[TestCase(t, TextCaseType.FirstUpperRestLower, "T")]
		[TestCase("", TextCaseType.FirstUpperRestLower, "")]
		[TestCase(null, null, null)]
		public void TextCaseConverter(object value, object comparedValue, object expectedResult)
		{
			var textCaseConverter = new TextCaseConverter();

			var result = textCaseConverter.Convert(value, typeof(TextCaseConverter_Tests), comparedValue, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCase(0)]
		[TestCase(int.MinValue)]
		[TestCase(double.MaxValue)]
		public void InValidConverterValuesThrowArgumenException(object value)
		{
			var textCaseConverter = new TextCaseConverter();

			Assert.Throws<ArgumentException>(() => textCaseConverter.Convert(value, typeof(TextCaseConverter_Tests), null, CultureInfo.CurrentCulture));
		}
	}
}