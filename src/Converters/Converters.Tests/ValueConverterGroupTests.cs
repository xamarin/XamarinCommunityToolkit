using System;
using System.Globalization;
using FormsCommunityToolkit.Converters;
using NUnit.Framework;
using Xamarin.Forms;

namespace Converters.Tests
{
    [TestFixture]
    public class ValueConverterGroupTests
    {
        class MockConverter : IValueConverter
        {
            public const string TagFormat = "c{0}<-";

            public int Id { get;}

            public MockConverter(int id)
            {
                Id = id;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return string.Format(TagFormat, Id) + value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
        }

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
