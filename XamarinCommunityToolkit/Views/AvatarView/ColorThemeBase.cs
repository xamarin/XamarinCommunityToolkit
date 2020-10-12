using System;
using Xamarin.Forms;
using static System.Math;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public abstract class ColorThemeBase : IColorTheme
	{
		public Color[] BackgroundColors { get; set; }

		public Color[] ForegroundColors { get; set; }

		public Color GetForegroundColor(string text)
		{
			var textHash = Abs(text.GetHashCode());

			return ForegroundColors[textHash % ForegroundColors.Length];
		}

		public Color GetBackgroundColor(string text)
		{
			var textHash = Abs(text.GetHashCode());

			return BackgroundColors[textHash % BackgroundColors.Length];
		}
	}
}
