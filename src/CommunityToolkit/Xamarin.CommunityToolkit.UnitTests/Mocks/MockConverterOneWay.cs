using System;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Mocks
{
	/// <summary>
	/// Mock to test the <see cref="BaseNullableConverterOneWay{TFrom, TTo}"/> internals.
	/// </summary>
	public class MockNullableConverterOneWay : BaseNullableConverterOneWay<string, Color>
	{
		public override Color ConvertFrom(string? value) => value switch
		{
			null => Color.Black,
			"Red" => Color.Red,
			"Blue" => Color.Blue,
			_ => throw new ArgumentException($"{value} unknown.")
		};
	}

	/// <summary>
	/// Mock to test the <see cref="BaseConverterOneWay{TFrom, TTo}"/> internals.
	/// </summary>
	public class MockConverterOneWay : BaseConverterOneWay<string, Color>
	{
		public override Color ConvertFrom(string value) => value switch
		{
			"Red" => Color.Red,
			"Blue" => Color.Blue,
			_ => throw new ArgumentException($"{value} unknown.")
		};
	}
}