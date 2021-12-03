using System;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class IntToBoolConverter_Tests
	{
		[TestCase(1, true)]
		[TestCase(0, false)]
		public void IndexToArrayConverter(int value, bool expectedResult)
		{
			var intToBoolConverter = CreateConverter();

			var result = intToBoolConverter.Convert(value, typeof(IntToBoolConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCase(true, 1)]
		[TestCase(false, 0)]
		public void IndexToArrayConverterBack(bool value, int expectedResult)
		{
			var intToBoolConverter = CreateConverter();

			var result = intToBoolConverter.ConvertBack(value, typeof(IntToBoolConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCase(2.5)]
		[TestCase("")]
		public void InvalidConverterValuesThrowArgumentException(object value)
		{
			var intToBoolConverter = CreateConverter();
			Assert.Throws<ArgumentException>(() => intToBoolConverter.Convert(value, typeof(IntToBoolConverter_Tests), null, CultureInfo.CurrentCulture));
		}

		[TestCase(2.5)]
		[TestCase("")]
		public void InvalidConverterBackValuesThrowArgumentException(object value)
		{
			var intToBoolConverter = CreateConverter();
			Assert.Throws<ArgumentException>(() => intToBoolConverter.ConvertBack(value, typeof(IntToBoolConverter_Tests), null, CultureInfo.CurrentCulture));
		}

		[Test]
		public void NullConverterValuesThrowArgumentException()
		{
			var intToBoolConverter = CreateConverter();
			Assert.Throws<ArgumentNullException>(() => intToBoolConverter.Convert(null, typeof(IntToBoolConverter_Tests), null, CultureInfo.CurrentCulture));
		}

		[Test]
		public void NullConverterBackValuesThrowArgumentException()
		{
			var intToBoolConverter = CreateConverter();
			Assert.Throws<ArgumentNullException>(() => intToBoolConverter.ConvertBack(null, typeof(IntToBoolConverter_Tests), null, CultureInfo.CurrentCulture));
		}

		static IValueConverter CreateConverter() => new IntToBoolConverter();
	}
}