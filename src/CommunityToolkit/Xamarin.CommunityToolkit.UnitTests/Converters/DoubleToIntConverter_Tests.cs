﻿using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class DoubleToIntConverter_Tests
	{
		[Theory]
		[InlineData(2.5, 2)]
		[InlineData(2.55, 3)]
		[InlineData(2.555, 3)]
		[InlineData(2.555, 652, 255)]
		public void DoubleToIntConverter(double value, int expectedResult, object? ratio = null)
		{
			var doubleToIntConverter = new DoubleToIntConverter();

			var result = doubleToIntConverter.Convert(value, typeof(DoubleToIntConverter_Tests), ratio, CultureInfo.CurrentCulture);

			Assert.Equal(result, expectedResult);
		}

		[Theory]
		[InlineData(2, 2)]
		public void DoubleToIntConverterBack(int value, double expectedResult, object? ratio = null)
		{
			var doubleToIntConverter = new DoubleToIntConverter();

			var result = doubleToIntConverter.ConvertBack(value, typeof(DoubleToIntConverter_Tests), ratio, CultureInfo.CurrentCulture);

			Assert.Equal(result, expectedResult);
		}

		[Theory]
		[InlineData("")]
		public void DoubleToIntInValidValuesThrowArgumenException(object value)
		{
			var doubleToIntConverter = new DoubleToIntConverter();
			Assert.Throws<ArgumentException>(() => doubleToIntConverter.Convert(value, typeof(BoolToObjectConverter_Tests), null, CultureInfo.CurrentCulture));
			Assert.Throws<ArgumentException>(() => doubleToIntConverter.ConvertBack(value, typeof(BoolToObjectConverter_Tests), null, CultureInfo.CurrentCulture));
		}
	}
}