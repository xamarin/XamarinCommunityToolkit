using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	[TestOf(typeof(BaseConverterOneWay<string, Color>))]
	public class BaseConverterOneWay_Tests
	{
		[TestCaseSource(nameof(GetValidTestData))]
		public void MockConverterOneWayConvert((bool ShouldAllowNull, string? Value, Color ExpectedResult) testCase)
		{
			var mockConverterOneWay = CreateConverter(testCase.ShouldAllowNull);

			var result = mockConverterOneWay.Convert(testCase.Value, typeof(Color), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, testCase.ExpectedResult);
		}

		[TestCaseSource(nameof(GetInvalidTestData))]
		public void MockConverterOneWayConvertInvalidValuesThrowException((bool ShouldAllowNull, string? Value, Type ExpectedExceptionType) testCase)
		{
			var mockConverterOneWay = CreateConverter(testCase.ShouldAllowNull);

			Assert.Throws(testCase.ExpectedExceptionType, () => mockConverterOneWay.Convert(testCase.Value, typeof(Color), null, CultureInfo.CurrentCulture));
		}

		static IValueConverter CreateConverter(bool shouldAllowNull) =>
			shouldAllowNull ? new MockNullableConverterOneWay() : new MockConverterOneWay();

		static IEnumerable<(bool ShouldAllowNull, string? Value, Color ExpectedResult)> GetValidTestData()
		{
			yield return (true, "Red", Color.Red);
			yield return (true, "Blue", Color.Blue);
			yield return (true, null, Color.Black);

			yield return (false, "Red", Color.Red);
			yield return (false, "Blue", Color.Blue);
		}

		static IEnumerable<(bool ShouldAllowNull, string? Value, Type ExpectedExceptionType)> GetInvalidTestData()
		{
			yield return (true, "red", typeof(ArgumentException));
			yield return (true, "Green", typeof(ArgumentException));
			yield return (true, "red", typeof(ArgumentException));
			yield return (true, "Green", typeof(ArgumentException));

			yield return (false, null, typeof(ArgumentNullException));
		}
	}
}