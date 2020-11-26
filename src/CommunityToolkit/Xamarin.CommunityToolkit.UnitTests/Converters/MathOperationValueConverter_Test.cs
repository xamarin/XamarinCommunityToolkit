using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class MathOperationValueConverter_Test
	{
		[Theory]
		[InlineData(-1, OperatorEnum.Add, 2, 1)]
		[InlineData(10, OperatorEnum.Add, 120, 130)]
		[InlineData(101.11, OperatorEnum.Add, 12.4, 113.15)]
		[InlineData(-1, OperatorEnum.Minus, 2, -3)]
		[InlineData(100, OperatorEnum.Minus, 50, 50)]
		[InlineData(220.5, OperatorEnum.Minus, 1.5, 219)]
		[InlineData(-1, OperatorEnum.Multiply, 2, -2)]
		[InlineData(100, OperatorEnum.Multiply, 4, 400)]
		[InlineData(220.5, OperatorEnum.Multiply, 1.5, 330)]
		[InlineData(-1, OperatorEnum.Divide, 2, 0.5)]
		[InlineData(100, OperatorEnum.Divide, 4, 25)]
		[InlineData(20.5, OperatorEnum.Divide, 1.5, 12.75)]
		[InlineData(-5, OperatorEnum.Remainder, 4, -1)]
		[InlineData(100, OperatorEnum.Remainder, 4, 0)]
		[InlineData(20.5, OperatorEnum.Remainder, 100.25, 20.5)]
		[InlineData(-5, OperatorEnum.Exponent, 4, 625)]
		[InlineData(100, OperatorEnum.Exponent, 4, 100000000)]
		[InlineData(2.5, OperatorEnum.Exponent, 1.25, 3.1435835742073)]
		public void GeneralTests(double value, OperatorEnum @operator, double operatorValue, int expectedResult)
		{
			var mathOperationValueConverter = new MathOperationValueConverter
			{
				Operator = @operator,
				OperatorValue = operatorValue
			};

			var result = mathOperationValueConverter.Convert(value, typeof(MathOperationValueConverter_Test), null, CultureInfo.CurrentCulture);

			Assert.Equal(expectedResult, result);
		}

		[Fact]
		public void DivideByZeroReturnsZero()
		{
			var mathOperationValueConverter = new MathOperationValueConverter
			{
				Operator = OperatorEnum.Divide,
				OperatorValue = 0
			};

			var result = mathOperationValueConverter.Convert(0, typeof(MathOperationValueConverter_Test), null, CultureInfo.CurrentCulture);

			Assert.Equal(0, result);
		}

		[Fact]
		public void UnknownOperatorThrowsArgumentException()
		{
			var mathOperationValueConverter = new MathOperationValueConverter
			{
				Operator = OperatorEnum.Unknown,
				OperatorValue = 0
			};

			Assert.Throws<ArgumentException>(() => mathOperationValueConverter.Convert(0, typeof(MathOperationValueConverter_Test), null, CultureInfo.CurrentCulture));
		}
	}
}