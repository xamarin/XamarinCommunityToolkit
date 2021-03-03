using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.CommunityToolkit.Converters;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public enum TestEnumForEnumToBoolConverter
	{
		None = 0,
		One = 1,
		Two = 2,
		Three = 3,
		Four = 4,
		Five = 5,
		Six = 6
	}

	[Flags]
	public enum TestFlaggedEnumForEnumToBoolConverter
	{
		None = 0b0000,
		One = 0b0001,
		Two = 0b0010,
		Three = 0b0100,
		Four = 0b1000
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
		[InlineData(TestFlaggedEnumForEnumToBoolConverter.Four)]
		public void EnumToBoolConvert_ParameterNotSameEnum_ReturnsFalse(object parameter)
		{
			var enumToBoolConverter = new EnumToBoolConverter();

			var result = enumToBoolConverter.Convert(TestEnumForEnumToBoolConverter.Five, typeof(bool), parameter, CultureInfo.InvariantCulture);

			Assert.False(result as bool?);
		}

		[Theory]
		[MemberData(nameof(ConvertTestData))]
		public void EnumToBoolConvert_Validation(object[] trueValues, object value, object parameter,
			bool expectedResult)
		{
			var enumToBoolConverter = new EnumToBoolConverter();
			trueValues?.OfType<Enum>().ToList().ForEach(fe => enumToBoolConverter.TrueValues.Add(fe));

			var result = enumToBoolConverter.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture);
			Assert.Equal(expectedResult, result);
		}

		public static IEnumerable<object[]> ConvertTestData()
		{
			// Simple enum
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
				new object[] { TestEnumForEnumToBoolConverter.Five, TestEnumForEnumToBoolConverter.Six }, TestEnumForEnumToBoolConverter.Five, TestEnumForEnumToBoolConverter.Six, true
			};
			yield return new object[]
			{
				new object[] { TestEnumForEnumToBoolConverter.Five, TestEnumForEnumToBoolConverter.Six }, TestEnumForEnumToBoolConverter.Six, null, true
			};
			yield return new object[]
			{
				new object[] { TestEnumForEnumToBoolConverter.Five, TestEnumForEnumToBoolConverter.Six }, TestEnumForEnumToBoolConverter.One, TestEnumForEnumToBoolConverter.Five, false
			};
			yield return new object[]
			{
				new object[] { TestEnumForEnumToBoolConverter.Five, TestEnumForEnumToBoolConverter.Six }, TestEnumForEnumToBoolConverter.Two, null, false
			};

			// Flagged enum
			yield return new object[]
			{
				new object[] { (TestFlaggedEnumForEnumToBoolConverter.One | TestFlaggedEnumForEnumToBoolConverter.Three), TestFlaggedEnumForEnumToBoolConverter.Two }, TestFlaggedEnumForEnumToBoolConverter.One, null, true
			};
			yield return new object[]
			{
				new object[] { (TestFlaggedEnumForEnumToBoolConverter.One | TestFlaggedEnumForEnumToBoolConverter.Three), TestFlaggedEnumForEnumToBoolConverter.Two }, TestFlaggedEnumForEnumToBoolConverter.Two, null, true
			};
			yield return new object[]
			{
				new object[] { (TestFlaggedEnumForEnumToBoolConverter.One | TestFlaggedEnumForEnumToBoolConverter.Three), TestFlaggedEnumForEnumToBoolConverter.Two }, TestFlaggedEnumForEnumToBoolConverter.Three, null, true
			};
			yield return new object[]
			{
				new object[] { (TestFlaggedEnumForEnumToBoolConverter.One | TestFlaggedEnumForEnumToBoolConverter.Three), TestFlaggedEnumForEnumToBoolConverter.Two }, TestFlaggedEnumForEnumToBoolConverter.Four, null, false
			};
			yield return new object[]
			{
				null, TestFlaggedEnumForEnumToBoolConverter.One, (TestFlaggedEnumForEnumToBoolConverter.One | TestFlaggedEnumForEnumToBoolConverter.Three), true
			};
			yield return new object[]
			{
				null, TestFlaggedEnumForEnumToBoolConverter.Three, (TestFlaggedEnumForEnumToBoolConverter.One | TestFlaggedEnumForEnumToBoolConverter.Three), true
			};
			yield return new object[]
			{
				null, TestFlaggedEnumForEnumToBoolConverter.Two, (TestFlaggedEnumForEnumToBoolConverter.One | TestFlaggedEnumForEnumToBoolConverter.Three), false
			};
			yield return new object[]
			{
				null, (TestFlaggedEnumForEnumToBoolConverter.One | TestFlaggedEnumForEnumToBoolConverter.Three), (TestFlaggedEnumForEnumToBoolConverter.One | TestFlaggedEnumForEnumToBoolConverter.Three), true
			};
			yield return new object[]
			{
				null, (TestFlaggedEnumForEnumToBoolConverter.One | TestFlaggedEnumForEnumToBoolConverter.Two | TestFlaggedEnumForEnumToBoolConverter.Three), (TestFlaggedEnumForEnumToBoolConverter.One | TestFlaggedEnumForEnumToBoolConverter.Three), false
			};
		}
	}
}