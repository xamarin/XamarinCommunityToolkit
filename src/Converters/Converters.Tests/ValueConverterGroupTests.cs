using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Toolkit.Converters;
using Converters.Tests.Mocks;

namespace Converters.Tests
{
    [TestClass]
    public class ValueConverterGroupTests
    {
        ValueConverterGroup converter;

        [TestInitialize]
        public void Setup()
        {
            converter = new ValueConverterGroup();
        }

        [TestMethod]
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
