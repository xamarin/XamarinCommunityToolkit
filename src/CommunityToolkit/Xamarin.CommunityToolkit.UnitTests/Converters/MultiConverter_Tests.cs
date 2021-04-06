using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class MultiConverter_Tests
	{
		public static IEnumerable<object[]> GetData() => new List<object[]>
		{
			new object[] { new List<MultiConverterParameter>() { { new MultiConverterParameter() { Value = "Param 1", } }, { new MultiConverterParameter() { Value = "Param 2", } } } },
		};

		[TestCaseSource(nameof(GetData))]
		public void MultiConverter(object value)
		{
			var multiConverter = new MultiConverter();

			var result = multiConverter.Convert(value, typeof(MultiConverter), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, value);
		}
	}
}