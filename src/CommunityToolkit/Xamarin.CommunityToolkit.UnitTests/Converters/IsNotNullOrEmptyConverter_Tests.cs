using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class IsNotNullOrEmptyConverter_Tests
	{
		[TestCase("Test", true)]
		[TestCase(typeof(IsNotNullOrEmptyConverter), true)]
		[TestCase(null, false)]
		[TestCase("", false)]
		public void IsNotNullOrEmptyConverter(object value, bool expectedResult)
		{
			var isNotNullOrEmptyConverter = new IsNotNullOrEmptyConverter();

			var result = isNotNullOrEmptyConverter.Convert(value, typeof(IsNotNullOrEmptyConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}
	}
}