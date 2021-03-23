using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class MathExpressionConverter_Tests
	{
		[TestCase("3 + 4 * 2 + cos(100 + 20) / (1 - 5)^2 + pi + pow(20,2)", 2d)]
		[TestCase("3 + 4 * 2 / (1 - 5)^2", 2d)]
		public void BoolToObjectConvert(string value, object expectedResult)
		{
			var boolObjectConverter = new MathExpressionConverter();

			var result = boolObjectConverter.Convert(value, typeof(MathExpressionConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}
	}
}