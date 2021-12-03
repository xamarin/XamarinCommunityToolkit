using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class ListIsNullOrEmptyConverter_Tests
	{
		public static IEnumerable<object?[]> GetData() => new List<object?[]>
		{
			new object[] { new List<string>(), true },
			new object[] { new List<string>() { "TestValue" }, false },
			new object?[] { null, true },
			new object[] { Enumerable.Range(1, 3), false },
		};

		[TestCaseSource(nameof(GetData))]
		public void ListIsNullOrEmptyConverter(object value, bool expectedResult)
		{
			var listIsNullOrEmptyConverter = CreateConverter();

			var result = listIsNullOrEmptyConverter.Convert(value, typeof(ListIsNullOrEmptyConverter), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCase(0)]
		public void InValidConverterValuesThrowArgumentException(object value)
		{
			var listIsNullOrEmptyConverter = CreateConverter();

			Assert.Throws<ArgumentException>(() => listIsNullOrEmptyConverter.Convert(value, typeof(ListIsNullOrEmptyConverter), null, CultureInfo.CurrentCulture));
		}

		static IValueConverter CreateConverter() => new ListIsNullOrEmptyConverter();
	}
}