using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class UtcDateTimeToLocalStringConverter_Tests
	{
		public const string TestValue = nameof(TestValue);

		public static IEnumerable<object?[]> GetData() => new List<object?[]>
		{
			new object[] { DateTime.UtcNow.Ticks, DateTime.Now.ToLocalTime() },
			new object[] { new List<string>() { "TestValue" }, false },
			new object?[] { null, true },
			new object[] { Enumerable.Range(1, 3), false },
		};

		[TestCaseSource(nameof(GetData))]
		public void IsEqual(object value, object comparedValue)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter();

			var result = utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), comparedValue, CultureInfo.CurrentCulture);

			Assert.IsInstanceOf<bool>(result);
			Assert.IsTrue((bool)result);
		}

		[TestCaseSource(nameof(GetData))]
		public void IsNotEqual(object value, object comparedValue)
		{
			var utcDateTimeToLocalStringConverter = new UtcDateTimeToLocalStringConverter();

			var result = utcDateTimeToLocalStringConverter.Convert(value, typeof(UtcDateTimeToLocalStringConverter_Tests), comparedValue, CultureInfo.CurrentCulture);

			Assert.IsInstanceOf<bool>(result);
			Assert.False((bool)result);
		}
	}
}