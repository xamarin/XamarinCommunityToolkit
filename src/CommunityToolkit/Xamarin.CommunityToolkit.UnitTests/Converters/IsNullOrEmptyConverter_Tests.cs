using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class IsNullOrEmptyConverter_Tests
	{
		[TestCase(null, true)]
		[TestCase("", true)]
		[TestCase("Test", false)]
		[TestCase(typeof(IsNullOrEmptyConverter), false)]
		public void IsNullOrEmptyConverter(object value, bool expectedResult)
		{
			var isNullOrEmptyConverter = new IsNullOrEmptyConverter();

			var result = isNullOrEmptyConverter.Convert(value, typeof(IsNotNullOrEmptyConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}
	}
}