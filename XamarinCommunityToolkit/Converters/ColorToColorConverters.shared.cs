using System;
using System.Globalization;

using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	public class ColorToBlackOrWhiteConverter : BaseConverterOneWay<Color, Color>
	{
		public override Color ConvertFrom(Color value) => value.ToBlackOrWhite();
	}

	public class ColorToColorForTextConverter : BaseConverterOneWay<Color, Color>
	{
		public override Color ConvertFrom(Color value) => value.ToBlackOrWhiteForText();
	}

	public class ColorToGrayScaleColorConverter : BaseConverterOneWay<Color, Color>
	{
		public override Color ConvertFrom(Color value) => value.ToGrayScale();
	}

	public class ColorToInverseColorConverter : BaseConverterOneWay<Color, Color>
	{
		public override Color ConvertFrom(Color value) => value.ToInverseColor();
	}
}