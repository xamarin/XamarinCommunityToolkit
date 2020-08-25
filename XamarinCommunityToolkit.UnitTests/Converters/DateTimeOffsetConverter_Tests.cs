using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class DateTimeOffsetConverter_Tests
	{
		private static DateTime testDateTimeLocal = new DateTime(2020, 08, 25, 13, 37, 00, DateTimeKind.Local);
		private static DateTime testDateTimeUtc = new DateTime(2020, 08, 25, 13, 37, 00, DateTimeKind.Utc);
		private static DateTime testDateTimeUnspecified = new DateTime(2020, 08, 25, 13, 37, 00);
		private static DateTimeOffset testDateTimeOffsetLocal = new DateTimeOffset(2020, 08, 25, 13, 37, 00, DateTimeOffset.Now.Offset);
		private static DateTimeOffset testDateTimeOffsetUtc = new DateTimeOffset(2020, 08, 25, 13, 37, 00, DateTimeOffset.UtcNow.Offset);
			
		public static IEnumerable<object[]> GetData()
		{
			return new List<object[]>
			{
				new object[] { DateTimeOffset.MinValue, DateTime.MinValue},
				new object[] { DateTimeOffset.MaxValue, DateTime.MaxValue},
				new object[] {  testDateTimeOffsetLocal, testDateTimeLocal },
				new object[] { testDateTimeOffsetUtc, testDateTimeUtc }, 
				new object[] { testDateTimeOffsetUtc, testDateTimeUnspecified}, 
			};
		}
		
		public static IEnumerable<object[]> GetDataReverse()
		{
			return new List<object[]>
			{
				new object[] { DateTime.MinValue, DateTimeOffset.MinValue},
				new object[] { DateTime.MaxValue, DateTimeOffset.MaxValue},
				new object[] { testDateTimeLocal, testDateTimeOffsetLocal },
				new object[] { testDateTimeUtc, testDateTimeOffsetUtc}, 
				new object[] { testDateTimeUnspecified, testDateTimeOffsetUtc}, 
			};
		}

		[Theory]
		[MemberData(nameof(GetData))]
		public void DateTimeOffsetConverter(DateTimeOffset value, DateTime expectedResult)
		{
			var dateTimeOffsetConverter = new DateTimeOffsetConverter();
		
			var result = dateTimeOffsetConverter.Convert(value, typeof(DateTimeOffsetConverter_Tests), null,
				CultureInfo.CurrentCulture);
		
			Assert.Equal(expectedResult, result);
		}
		
		[Theory]
		[MemberData(nameof(GetDataReverse))]
		public void DateTimeOffsetConverterBack(DateTime value, DateTimeOffset expectedResult)
		{
			var dateTimeOffsetConverter = new DateTimeOffsetConverter();

			var result = dateTimeOffsetConverter.ConvertBack(value, typeof(DateTimeOffsetConverter_Tests), null,
				CultureInfo.CurrentCulture);

			Assert.Equal(expectedResult, result);
		}
	}
}