using System;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class InvertedBoolConverter_Tests
	{
		[TestCase(true, false)]
		[TestCase(false, true)]
		public void InverterBoolConverter(bool value, bool expectedResult)
		{
			var inverterBoolConverter = CreateConverter();

			var result = inverterBoolConverter.Convert(value, typeof(InvertedBoolConverter_Tests), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCase(2)]
		[TestCase("")]
		public void InvalidConverterValuesThrowArgumentException(object value)
		{
			var inverterBoolConverter = CreateConverter();
			Assert.Throws<ArgumentException>(() => inverterBoolConverter.Convert(value, typeof(IndexToArrayItemConverter), null, CultureInfo.CurrentCulture));
		}

		[Test]
		public void NullThrowsArgumentNullException()
		{
			var inverterBoolConverter = CreateConverter();
			Assert.Throws<ArgumentNullException>(() => inverterBoolConverter.Convert(null, typeof(IndexToArrayItemConverter), null, CultureInfo.CurrentCulture));
		}

		static IValueConverter CreateConverter() => new InvertedBoolConverter();
	}
}