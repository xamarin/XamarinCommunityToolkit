
using iColor = UIKit.UIColor;

namespace Xamarin.CommunityToolkit.MauiCompat
{
	public static partial class MauiColorExtensions
	{
		public static iColor ToUIColor(this Microsoft.Maui.Graphics.Color self)
		{
			var colorToConvert = self;
			if (colorToConvert == null)
			{
				colorToConvert = Microsoft.Maui.Graphics.Colors.Transparent;
			}

			return Microsoft.Maui.Platform.ColorExtensions.ToPlatform(colorToConvert);
		}
	}
}