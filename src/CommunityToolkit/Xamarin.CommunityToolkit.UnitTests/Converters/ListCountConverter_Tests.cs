using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class ListCountConverter_Tests
	{
		public static IEnumerable<object?[]> GetData() => new List<object?[]>
		{
			new object[] { new List<string>(), 0 },
			new object[] { new List<string>() { "TestValue" }, 1 },
			new object?[] { null, 0 },
			new object[] { Enumerable.Range(1, 3), 3 },
		};

		[TestCaseSource(nameof(GetData))]
		public void ListCountConverter(object value, bool expectedResult)
		{
			var listCountConverter = new ListCountConverter();

			var result = listCountConverter.Convert(value, typeof(ListCountConverter), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCase(0)]
		public void InValidConverterValuesThrowArgumenException(object value)
		{
			var listCountConverter = new ListCountConverter();

			Assert.Throws<ArgumentException>(() => listCountConverter.Convert(value, typeof(ListCountConverter), null, CultureInfo.CurrentCulture));
		}
	}
}