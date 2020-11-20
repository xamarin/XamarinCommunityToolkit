using System.Collections.Generic;
using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class MultiConverter_Tests
	{
		public static IEnumerable<object[]> GetData()
		{
			return new List<object[]>
			{
				new object[] { new List<MultiConverterParameter>() { { new MultiConverterParameter() { Value = "Param 1", } } , { new MultiConverterParameter() { Value = "Param 2", } } }},
			};
		}

		[Theory]
		[MemberData(nameof(GetData))]
		public void MultiConverter(object value)
		{
			var multiConverter = new MultiConverter();

			var result = multiConverter.Convert(value, typeof(MultiConverter), null, CultureInfo.CurrentCulture);

			Assert.Equal(result, value);
		}
	}
}