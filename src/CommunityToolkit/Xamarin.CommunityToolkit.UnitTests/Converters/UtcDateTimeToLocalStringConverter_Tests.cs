using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class UtcDateTimeToLocalStringConverter_Tests
	{
		public static IEnumerable<object?[]> GetEmptyDateTimeFormatData() => new List<object?[]>
		{
			new object?[] { DateTime.UtcNow, string.Empty, DateTime.Now.ToLocalTime().ToString("G") }
		};

		public static IEnumerable<object?[]> GetInvalidDateTimeData() => new List<object?[]>
		{
			new object?[] { null, "G" }
		};

		public static IEnumerable<object?[]> GetInvalidDateTimeFormatData() => new List<object?[]>
		{
			new object?[] {  DateTime.UtcNow, "asd" }
		};

		public static IEnumerable<object?[]> GetValidDateTimeData() => new List<object?[]>
		{
			new object?[] { DateTime.UtcNow, "G", DateTime.Now.ToLocalTime().ToString("G"),  }
		};

		public static IEnumerable<object?[]> GetValidDateTimeOffsetData() => new List<object?[]>
		{
			new object?[] { DateTimeOffset.UtcNow, "G", DateTime.Now.ToLocalTime().ToString("G"),  }
		};

		[TestCaseSource(nameof(GetEmptyDateTimeFormatData))]
		public void EmptyDateTimeFormatShouldFallback(DateTime value, string dateTimeFormat, string expectedResult)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			var result = utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(expectedResult, result);
		}

		[TestCaseSource(nameof(GetInvalidDateTimeFormatData))]
		public void InvalidDateTimeFormatThrowArgumenException(DateTime? value, string dateTimeFormat)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			Assert.Throws<ArgumentException>(() => utcDateTimeToLocalStringConverter.Convert(value, null, null, null));
		}

		[TestCaseSource(nameof(GetInvalidDateTimeData))]
		public void InvalidDateTimeThrowArgumenException(DateTime? value, string dateTimeFormat)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			Assert.Throws<ArgumentException>(() => utcDateTimeToLocalStringConverter.Convert(value, null, null, null));
		}

		[TestCaseSource(nameof(GetValidDateTimeData))]
		public void ValidDateTimeOffset(DateTime value, string dateTimeFormat, string expectedResult)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			var result = utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(expectedResult, result);
		}

		[TestCaseSource(nameof(GetValidDateTimeOffsetData))]
		public void ValidDateTimeOffsetData(DateTimeOffset value, string dateTimeFormat, string expectedResult)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			var result = utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(expectedResult, result);
		}
	}
}