using System;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using static System.Math;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// The <see cref="ColorTheme"/> can be used to make the <see cref="AvatarView"/> have a consistent look. A theme consists of a set of colors that are used and applied to a variety of properties on the <see cref="AvatarView"/>. You can also implement your own theme by implementing the <see cref="IColorTheme"/> interface.
	/// </summary>
	public sealed class ColorTheme : IColorTheme
	{
		readonly Color[] backgroundColors;

		readonly Color[] foregroundColors;

		/// <summary>
		/// Constructor of <see cref="ColorTheme"/> where you can specify color sets for the foreground and background. Depending on the <see cref="AvatarView.Text"/> a random color from the set gets selected. This makes it very easy to create a consistent look.
		/// </summary>
		/// <param name="foregroundColors">List of <see cref="Color"/> that can be picked from as foreground colors</param>
		/// <param name="backgroundColors">List of <see cref="Color"/> that can be picked from as background colors</param>
		public ColorTheme(Color[] foregroundColors, Color[] backgroundColors)
		{
			if (!(foregroundColors?.Length > 0))
				throw new ArgumentException("Must not be null or empty", nameof(foregroundColors));

			if (!(backgroundColors?.Length > 0))
				throw new ArgumentException("Must not be null or empty", nameof(backgroundColors));

			this.foregroundColors = foregroundColors;
			this.backgroundColors = backgroundColors;
		}

		public Color GetForegroundColor(string text)
		{
			var textHash = Abs(text.GetHashCode());
			return foregroundColors[textHash % foregroundColors.Length];
		}

		public Color GetBackgroundColor(string text)
		{
			var textHash = Abs(text.GetHashCode());
			return backgroundColors[textHash % backgroundColors.Length];
		}

		/// <summary>
		/// Default <see cref="ColorTheme"/> used by <see cref="AvatarView.ColorTheme"/> whenever a theme is not set.
		/// </summary>
		public static readonly IColorTheme Default = new ColorTheme(
			new Color[]
			{
				Colors.FromRgb(255, 255, 255),
				Colors.FromRgb(255, 255, 255),
				Colors.FromRgb(255, 255, 255),
				Colors.FromRgb(131, 81, 102),
				Colors.FromRgb(53, 21, 61),
				Colors.FromRgb(255, 255, 255),
				Colors.FromRgb(114, 50, 75),
				Colors.FromRgb(255, 255, 255)
			},
			new Color[]
			{
				Colors.FromRgb(69, 43, 103),
				Colors.FromRgb(119, 78, 133),
				Colors.FromRgb(211, 153, 184),
				Colors.FromRgb(249, 218, 231),
				Colors.FromRgb(223, 196, 208),
				Colors.FromRgb(209, 158, 180),
				Colors.FromRgb(171, 116, 139),
				Colors.FromRgb(143, 52, 87)
			});

		/// <summary>
		/// A Junge inspirated <see cref="ColorTheme"/>.
		/// </summary>
		public static readonly IColorTheme Jungle = new ColorTheme(
			new Color[]
			{
				Colors.FromRgb(255, 255, 255),
				Colors.FromRgb(255, 255, 255),
				Colors.FromRgb(245, 245, 245),
				Colors.FromRgb(51, 61, 71),
				Colors.FromRgb(245, 245, 245),
				Colors.FromRgb(245, 245, 245)
			},
			new Color[]
			{
				Colors.FromRgb(57, 94, 102),
				Colors.FromRgb(56, 125, 122),
				Colors.FromRgb(50, 147, 111),
				Colors.FromRgb(38, 169, 108),
				Colors.FromRgb(42, 192, 22)
			});

		/// <summary>
		/// A Desert inspirated <see cref="ColorTheme"/>.
		/// </summary>
		public static readonly IColorTheme Desert = new ColorTheme(
			new Color[]
			{
				Colors.FromRgb(245, 245, 245),
				Colors.FromRgb(51, 61, 71),
				Colors.FromRgb(51, 61, 71),
				Colors.FromRgb(51, 61, 71),
				Colors.FromRgb(245, 245, 245)
			},
			new Color[]
			{
				Colors.FromRgb(133, 98, 42),
				Colors.FromRgb(230, 165, 68),
				Colors.FromRgb(215, 175, 114),
				Colors.FromRgb(239, 222, 194),
				Colors.FromRgb(168, 101, 30)
			});

		/// <summary>
		/// A Ocean inspirated <see cref="ColorTheme"/>.
		/// </summary>
		public static readonly IColorTheme Ocean = new ColorTheme(
			new Color[]
			{
				Colors.FromRgb(245, 245, 245),
				Colors.FromRgb(245, 245, 245),
				Colors.FromRgb(245, 245, 245),
				Colors.FromRgb(120, 123, 125),
				Colors.FromRgb(120, 123, 125)
			},
			new Color[]
			{
				Colors.FromRgb(53, 143, 201),
				Colors.FromRgb(107, 174, 236),
				Colors.FromRgb(76, 132, 196),
				Colors.FromRgb(196, 244, 252),
				Colors.FromRgb(172, 228, 252)
			});

		/// <summary>
		/// A Volcano inspirated <see cref="ColorTheme"/>.
		/// </summary>
		public static readonly IColorTheme Volcano = new ColorTheme(
			new Color[]
			{
				Colors.FromRgb(255, 255, 255),
				Colors.FromRgb(255, 255, 255),
				Colors.FromRgb(245, 245, 245),
				Colors.FromRgb(51, 61, 71),
				Colors.FromRgb(245, 245, 245),
				Colors.FromRgb(245, 245, 245)
			},
			new Color[]
			{
				Colors.FromRgb(216, 40, 9),
				Colors.FromRgb(235, 80, 33),
				Colors.FromRgb(250, 117, 69),
				Colors.FromRgb(252, 184, 48),
				Colors.FromRgb(41, 41, 41),
				Colors.FromRgb(17, 17, 17)
			});
	}
}