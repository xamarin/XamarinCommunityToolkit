using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using XamarinCommunityToolkit.Converters;
using Xunit;

namespace XamarinCommunityToolkit.UnitTests.Converters
{
    public class ItemTappedEventArgsConverter_Tests
    {
        public static object expectedValue = 100;

        public static IEnumerable<object[]> GetData() => new List<object[]>
            {
                new object[] { new ItemTappedEventArgs(null, expectedValue), expectedValue},
            };

        [Theory]
        [MemberData(nameof(GetData))]
        public void InverterBoolConverter(ItemTappedEventArgs value, object expectedResult)
        {
            var itemTappedEventArgsConverter = new ItemTappedEventArgsConverter();

            var result = itemTappedEventArgsConverter.Convert(value, typeof(ItemTappedEventArgsConverter), null, CultureInfo.CurrentCulture);

            Assert.Equal(result, expectedResult);
        }

        [Theory]
        [InlineData("Random String")]
        public void InValidConverterValuesThrowArgumenException(object value)
        {
            var itemTappedEventArgsConverter = new ItemTappedEventArgsConverter();
            Assert.Throws<ArgumentException>(() => itemTappedEventArgsConverter.Convert(value, typeof(ItemTappedEventArgsConverter), null, CultureInfo.CurrentCulture));
        }
    }
}
