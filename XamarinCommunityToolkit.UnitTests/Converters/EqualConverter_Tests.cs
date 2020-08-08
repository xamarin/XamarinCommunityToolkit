using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class EqualConverter_Tests
	{
		public const string TestValue = nameof(TestValue);

		[Theory]
		[InlineData(200, 200)]
		[InlineData(TestValue, TestValue)]
		public void IsEqual(object value, object comparedValue)
		{
			var equalConverter = new EqualConverter();

			var result = equalConverter.Convert(value, typeof(EqualConverter_Tests), comparedValue, CultureInfo.CurrentCulture);

			Assert.IsType<bool>(result);
			Assert.True((bool)result);
		}

		[Theory]
		[InlineData(200, 400)]
		[InlineData(TestValue, "")]
		public void IsNotEqual(object value, object comparedValue)
		{
			var equalConverter = new EqualConverter();

			var result = equalConverter.Convert(value, typeof(EqualConverter_Tests), comparedValue, CultureInfo.CurrentCulture);

			Assert.IsType<bool>(result);
			Assert.False((bool)result);
		}
	}
}