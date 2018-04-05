using System.Globalization;
using Converters.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Toolkit.Converters;

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

        [DataTestMethod]
        [DataRow("Test_Value")]
        public void ConvertList(object value)
        {
            var expectedResult = value.ToString();

            for (var i = 0; i < 10; i++)
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
