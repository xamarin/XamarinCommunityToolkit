using System;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class MathExpressionConverter_Tests
	{
		//[TestCase("3 + 4 * 2 / (1 - 5)^2", 3.5d)]
		[TestCase("3 + 4 * 2 + cos(100 + 20) / (1 - 5)^2 + pow(20,2)", 414.11034265359)]
		public void BoolToObjectConvert(string value, object expectedResult)
		{
			var boolObjectConverter = new MathExpressionConverter();

			var a = 3d + 4d * 2d + Math.Cos(100d + 20d) / Math.Pow((1 - 5), 2d) + Math.Pow(20d, 2d);
			var result = boolObjectConverter.Convert(value, typeof(MathExpressionConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}
	}
}