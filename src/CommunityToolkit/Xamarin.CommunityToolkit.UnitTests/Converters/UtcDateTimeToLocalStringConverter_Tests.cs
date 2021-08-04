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
			new object?[] { default(DateTime).ToUniversalTime(), string.Empty, default(DateTime).ToLocalTime().ToString("G") }
		};

		public static IEnumerable<object?[]> GetInvalidDateTimeData() => new List<object?[]>
		{
			new object?[] { null, "G" }
		};

		public static IEnumerable<object?[]> GetInvalidDateTimeFormatData() => new List<object?[]>
		{
			new object?[] { default(DateTime).ToUniversalTime(), "asd" }
		};

		public static IEnumerable<object?[]> GetValidDateTimeData() => new List<object?[]>
		{
			new object?[] { default(DateTime).ToUniversalTime(), "G", default(DateTime).ToLocalTime().ToString("G"),  }
		};

		public static IEnumerable<object?[]> GetValidDateTimeOffsetData() => new List<object?[]>
		{
			new object?[] { default(DateTimeOffset).ToUniversalTime(), "G", default(DateTime).ToLocalTime().ToString("G"),  }
		};

		[TestCaseSource(nameof(GetEmptyDateTimeFormatData))]
		public void EmptyDateTimeFormatShouldFallback(DateTime? value, string dateTimeFormat, string expectedResult)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			var result = utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), default, CultureInfo.CurrentCulture);

			Assert.AreEqual(expectedResult, result);
		}

		[TestCaseSource(nameof(GetInvalidDateTimeFormatData))]
		public void InvalidDateTimeFormatThrowArgumenException(DateTime? value, string dateTimeFormat)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			Assert.Throws<ArgumentException>(() => utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), default, CultureInfo.CurrentCulture));
		}

		[TestCaseSource(nameof(GetInvalidDateTimeData))]
		public void InvalidDateTimeThrowArgumenException(DateTime? value, string dateTimeFormat)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			Assert.Throws<ArgumentException>(() => utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), default, CultureInfo.CurrentCulture));
		}

		[TestCaseSource(nameof(GetValidDateTimeData))]
		public void ValidDateTimeOffset(DateTime? value, string dateTimeFormat, string expectedResult)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			var result = utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), default, CultureInfo.CurrentCulture);

			Assert.AreEqual(expectedResult, result);
		}

		[TestCaseSource(nameof(GetValidDateTimeOffsetData))]
		public void ValidDateTimeOffsetData(DateTimeOffset? value, string dateTimeFormat, string expectedResult)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = dateTimeFormat
			};

			var result = utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), default, CultureInfo.CurrentCulture);

			Assert.AreEqual(expectedResult, result);
		}
	}
}