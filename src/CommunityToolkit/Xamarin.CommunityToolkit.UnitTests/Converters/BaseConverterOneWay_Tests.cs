using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	[TestOf(typeof(BaseConverterOneWay<string, Color>))]
	public class BaseConverterOneWay_Tests
	{
		#region AllowsNullOrDefault = true

		/// <summary>
		/// Derivation of <see cref="BaseConverterOneWay{TFrom,TTo}"/> to test the new <see cref="AllowsNullOrDefault"/> handling, allowing null.
		/// </summary>
		private class ColorNameToColorConverter : BaseConverterOneWay<string, Color>
		{
			protected override bool AllowsNullOrDefault => true;

			public override Color ConvertFrom(string? value) =>
				value switch
				{
					// By setting AllowsNullOrDefault to true, it is possible to handle this case by the derived class.
					null => Color.Black,
					"Red" => Color.Red,
					"Blue" => Color.Blue,
					_ => throw new ArgumentException($"{value} unknown.")
				};
		}

		static IEnumerable<object?[]> GetTestDataWithNull()
		{
			yield return new object?[] { "Red", Color.Red };
			yield return new object?[] { "Blue", Color.Blue };
			yield return new object?[] { null, Color.Black };
		}

		[TestCaseSource(nameof(GetTestDataWithNull))]
		public void ColorNameToColorConverterConvert(string value, Color expectedResult)
		{
			var colorNameToColorConverter = new ColorNameToColorConverter();
			var result = colorNameToColorConverter.Convert(value, typeof(Color), null!, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCase("")]
		[TestCase("Green")]
		[TestCase("red")]
		public void ColorNameToColorConverterConvertInvalidValuesThrowArgumentException(string value)
		{
			var colorNameToColorConverter = new ColorNameToColorConverter();
			Assert.Throws<ArgumentException>(() => colorNameToColorConverter.Convert(value, typeof(Color), null!, CultureInfo.CurrentCulture));
		}

		#endregion

		#region AllowsNullOrDefault = false

		/// <summary>
		/// Derivation of <see cref="BaseConverterOneWay{TFrom,TTo}"/> to test the new <see cref="AllowsNullOrDefault"/> handling, disallowing null.
		/// </summary>
		private class ColorNameToColorConverterDisallowingNull : ColorNameToColorConverter
		{
			// By setting AllowsNullOrDefault to false, it is no longer possible to handle null and instead we expect an ArgumentException.
			protected override bool AllowsNullOrDefault => false;
		}

		static IEnumerable<object?[]> GetTestDataWithoutNull()
		{
			yield return new object?[] { "Red", Color.Red };
			yield return new object?[] { "Blue", Color.Blue };
		}

		[TestCaseSource(nameof(GetTestDataWithoutNull))]
		public void ColorNameToColorConverterDisallowingNullConvert(string value, Color expectedResult)
		{
			var colorNameToColorConverterDisallowingNull = new ColorNameToColorConverterDisallowingNull();
			var result = colorNameToColorConverterDisallowingNull.Convert(value, typeof(Color), null!, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[TestCase("")]
		[TestCase("Green")]
		[TestCase("red")]
		[TestCase(null)]
		public void ColorNameToColorConverterDisallowingNullConvertInvalidValuesThrowArgumentException(string value)
		{
			var colorNameToColorConverterDisallowingNull = new ColorNameToColorConverterDisallowingNull();
			Assert.Throws<ArgumentException>(() => colorNameToColorConverterDisallowingNull.Convert(value, typeof(Color), null!, CultureInfo.CurrentCulture));
		}
		#endregion
	}
}