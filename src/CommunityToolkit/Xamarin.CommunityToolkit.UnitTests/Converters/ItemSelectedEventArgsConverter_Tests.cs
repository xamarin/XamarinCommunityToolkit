using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class ItemSelectedEventArgsConverter_Tests
	{
		static object expectedValue = 100;

		public static IEnumerable<object[]> GetData() => new List<object[]>
		{
            // We know it's deprecated, still good to test it
#pragma warning disable CS0618 // Type or member is obsolete
            new object[] { new SelectedItemChangedEventArgs(expectedValue), expectedValue},
#pragma warning restore CS0618 // Type or member is obsolete
        };

		[Theory]
		[MemberData(nameof(GetData))]
		public void ItemSelectedEventArgsConverter(SelectedItemChangedEventArgs value, object expectedResult)
		{
			var itemSelectedEventArgsConverter = new ItemSelectedEventArgsConverter();

			var result = itemSelectedEventArgsConverter.Convert(value, typeof(ItemSelectedEventArgsConverter), null, CultureInfo.CurrentCulture);
			Assert.Equal(result, expectedResult);
		}

		[Theory]
		[InlineData("Random String")]
		public void InvalidConverterValuesThrowsArgumenException(object value)
		{
			var itemSelectedEventArgsConverter = new ItemSelectedEventArgsConverter();
			Assert.Throws<ArgumentException>(() => itemSelectedEventArgsConverter.Convert(value, typeof(ItemSelectedEventArgsConverter), null, CultureInfo.CurrentCulture));
		}
	}
}