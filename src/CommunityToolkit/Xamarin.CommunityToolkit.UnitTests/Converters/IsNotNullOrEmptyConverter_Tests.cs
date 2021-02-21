using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class IsNotNullOrEmptyConverter_Tests
	{
		[Theory]
		[InlineData("Test", true)]
		[InlineData(typeof(IsNotNullOrEmptyConverter), true)]
		[InlineData(null, false)]
		[InlineData("", false)]
		public void IsNotNullOrEmptyConverter(object value, bool expectedResult)
		{
			var isNotNullOrEmptyConverter = new IsNotNullOrEmptyConverter();

			var result = isNotNullOrEmptyConverter.Convert(value, typeof(IsNotNullOrEmptyConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.Equal(result, expectedResult);
		}
	}
}