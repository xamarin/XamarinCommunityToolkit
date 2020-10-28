using System;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class ListToStringConverter_Tests
	{
		public static IEnumerable<object[]> GetData()
			=> new List<object[]>
			{
				new object[] { new string[] { "A", "B", "C" }, "+_+", "A+_+B+_+C" },
				new object[] { new string[] { "A", string.Empty, "C" }, ",", "A,C" },
				new object[] { new string[] { "A", null, "C" }, ",", "A,C" },
				new object[] { new string[] { "A" }, ":-:", "A" },
				new object[] { new string[] { }, ",", string.Empty },
				new object[] { null, ",", string.Empty },
				new object[] { new string[] { "A", "B", "C" }, null, "ABC" },
			};

		[Theory]
		[MemberData(nameof(GetData))]
		public void ListToStringConverter(object value, object parameter, object expectedResult)
		{
			var listToStringConverter = new ListToStringConverter();

			var result = listToStringConverter.Convert(value, null, parameter, null);

			Assert.Equal(result, expectedResult);
		}

		[Theory]
		[InlineData(0)]
		public void InValidConverterValuesThrowArgumenException(object value)
		{
			var listToStringConverter = new ListToStringConverter();

			Assert.Throws<ArgumentException>(() => listToStringConverter.Convert(value, null, null, null));
		}

		[Theory]
		[InlineData(0)]
		public void InValidConverterParametersThrowArgumenException(object parameter)
		{
			var listToStringConverter = new ListToStringConverter();

			Assert.Throws<ArgumentException>(() => listToStringConverter.Convert(new object[0], null, parameter, null));
		}
	}
}