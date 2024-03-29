﻿using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class IsNotNullOrEmptyConverter_Tests
	{
		[TestCase("Test", true)]
		[TestCase(typeof(IsNotNullOrEmptyConverter), true)]
		[TestCase(null, false)]
		[TestCase("", false)]
		public void IsNotNullOrEmptyConverter(object value, bool expectedResult)
		{
			IValueConverter isNotNullOrEmptyConverter = new IsNotNullOrEmptyConverter();

			var result = isNotNullOrEmptyConverter.Convert(value, typeof(IsNotNullOrEmptyConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}
	}
}