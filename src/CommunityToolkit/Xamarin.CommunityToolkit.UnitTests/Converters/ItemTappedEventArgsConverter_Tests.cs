using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class ItemTappedEventArgsConverter_Tests
	{
		static object expectedValue = 100;

		public static IEnumerable<object?[]> GetData() => new List<object?[]>
			{
            // We know it's deprecated, still good to test it
#pragma warning disable CS0618 // Type or member is obsolete
                new object?[] { new ItemTappedEventArgs(null, expectedValue), expectedValue },
                new object?[] { new ItemTappedEventArgs(null, null), null },
#pragma warning restore CS0618 // Type or member is obsolete
			};

		[TestCaseSource(nameof(GetData))]
		public void ItemTappedEventArgsConverter(ItemTappedEventArgs value, object expectedResult)
		{
			var itemTappedEventArgsConverter = CreateConverter();

			var result = itemTappedEventArgsConverter.Convert(value, typeof(ItemTappedEventArgsConverter), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCase("Random String")]
		public void InValidConverterValuesThrowArgumentException(object value)
		{
			var itemTappedEventArgsConverter = CreateConverter();
			Assert.Throws<ArgumentException>(() => itemTappedEventArgsConverter.Convert(value, typeof(ItemTappedEventArgsConverter), null, CultureInfo.CurrentCulture));
		}

		static IValueConverter CreateConverter() => new ItemTappedEventArgsConverter();
	}
}