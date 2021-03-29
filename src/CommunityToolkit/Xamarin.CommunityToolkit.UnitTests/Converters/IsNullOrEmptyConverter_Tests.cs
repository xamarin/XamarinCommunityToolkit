using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class IsNullOrEmptyConverter_Tests
	{
		public static IEnumerable<object?[]> GetData() => new List<object?[]>
		{
			new object?[] { null, true },
			new object[] { string.Empty, true },
			new object[] { "Test", false },
			new object[] { typeof(IsNullOrEmptyConverter), false },
			new object[] { new List<string>(), true },
			new object[] { new List<string>() { "TestValue" }, false },
			new object[] { Enumerable.Range(1, 3), false },
		};

		[TestCaseSource(nameof(GetData))]
		public void IsNullOrEmptyConverter(object value, bool expectedResult)
		{
			var isNullOrEmptyConverter = new IsNullOrEmptyConverter();

			var result = isNullOrEmptyConverter.Convert(value, typeof(IsNotNullOrEmptyConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCaseSource(nameof(GetData))]
		public void IsNullOrEmptyConverterInverted(object value, bool expectedResult)
		{
			var isNullOrEmptyConverter = new IsNullOrEmptyConverter
			{
				InvertCheck = true
			};

			var result = isNullOrEmptyConverter.Convert(value, typeof(IsNotNullOrEmptyConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, !expectedResult);
		}
	}
}