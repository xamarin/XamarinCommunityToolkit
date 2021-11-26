using System;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Mocks
{
	/// <summary>
	/// Mock to test the new <see cref="BaseConverterOneWay{TFrom, TTo}.AllowsNull"/> handling.
	/// </summary>
	public class MockConverterOneWay : BaseConverterOneWay<string, Color>
	{
		protected override bool AllowsNull { get; }

		public MockConverterOneWay(bool allowsNull)
		{
			AllowsNull = allowsNull;
		}

		public override Color ConvertFrom(string? value) =>
			value switch
			{
				// By setting AllowsNull to true, it is possible to handle this case.
				null => Color.Black,
				"Red" => Color.Red,
				"Blue" => Color.Blue,
				_ => throw new ArgumentException($"{value} unknown.")
			};
	}
}
