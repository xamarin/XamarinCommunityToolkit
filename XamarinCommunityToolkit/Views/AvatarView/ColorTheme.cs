using System;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class ColorTheme : ColorThemeBase
	{
		public ColorTheme(Color[] foregroundColors, Color[] backgroundColors)
		{
			if (foregroundColors == null || foregroundColors.Count() <= 0)
			{
				throw new ArgumentException($"{nameof(foregroundColors)} must not be null or empty");
			}

			if (backgroundColors == null || backgroundColors.Count() <= 0)
			{
				throw new ArgumentException($"{nameof(backgroundColors)} must not be null or empty");
			}

			ForegroundColors = foregroundColors;
			BackgroundColors = backgroundColors;
		}
	}
}
