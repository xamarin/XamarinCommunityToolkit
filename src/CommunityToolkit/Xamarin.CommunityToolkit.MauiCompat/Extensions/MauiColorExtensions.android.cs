
using AColor = Android.Graphics.Color;

namespace Xamarin.CommunityToolkit.MauiCompat
{
	public static partial class MauiColorExtensions
	{
		public static AColor ToAndroid(this Microsoft.Maui.Graphics.Color self)
		{
			var colorToConvert = self;

			if (colorToConvert == null)
			{
				colorToConvert = Microsoft.Maui.Graphics.Colors.Transparent;
			}

			return Microsoft.Maui.Controls.Compatibility.Platform.Android.ColorExtensions.ToAndroid(colorToConvert);
		}
	}
}