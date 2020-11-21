using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class DateTimeOffsetConverter_Tests
	{
		static DateTime testDateTimeNow = DateTime.Now;
		static DateTime testDateTimeLocal = new DateTime(2020, 08, 25, 13, 37, 00, DateTimeKind.Local);
		static DateTime testDateTimeUtc = new DateTime(2020, 08, 25, 13, 37, 00, DateTimeKind.Utc);
		static DateTime testDateTimeUnspecified = new DateTime(2020, 08, 25, 13, 37, 00);
		static DateTimeOffset testDateTimeOffsetNow = new DateTimeOffset(testDateTimeNow);
		static DateTimeOffset testDateTimeOffsetLocal = new DateTimeOffset(2020, 08, 25, 13, 37, 00, DateTimeOffset.Now.Offset);
		static DateTimeOffset testDateTimeOffsetUtc = new DateTimeOffset(2020, 08, 25, 13, 37, 00, DateTimeOffset.UtcNow.Offset);

		public static IEnumerable<object[]> GetData() =>
			new List<object[]>
			{
				new object[] { testDateTimeNow, testDateTimeNow},
				new object[] { DateTimeOffset.MinValue, DateTime.MinValue},
				new object[] { DateTimeOffset.MaxValue, DateTime.MaxValue},
				new object[] {  testDateTimeOffsetLocal, testDateTimeLocal },
				new object[] { testDateTimeOffsetUtc, testDateTimeUtc },
				new object[] { testDateTimeOffsetUtc, testDateTimeUnspecified},
			};

		public static IEnumerable<object[]> GetDataReverse() =>
			new List<object[]>
			{
				new object[] { testDateTimeNow, testDateTimeNow},
				new object[] { DateTime.MinValue, DateTimeOffset.MinValue},
				new object[] { DateTime.MaxValue, DateTimeOffset.MaxValue},
				new object[] { testDateTimeLocal, testDateTimeOffsetLocal },
				new object[] { testDateTimeUtc, testDateTimeOffsetUtc},
				new object[] { testDateTimeUnspecified, testDateTimeOffsetUtc},
			};

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

		[Fact]
		public void DateTimeOffsetConverter_GivenInvalidParameters_ThrowsException()
		{
			var dateTimeOffsetConverter = new DateTimeOffsetConverter();

			Assert.Throws<ArgumentException>(() => dateTimeOffsetConverter.Convert("Not a DateTimeOffset",
				typeof(DateTimeOffsetConverter_Tests), null,
				CultureInfo.CurrentCulture));
		}

		[Fact]
		public void DateTimeOffsetConverterBack_GivenInvalidParameters_ThrowsException()
		{
			var dateTimeOffsetConverter = new DateTimeOffsetConverter();

			Assert.Throws<ArgumentException>(() => dateTimeOffsetConverter.ConvertBack("Not a DateTime",
				typeof(DateTimeOffsetConverter_Tests), null,
				CultureInfo.CurrentCulture));
		}
	}
}