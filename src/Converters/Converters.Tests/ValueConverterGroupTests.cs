using System.Globalization;
using NUnit.Framework;
using XamarinCommunityToolkit.Converters;
using Converters.Tests.Mocks;

namespace Converters.Tests
{
    [TestFixture]
    public class ValueConverterGroupTests
    {
        ValueConverterGroup converter;

        [SetUp]
        public void Setup()
        {
            converter = new ValueConverterGroup();
        }

        [TestCase("Test_Value")]
        public void ConvertList(object value)
        {
            var expectedResult = value.ToString();

            for (int i = 0; i < 10; i++)
            {
                var c = new MockConverter(i);
                converter.Add(c);

                expectedResult = string.Format(MockConverter.TagFormat, i) + expectedResult;
            }

            var actualResult = converter.Convert(value, typeof(string), null, CultureInfo.CurrentCulture);

            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
