using System.Globalization;
using Microsoft.Toolkit.Xamarin.Forms.Converters;
using Xunit;

namespace Microsoft.Toolkit.Xamarin.Forms.UnitTests.Converters
{
    public class IsNotNullOrEmptyConverter_Tests
    {
        [Theory]
        [InlineData("Test", true)]
        [InlineData(null, false)]
        [InlineData("", false)]
        public void IsNotNullOrEmptyConverter(string value, bool expectedResult)
        {
            var isNotNullOrEmptyConverter = new IsNotNullOrEmptyConverter();

            var result = isNotNullOrEmptyConverter.Convert(value, typeof(IsNotNullOrEmptyConverter_Tests), null, CultureInfo.CurrentCulture);

            Assert.Equal(result, expectedResult);
        }
    }
}
