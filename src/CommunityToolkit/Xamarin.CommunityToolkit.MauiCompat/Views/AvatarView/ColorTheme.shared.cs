using System;using Microsoft.Extensions.Logging;
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
				Color.FromRgb(255, 255, 255),
				Color.FromRgb(255, 255, 255),
				Color.FromRgb(255, 255, 255),
				Color.FromRgb(131, 81, 102),
				Color.FromRgb(53, 21, 61),
				Color.FromRgb(255, 255, 255),
				Color.FromRgb(114, 50, 75),
				Color.FromRgb(255, 255, 255)
			},
			new Color[]
			{
				Color.FromRgb(69, 43, 103),
				Color.FromRgb(119, 78, 133),
				Color.FromRgb(211, 153, 184),
				Color.FromRgb(249, 218, 231),
				Color.FromRgb(223, 196, 208),
				Color.FromRgb(209, 158, 180),
				Color.FromRgb(171, 116, 139),
				Color.FromRgb(143, 52, 87)
			});

		/// <summary>
		/// A Junge inspirated <see cref="ColorTheme"/>.
		/// </summary>
		public static readonly IColorTheme Jungle = new ColorTheme(
			new Color[]
			{
				Color.FromRgb(255, 255, 255),
				Color.FromRgb(255, 255, 255),
				Color.FromRgb(245, 245, 245),
				Color.FromRgb(51, 61, 71),
				Color.FromRgb(245, 245, 245),
				Color.FromRgb(245, 245, 245)
			},
			new Color[]
			{
				Color.FromRgb(57, 94, 102),
				Color.FromRgb(56, 125, 122),
				Color.FromRgb(50, 147, 111),
				Color.FromRgb(38, 169, 108),
				Color.FromRgb(42, 192, 22)
			});

		/// <summary>
		/// A Desert inspirated <see cref="ColorTheme"/>.
		/// </summary>
		public static readonly IColorTheme Desert = new ColorTheme(
			new Color[]
			{
				Color.FromRgb(245, 245, 245),
				Color.FromRgb(51, 61, 71),
				Color.FromRgb(51, 61, 71),
				Color.FromRgb(51, 61, 71),
				Color.FromRgb(245, 245, 245)
			},
			new Color[]
			{
				Color.FromRgb(133, 98, 42),
				Color.FromRgb(230, 165, 68),
				Color.FromRgb(215, 175, 114),
				Color.FromRgb(239, 222, 194),
				Color.FromRgb(168, 101, 30)
			});

		/// <summary>
		/// A Ocean inspirated <see cref="ColorTheme"/>.
		/// </summary>
		public static readonly IColorTheme Ocean = new ColorTheme(
			new Color[]
			{
				Color.FromRgb(245, 245, 245),
				Color.FromRgb(245, 245, 245),
				Color.FromRgb(245, 245, 245),
				Color.FromRgb(120, 123, 125),
				Color.FromRgb(120, 123, 125)
			},
			new Color[]
			{
				Color.FromRgb(53, 143, 201),
				Color.FromRgb(107, 174, 236),
				Color.FromRgb(76, 132, 196),
				Color.FromRgb(196, 244, 252),
				Color.FromRgb(172, 228, 252)
			});

		/// <summary>
		/// A Volcano inspirated <see cref="ColorTheme"/>.
		/// </summary>
		public static readonly IColorTheme Volcano = new ColorTheme(
			new Color[]
			{
				Color.FromRgb(255, 255, 255),
				Color.FromRgb(255, 255, 255),
				Color.FromRgb(245, 245, 245),
				Color.FromRgb(51, 61, 71),
				Color.FromRgb(245, 245, 245),
				Color.FromRgb(245, 245, 245)
			},
			new Color[]
			{
				Color.FromRgb(216, 40, 9),
				Color.FromRgb(235, 80, 33),
				Color.FromRgb(250, 117, 69),
				Color.FromRgb(252, 184, 48),
				Color.FromRgb(41, 41, 41),
				Color.FromRgb(17, 17, 17)
			});
	}
}