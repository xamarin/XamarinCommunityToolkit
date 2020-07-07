using System.Globalization;
using XamarinCommunityToolkit.Converters;
using Xunit;

namespace XamarinCommunityToolkit.UnitTests.Converters
{
    public class NotEqualConverter_Tests
    {
        [Theory]
        [InlineData(true, true, false)]
        [InlineData(int.MaxValue, int.MinValue, true)]
        [InlineData("Test", true, true)]
        [InlineData(null, null, false)]
        public void NotEqualConverter(object value, object comparedValue, bool expectedResult)
        {
            var notEqualConverter = new NotEqualConverter();

            var result = notEqualConverter.Convert(value, typeof(NotEqualConverter_Tests), comparedValue, CultureInfo.CurrentCulture);

            Assert.Equal(result, expectedResult);
        }
    }
}
