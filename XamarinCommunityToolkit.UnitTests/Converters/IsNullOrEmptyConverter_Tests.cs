using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class IsNullOrEmptyConverter_Tests
	{
		[Theory]
		[InlineData("Test", false)]
		[InlineData(null, true)]
		[InlineData(typeof(IsNullOrEmptyConverter), false)]
		[InlineData("", true)]
		public void IsNullOrEmptyConverter(object value, bool expectedResult)
		{
			var isNullOrEmptyConverter = new IsNullOrEmptyConverter();

			var result = isNullOrEmptyConverter.Convert(value, typeof(IsNotNullOrEmptyConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.Equal(result, expectedResult);
		}
	}
}
