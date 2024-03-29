﻿using System;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class BoolToObjectConverter_Tests
	{
		public const string TrueTestObject = nameof(TrueTestObject);
		public const string FalseTestObject = nameof(FalseTestObject);

		[TestCase(true, TrueTestObject, FalseTestObject, TrueTestObject)]
		[TestCase(false, TrueTestObject, FalseTestObject, FalseTestObject)]
		public void BoolToObjectConvert(bool value, object trueObject, object falseObject, object expectedResult)
		{
			var boolObjectConverter = new BoolToObjectConverter();
			boolObjectConverter.TrueObject = trueObject;
			boolObjectConverter.FalseObject = falseObject;

			var result = boolObjectConverter.Convert(value, typeof(BoolToObjectConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCase(TrueTestObject, TrueTestObject, FalseTestObject, true)]
		[TestCase(FalseTestObject, TrueTestObject, FalseTestObject, false)]
		public void BoolToObjectConvertBack(object value, object trueObject, object falseObject, bool expectedResult)
		{
			var boolObjectConverter = new BoolToObjectConverter();
			boolObjectConverter.TrueObject = trueObject;
			boolObjectConverter.FalseObject = falseObject;

			var result = boolObjectConverter.ConvertBack(value, typeof(BoolToObjectConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCase("")]
		public void BoolToObjectInValidValuesThrowArgumentException(object value)
		{
			var boolObjectConverter = new BoolToObjectConverter();
			Assert.Throws<ArgumentException>(() => boolObjectConverter.Convert(value, typeof(BoolToObjectConverter_Tests), null, CultureInfo.CurrentCulture));
		}
	}
}