using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using NUnit.Framework;
using System.Collections.Generic;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class CompareConverter_Tests
	{
		public const string TrueTestObject = nameof(TrueTestObject);
		public const string FalseTestObject = nameof(FalseTestObject);


		static IEnumerable<object?[]> GetTestData()
		{
			yield return new object?[] { 10d, CompareConverter.Operator.Greater, 20d, TrueTestObject, FalseTestObject, FalseTestObject };
			yield return new object?[] { 10d, CompareConverter.Operator.GreaterOrEqual, 20d, TrueTestObject, FalseTestObject, FalseTestObject };
			yield return new object?[] { 10d, CompareConverter.Operator.Equal, 20d, TrueTestObject, FalseTestObject, FalseTestObject };
			yield return new object?[] { 10d, CompareConverter.Operator.NotEqual, 20d, TrueTestObject, FalseTestObject, TrueTestObject };
			yield return new object?[] { 10d, CompareConverter.Operator.Smaller, 20d, TrueTestObject, FalseTestObject, TrueTestObject };
			yield return new object?[] { 10d, CompareConverter.Operator.SmallerOrEqual, 20d, TrueTestObject, FalseTestObject, TrueTestObject };

			yield return new object?[] { 20d, CompareConverter.Operator.Greater, 20d, TrueTestObject, FalseTestObject, FalseTestObject };
			yield return new object?[] { 20d, CompareConverter.Operator.GreaterOrEqual, 20d, TrueTestObject, FalseTestObject, TrueTestObject };
			yield return new object?[] { 20d, CompareConverter.Operator.Equal, 20d, TrueTestObject, FalseTestObject, TrueTestObject };
			yield return new object?[] { 20d, CompareConverter.Operator.NotEqual, 20d, TrueTestObject, FalseTestObject, FalseTestObject };
			yield return new object?[] { 20d, CompareConverter.Operator.Smaller, 20d, TrueTestObject, FalseTestObject, FalseTestObject };
			yield return new object?[] { 20d, CompareConverter.Operator.SmallerOrEqual, 20d, TrueTestObject, FalseTestObject, TrueTestObject };

			yield return new object?[] { 20d, CompareConverter.Operator.Greater, 10d, TrueTestObject, FalseTestObject, TrueTestObject };
			yield return new object?[] { 20d, CompareConverter.Operator.GreaterOrEqual, 10d, TrueTestObject, FalseTestObject, TrueTestObject };
			yield return new object?[] { 20d, CompareConverter.Operator.Equal, 10d, TrueTestObject, FalseTestObject, FalseTestObject };
			yield return new object?[] { 20d, CompareConverter.Operator.NotEqual, 10d, TrueTestObject, FalseTestObject, TrueTestObject };
			yield return new object?[] { 20d, CompareConverter.Operator.Smaller, 10d, TrueTestObject, FalseTestObject, FalseTestObject };
			yield return new object?[] { 20d, CompareConverter.Operator.SmallerOrEqual, 10d, TrueTestObject, FalseTestObject, FalseTestObject };


			yield return new object?[] { 20d, CompareConverter.Operator.Greater, 20d, null, null, false };
			yield return new object?[] { 20d, CompareConverter.Operator.GreaterOrEqual, 20d, null, null, true };
			yield return new object?[] { 20d, CompareConverter.Operator.Equal, 20d, null, null, true };
			yield return new object?[] { 20d, CompareConverter.Operator.NotEqual, 20d, null, null, false };
			yield return new object?[] { 20d, CompareConverter.Operator.Smaller, 20d, null, null, false };
			yield return new object?[] { 20d, CompareConverter.Operator.SmallerOrEqual, 20d, null, null, true };

			yield return new object?[] { 20d, CompareConverter.Operator.Greater, 10d, null, null, true };
			yield return new object?[] { 20d, CompareConverter.Operator.GreaterOrEqual, 10d, null, null, true };
			yield return new object?[] { 20d, CompareConverter.Operator.Equal, 10d, null, null, false };
			yield return new object?[] { 20d, CompareConverter.Operator.NotEqual, 10d, null, null, true };
			yield return new object?[] { 20d, CompareConverter.Operator.Smaller, 10d, null, null, false };
			yield return new object?[] { 20d, CompareConverter.Operator.SmallerOrEqual, 10d, null, null, false };

			yield return new object?[] { 10d, CompareConverter.Operator.Greater, 20d, null, null, false };
			yield return new object?[] { 10d, CompareConverter.Operator.GreaterOrEqual, 20d, null, null, false };
			yield return new object?[] { 10d, CompareConverter.Operator.Equal, 20d, null, null, false };
			yield return new object?[] { 10d, CompareConverter.Operator.NotEqual, 20d, null, null, true };
			yield return new object?[] { 10d, CompareConverter.Operator.Smaller, 20d, null, null, true };
			yield return new object?[] { 10d, CompareConverter.Operator.SmallerOrEqual, 20d, null, null, true };
		}

		[TestCaseSource(nameof(GetTestData))]
		[TestCase(20d, CompareConverter.Operator.Greater, 20d, TrueTestObject, FalseTestObject, TrueTestObject)]
		public void CompareConverterConvert(IComparable value, CompareConverter.Operator comparisonOperator, IComparable comparingValue, object trueObject, object falseObject, object expectedResult)
		{
			var compareConverter = new CompareConverter
			{
				TrueObject = trueObject,
				FalseObject = falseObject,
				ComparisonOperator = comparisonOperator,
				ComparingValue = comparingValue
			};

			var result = compareConverter.Convert(value, typeof(BoolToObjectConverter_Tests), null!, CultureInfo.CurrentCulture);
			Assert.AreEqual(result, expectedResult);
		}

		[Test]
		public void CompareConverterInValidValuesThrowArgumenException()
		{
			var compareConverter = new CompareConverter()
			{
				ComparingValue = 20d
			};

			Assert.Throws<ArgumentException>(() => compareConverter.Convert(new { Name = "xct" }, typeof(BoolToObjectConverter_Tests), null!, CultureInfo.CurrentCulture));
		}

		[TestCase(20d, null, TrueTestObject, FalseTestObject)]
		[TestCase(20d, 20d, TrueTestObject, null)]
		[TestCase(20d, 20d, null, FalseTestObject)]
		[TestCase(null, 20d, null, FalseTestObject)]
		public void CompareConverterInValidValuesThrowArgumentNullException(object value, IComparable comparingValue, object trueObject, object falseObject)
		{
			var compareConverter = new CompareConverter()
			{
				ComparingValue = comparingValue,
				FalseObject = falseObject,
				TrueObject = trueObject
			};

			Assert.Throws<ArgumentNullException>(() => compareConverter.Convert(value, typeof(BoolToObjectConverter_Tests), null!, CultureInfo.CurrentCulture));
		}

		[TestCase(20d, (CompareConverter.Operator)10, 20d)]
		public void CompareConverterInValidValuesThrowArgumentOutOfRangeException(object value, CompareConverter.Operator comparisonOperator, IComparable comparingValue)
		{
			var compareConverter = new CompareConverter
			{
				ComparisonOperator = comparisonOperator,
				ComparingValue = comparingValue
			};

			Assert.Throws<ArgumentOutOfRangeException>(() => compareConverter.Convert(value, typeof(BoolToObjectConverter_Tests), null!, CultureInfo.CurrentCulture));
		}
	}
}