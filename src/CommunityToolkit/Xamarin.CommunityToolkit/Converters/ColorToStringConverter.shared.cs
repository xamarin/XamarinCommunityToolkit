using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	public class ColorToRgbStringConverter : BaseConverterOneWay<Color, string>
	{
		public override string ConvertFrom(Color value) => value.ToRgbString();
	}

	public class ColorToRgbaStringConverter : BaseConverterOneWay<Color, string>
	{
		public override string ConvertFrom(Color value) => value.ToRgbaString();
	}

	public class ColorToHexRgbStringConverter : BaseConverter<Color, string>
	{
		public override string ConvertFrom(Color value) => value.ToHexRgbString();

		public override Color ConvertBackTo(string value) => Color.FromHex(value);
	}

	public class ColorToHexRgbaStringConverter : BaseConverter<Color, string>
	{
		public override string ConvertFrom(Color value) => value.ToHexRgbaString();

		public override Color ConvertBackTo(string value) => Color.FromHex(value);
	}

	public class ColorToCmykStringConverter : BaseConverterOneWay<Color, string>
	{
		public override string ConvertFrom(Color value) => value.ToCmykString();
	}

	public class ColorToCmykaStringConverter : BaseConverterOneWay<Color, string>
	{
		public override string ConvertFrom(Color value) => value.ToCmykaString();
	}

	public class ColorToHslStringConverter : BaseConverterOneWay<Color, string>
	{
		public override string ConvertFrom(Color value) => value.ToHslString();
	}

	public class ColorToHslaStringConverter : BaseConverterOneWay<Color, string>
	{
		public override string ConvertFrom(Color value) => value.ToHslaString();
	}
}