using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class UtcDateTimeToLocalStringConverter_Tests
	{
		public static IEnumerable<object?[]> GetPassingData() => new List<object?[]>
		{
			new object[] { DateTime.UtcNow, DateTime.Now.ToLocalTime().ToString("G") },
			new object[] { DateTimeOffset.UtcNow, DateTime.Now.ToLocalTime().ToString("G") }
		};

		[TestCaseSource(nameof(GetPassingData))]
		public void UtcDateTimeToLocalStringConvert_Validation(DateTimeOffset value, string expectedResult)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter()
			{
				DateTimeFormat = "G"
			};

			var result = utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(expectedResult, result);
		}
	}
}