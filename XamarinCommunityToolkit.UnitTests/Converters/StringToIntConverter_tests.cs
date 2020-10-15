using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class StringToIntConverter_tests
	{
		[Theory]
		[InlineData("2", 2)]
		[InlineData("255", 255)]
		[InlineData("99191", 99191)]
		public void StringToIntConverter(string value, int expectedResult)
		{
			var stringToIntConverter = new StringToIntConverter();

			var result = stringToIntConverter.Convert(value, typeof(StringToIntConverter_tests), null, CultureInfo.CurrentCulture);

			Assert.Equal(result, expectedResult);
		}

		[Theory]
		[InlineData(2, "2")]
		[InlineData(255, "255")]
		[InlineData(99191, "99191")]
		public void IntToStringConverter(int value, string expectedResult)
		{
			var stringToIntConverter = new StringToIntConverter();

			var result = stringToIntConverter.ConvertBack(value, typeof(StringToIntConverter_tests), null,
				CultureInfo.CurrentCulture);

			Assert.Equal(result, expectedResult);
		}

		[Theory]
		[InlineData(0)]
		[InlineData("ABC")]
		[InlineData("Ac123")]
		public void InValidConverterValuesThrowArgumenException(object value)
		{
			var stringToDoubleConverter = new StringToDoubleConverter();

			Assert.Throws<ArgumentException>(() => stringToDoubleConverter.Convert(value, typeof(StringToIntConverter_tests), null, CultureInfo.CurrentCulture));
		}
	}
}
