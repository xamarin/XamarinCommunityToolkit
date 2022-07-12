using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class ItemSelectedEventArgsConverter_Tests
	{
		static readonly object expectedValue = 100;

		public static IEnumerable<object?[]> GetData() => new List<object?[]>
		{
            // We know it's deprecated, still good to test it
#pragma warning disable CS0618 // Type or member is obsolete
            new object[] { new SelectedItemChangedEventArgs(expectedValue), expectedValue },
            new object?[] { null, null },
#pragma warning restore CS0618 // Type or member is obsolete
		};

		[TestCaseSource(nameof(GetData))]
		public void ItemSelectedEventArgsConverter(SelectedItemChangedEventArgs value, object expectedResult)
		{
			var itemSelectedEventArgsConverter = CreateConverter();

			var result = itemSelectedEventArgsConverter.Convert(value, typeof(ItemSelectedEventArgsConverter), null, CultureInfo.CurrentCulture);
			Assert.AreEqual(result, expectedResult);
		}

		[TestCase("Random String")]
		public void InvalidConverterValuesThrowsArgumentException(object value)
		{
			var itemSelectedEventArgsConverter = CreateConverter();
			Assert.Throws<ArgumentException>(() => itemSelectedEventArgsConverter.Convert(value, typeof(ItemSelectedEventArgsConverter), null, CultureInfo.CurrentCulture));
		}

		static IValueConverter CreateConverter() => new ItemSelectedEventArgsConverter();
	}
}