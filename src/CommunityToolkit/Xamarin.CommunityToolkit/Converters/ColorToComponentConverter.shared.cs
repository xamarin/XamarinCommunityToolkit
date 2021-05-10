using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	public class ColorToByteAlphaConverter : BaseConverterOneWay<Color, byte>
	{
		public override byte ConvertFrom(Color value) => value.GetByteAlpha();
	}

	public class ColorToByteRedConverter : BaseConverterOneWay<Color, byte>
	{
		public override byte ConvertFrom(Color value) => value.GetByteRed();
	}

	public class ColorToByteGreenConverter : BaseConverterOneWay<Color, byte>
	{
		public override byte ConvertFrom(Color value) => value.GetByteGreen();
	}

	public class ColorToByteBlueConverter : BaseConverterOneWay<Color, byte>
	{
		public override byte ConvertFrom(Color value) => value.GetByteBlue();
	}

	public class ColorToPercentCyanConverter : BaseConverterOneWay<Color, double>
	{
		public override double ConvertFrom(Color value) => value.GetPercentCyan();
	}

	public class ColorToPercentMagentaConverter : BaseConverterOneWay<Color, double>
	{
		public override double ConvertFrom(Color value) => value.GetPercentMagenta();
	}

	public class ColorToPercentYellowConverter : BaseConverterOneWay<Color, double>
	{
		public override double ConvertFrom(Color value) => value.GetPercentYellow();
	}

	public class ColorToBlackKeyConverter : BaseConverterOneWay<Color, double>
	{
		public override double ConvertFrom(Color value) => value.GetPercentBlackKey();
	}

	// Hue is a degree on the color wheel from 0 to 360. 0 is red, 120 is green, 240 is blue.
	public class ColorToDegreeHueConverter : BaseConverterOneWay<Color, double>
	{
		public override double ConvertFrom(Color value) => value.GetDegreeHue();
	}
}