using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class ListIsNullOrEmptyConverter_Tests
	{
		public static IEnumerable<object[]> GetData()
		{
			return new List<object[]>
			{
				new object[] { new List<string>(), true},
				new object[] { new List<string>() { "TestValue"}, false},
				new object[] { null, true},
				new object[] { Enumerable.Range(1, 3), false},
			};
		}

		[Theory]
		[MemberData(nameof(GetData))]
		public void ListIsNullOrEmptyConverter(object value, bool expectedResult)
		{
			var listIstNullOrEmptyConverter = new ListIsNullOrEmptyConverter();

			var result = listIstNullOrEmptyConverter.Convert(value, typeof(ListIsNullOrEmptyConverter), null, CultureInfo.CurrentCulture);

			Assert.Equal(result, expectedResult);
		}

		[Theory]
		[InlineData(0)]
		public void InValidConverterValuesThrowArgumenException(object value)
		{
			var listIstNullOrEmptyConverter = new ListIsNullOrEmptyConverter();

			Assert.Throws<ArgumentException>(() => listIstNullOrEmptyConverter.Convert(value, typeof(ListIsNullOrEmptyConverter), null, CultureInfo.CurrentCulture));
		}
	}
}