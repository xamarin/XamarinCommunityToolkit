using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class UtcDateTimeToLocalStringConverter_Tests
	{
		static readonly DateTime testDateTimeUtcNow = DateTime.UtcNow;
		static readonly DateTimeOffset testDateTimeOffsetUtcNow = DateTimeOffset.UtcNow;
		static readonly DateTime testlocalDateTime = DateTime.Now.ToLocalTime();

		public static IEnumerable<object?[]> GetDateTimeData() => new List<object?[]>
		{
			new object?[] { testDateTimeUtcNow, "G", DateTime.Now.ToLocalTime().ToString("G"),  }
		};

		public static IEnumerable<object?[]> GetDateTimeOffsetData() => new List<object?[]>
		{
			new object?[] { testDateTimeOffsetUtcNow, "G", DateTime.Now.ToLocalTime().ToString("G"),  }
		};

		public static IEnumerable<object?[]> GetInvalidDateTimeData() => new List<object?[]>
		{
			new object?[] { null, "G" }
		};

		public static IEnumerable<object?[]> GetEmptyDateTimeFormatData() => new List<object?[]>
		{
			new object?[] { testDateTimeUtcNow, string.Empty }
		};

		public static IEnumerable<object?[]> GetInvalidDateTimeFormatData() => new List<object?[]>
		{
			new object?[] { testDateTimeUtcNow, "asd", DateTime.Now.ToLocalTime().ToString("G"),  }
		};

		[TestCaseSource(nameof(GetDateTimeData))]
		public void UtcDateTimeToLocalStringConvert_Validation(DateTime value, string dateTimeFormat, string expectedResult)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			var result = utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(expectedResult, result);
		}

		[TestCaseSource(nameof(GetDateTimeOffsetData))]
		public void UtcDateTimeOffsetToLocalStringConvert_Validation(DateTimeOffset value, string dateTimeFormat, string expectedResult)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			var result = utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(expectedResult, result);
		}

		[TestCaseSource(nameof(GetInvalidDateTimeData))]
		public void UtcDateTimeToLocalStringConvert_InValidDateTimeFormatThrowArgumenException(DateTimeOffset value, string dateTimeFormat)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			Assert.Throws<ArgumentException>(() => utcDateTimeToLocalStringConverter.Convert(value, null, null, null));
		}

		[TestCaseSource(nameof(GetEmptyDateTimeFormatData))]
		[TestCaseSource(nameof(GetInvalidDateTimeFormatData))]
		public void UtcDateTimeToLocalStringConvert_InValidDateTimeThrowArgumenException(DateTimeOffset value, string dateTimeFormat)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			Assert.Throws<ArgumentException>(() => utcDateTimeToLocalStringConverter.Convert(value, null, null, null));
		}
	}
}