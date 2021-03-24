using System;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class MathExpressionConverter_Tests
	{
		readonly Type type = typeof(MathExpressionConverter_Tests);
		readonly CultureInfo cultureInfo = CultureInfo.CurrentCulture;
		const double tolerance = 0.00001d;

		[TestCase("2 + 2 * 2", 6d)]
		[TestCase("(2 + 2) * 2", 8d)]
		[TestCase("3 + 4 * 2 / (1 - 5)^2", 3.5d)]
		[TestCase("3 + 4 * 2 + cos(100 + 20) / (1 - 5)^2 + pow(20,2)", 411.05088631065792d)]
		public void MathExpressionConverterReturnsCorrectResult(string value, double expectedResult)
		{
			var mathExpressionConverter = new MathExpressionConverter();

			var result = mathExpressionConverter.Convert(value, type, null, cultureInfo);

			Assert.IsTrue(Math.Abs((double)result - expectedResult) < tolerance);
		}

		[TestCase("1 + 3 + 5 + (3 - 2))")]
		[TestCase("1 + 2) + (9")]
		[TestCase("100 + pow(2)")]
		public void MathExpressionConverterThrowsExceptions(string value)
		{
			var mathExpressionConverter = new MathExpressionConverter();

			var result = new TestDelegate(()
				=> mathExpressionConverter.Convert(value, type, null, cultureInfo));

			Assert.Catch<Exception>(result);
		}
	}
}