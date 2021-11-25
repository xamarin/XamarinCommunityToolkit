using System;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Mocks
{
	/// <summary>
	/// Derivation of <see cref="BaseConverterOneWay{TFrom,TTo}"/> to test the new <see cref="AllowsNullOrDefault"/> handling.
	/// </summary>
	public class MockConverterOneWay : BaseConverterOneWay<string, Color>
	{
		protected override bool AllowsNullOrDefault { get; }

		public MockConverterOneWay(bool allowsNullOrDefault)
		{
			AllowsNullOrDefault = allowsNullOrDefault;
		}

		public override Color ConvertFrom(string? value) =>
			value switch
			{
				// By setting AllowsNullOrDefault to true, it is possible to handle this case.
				null => Color.Black,
				"Red" => Color.Red,
				"Blue" => Color.Blue,
				_ => throw new ArgumentException($"{value} unknown.")
			};
	}
}
