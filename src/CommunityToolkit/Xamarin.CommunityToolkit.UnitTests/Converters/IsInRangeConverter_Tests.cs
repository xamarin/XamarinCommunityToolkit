﻿using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class IsInRangeConverter_Tests
	{
		public static IEnumerable<object[]> GetData() => new List<object[]>
		{
			new object[] { 2, 1, 3, true },
			new object[] { 1, 2, 3, false },
			new object[] { new DateTime(2000, 1, 15), new DateTime(2000, 1, 1), new DateTime(2000, 2, 1), true },
			new object[] { new DateTime(2000, 1, 1), new DateTime(2000, 1, 15), new DateTime(2000, 2, 1), false },
			new object[] { "b", "a", "d", true },
			new object[] { "a", "b", "d", false },
		};

		public static IEnumerable<object?[]> GetDataForException() => new List<object?[]>
		{
			new object?[] { null, 2, 3, typeof(ArgumentNullException) },
			new object?[] { 1, null, 3, typeof(ArgumentException) },
			new object?[] { 1, 2, null, typeof(ArgumentException) },
		};

		[TestCaseSource(nameof(GetData))]
		public void IsInRangeConverter(object value, object minValue, object maxValue, bool expectedResult)
		{
			var isInRangeConverter = CreateConverter(maxValue, minValue);

			var result = isInRangeConverter.Convert(value, typeof(IsInRangeConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCaseSource(nameof(GetDataForException))]
		public void IsInRangeConverterInvalidValuesThrowArgumentException(object value, object minValue, object maxValue, Type expectedExceptionType)
		{
			var isInRangeConverter = CreateConverter(maxValue, minValue);
			Assert.Throws(expectedExceptionType, () => isInRangeConverter.Convert(value, typeof(IsInRangeConverter_Tests), null, CultureInfo.CurrentCulture));
		}

		static IValueConverter CreateConverter(object maxValue, object minValue) =>
			new IsInRangeConverter
			{
				MinValue = minValue,
				MaxValue = maxValue
			};
	}
}