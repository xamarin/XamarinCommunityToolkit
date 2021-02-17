using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	internal enum TestEnumForEnumToBoolConverter
	{
		None = 0,
		One = 1,
		Two = 2,
		Three = 3,
		Four = 4,
		Five = 5,
		Six = 6
	}

	public class EnumToBoolConverter_Tests
	{
		[Fact]
		public void EnumToBoolConvertBack_ThrowsNotImplementedException()
		{
			var enumToBoolConverter = new EnumToBoolConverter();

			Assert.Throws<NotImplementedException>(() =>
				enumToBoolConverter.ConvertBack(TestEnumForEnumToBoolConverter.Five, typeof(bool), null,
					CultureInfo.InvariantCulture));
		}

		[Theory]
		[InlineData("a string")]
		[InlineData(42)]
		[InlineData(null)]
		[InlineData(false)]
		public void EnumToBoolConvert_ValueNotEnum_ThrowsArgumentException(object value)
		{
			var enumToBoolConverter = new EnumToBoolConverter();

			Assert.Throws<ArgumentException>("value", () => enumToBoolConverter.Convert(value, typeof(bool),
				TestEnumForEnumToBoolConverter.Five, CultureInfo.InvariantCulture));
		}

		[Theory]
		[InlineData("a string")]
		[InlineData(42)]
		[InlineData(null)]
		[InlineData(false)]
		public void EnumToBoolConvert_ParameterNotEnum_ThrowsArgumentException(object parameter)
		{
			var enumToBoolConverter = new EnumToBoolConverter();

			Assert.Throws<ArgumentException>("parameter", () => enumToBoolConverter.Convert(TestEnumForEnumToBoolConverter.Five, typeof(bool),
				parameter, CultureInfo.InvariantCulture));
		}

		[Theory]
		[MemberData(nameof(ConvertTestDate))]
		public void EnumToBoolConvert_TrueValidation(Enum[] trueValues, object value, object parameter,
			bool expectedResult)
		{
			var enumToBoolConverter = new EnumToBoolConverter
			{
				TrueValues = trueValues ?? new Enum[0]
			};

			var result = enumToBoolConverter.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture);
			Assert.Equal(expectedResult, result);
		}

		public static IEnumerable<object[]> ConvertTestDate()
		{
			yield return new object[]
			{
				null, TestEnumForEnumToBoolConverter.Five, TestEnumForEnumToBoolConverter.Five, true
			};
			yield return new object[]
			{
				null, TestEnumForEnumToBoolConverter.Five, TestEnumForEnumToBoolConverter.Six, false
			};
			yield return new object[]
			{
				new[] { TestEnumForEnumToBoolConverter.Five, TestEnumForEnumToBoolConverter.Six }, TestEnumForEnumToBoolConverter.Five, TestEnumForEnumToBoolConverter.Six, true
			};
			yield return new object[]
			{
				new[] { TestEnumForEnumToBoolConverter.Five, TestEnumForEnumToBoolConverter.Six }, TestEnumForEnumToBoolConverter.Six, null, true
			};
			yield return new object[]
			{
				new[] { TestEnumForEnumToBoolConverter.Five, TestEnumForEnumToBoolConverter.Six }, TestEnumForEnumToBoolConverter.One, TestEnumForEnumToBoolConverter.One, false
			};
			yield return new object[]
			{
				new[] { TestEnumForEnumToBoolConverter.Five, TestEnumForEnumToBoolConverter.Six }, TestEnumForEnumToBoolConverter.Two, null, false
			};
		}
	}
}