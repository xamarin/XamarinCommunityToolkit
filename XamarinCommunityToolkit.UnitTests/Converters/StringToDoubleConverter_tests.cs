using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class StringToDoubleConverter_tests
	{
		[Theory]
		[InlineData("2.2", 2.2)]
		[InlineData("3.9", 3.9)]
		[InlineData("255.98", 255.98)]
		[InlineData("99191.98765", 99191.98765)]
		public void StringToDoubleConverter(string value, double expectedResult)
		{
			var stringToDoubleConverter = new StringToDoubleConverter();

			var result = stringToDoubleConverter.Convert(value, typeof(StringToIntConverter_tests), null,
				CultureInfo.CurrentCulture);

			Assert.Equal(result, expectedResult);
		}

		[Theory]
		[InlineData(2.2, "2.2")]
		[InlineData(3.9, "3.9")]
		[InlineData(255.98, "255.98")]
		[InlineData(99191.98765, "99191.98765")]
		public void DoubleToStringConverter(double value, string expectedResult)
		{
			var doubletostring = new StringToDoubleConverter();

			var result = doubletostring.ConvertBack(value, typeof(StringToIntConverter_tests), null,
				CultureInfo.CurrentCulture);

			Assert.Equal(result, expectedResult);
		}
	}
}