using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class ItemTappedEventArgsConverter_Tests
	{
		static object expectedValue = 100;

		public static IEnumerable<object[]> GetData() => new List<object[]>
			{
            // We know it's deprecated, still good to test it
#pragma warning disable CS0618 // Type or member is obsolete
                new object[] { new ItemTappedEventArgs(null, expectedValue), expectedValue},
#pragma warning restore CS0618 // Type or member is obsolete
            };

		[Theory]
		[MemberData(nameof(GetData))]
		public void ItemTappedEventArgsConverter(ItemTappedEventArgs value, object expectedResult)
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