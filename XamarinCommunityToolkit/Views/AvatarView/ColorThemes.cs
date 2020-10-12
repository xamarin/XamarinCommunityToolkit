using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public struct ColorThemes
	{
		public static IColorTheme Default
		{
			get
			{
				var foreground = new Color[]
				{
					Color.FromRgb(255, 255, 255),
					Color.FromRgb(255, 255, 255),
					Color.FromRgb(255, 255, 255),
					Color.FromRgb(131, 81, 102),
					Color.FromRgb(53, 21, 61),
					Color.FromRgb(255, 255, 255),
					Color.FromRgb(114, 50, 75),
					Color.FromRgb(255, 255, 255)
				};

				var background = new Color[]
				{
					Color.FromRgb(69, 43, 103),
					Color.FromRgb(119, 78, 133),
					Color.FromRgb(211, 153, 184),
					Color.FromRgb(249, 218, 231),
					Color.FromRgb(223, 196, 208),
					Color.FromRgb(209, 158, 180),
					Color.FromRgb(171, 116, 139),
					Color.FromRgb(143, 52, 87)
				};

				return new ColorTheme(foreground, background);
			}
		}

		public static IColorTheme Jungle
		{
			get
			{
				var foreground = new Color[]
				{
					Color.FromRgb(255, 255, 255),
					Color.FromRgb(255, 255, 255),
					Color.FromRgb(245, 245, 245),
					Color.FromRgb(51, 61, 71),
					Color.FromRgb(245, 245, 245),
					Color.FromRgb(245, 245, 245)
				};

				var background = new Color[]
				{
					Color.FromRgb(57, 94, 102),
					Color.FromRgb(56, 125, 122),
					Color.FromRgb(50, 147, 111),
					Color.FromRgb(38, 169, 108),
					Color.FromRgb(42, 192, 22)
				};

				return new ColorTheme(foreground, background);
			}
		}

		public static IColorTheme Desert
		{
			get
			{
				var foreground = new Color[]
				{
					Color.FromRgb(245, 245, 245),
					Color.FromRgb(51, 61, 71),
					Color.FromRgb(51, 61, 71),
					Color.FromRgb(51, 61, 71),
					Color.FromRgb(245, 245, 245)

				};

				var background = new Color[]
				{
					Color.FromRgb(133, 98, 42),
					Color.FromRgb(230, 165, 68),
					Color.FromRgb(215, 175, 114),
					Color.FromRgb(239, 222, 194),
					Color.FromRgb(168, 101, 30),
				};

				return new ColorTheme(foreground, background);
			}
		}

		public static IColorTheme Ocean
		{
			get
			{
				var foreground = new Color[]
				{
					Color.FromRgb(245, 245, 245),
					Color.FromRgb(245, 245, 245),
					Color.FromRgb(245, 245, 245),
					Color.FromRgb(120, 123, 125),
					Color.FromRgb(120, 123, 125)
				};

				var background = new Color[]
				{
					Color.FromRgb(53, 143, 201),
					Color.FromRgb(107, 174, 236),
					Color.FromRgb(76, 132, 196),
					Color.FromRgb(196, 244, 252),
					Color.FromRgb(172, 228, 252)
				};

				return new ColorTheme(foreground, background);
			}
		}

		public static IColorTheme Volcano
		{
			get
			{
				var foreground = new Color[]
				{
					Color.FromRgb(255, 255, 255),
					Color.FromRgb(255, 255, 255),
					Color.FromRgb(245, 245, 245),
					Color.FromRgb(51, 61, 71),
					Color.FromRgb(245, 245, 245),
					Color.FromRgb(245, 245, 245)
				};

				var background = new Color[]
				{
					Color.FromRgb(216, 40, 9),
					Color.FromRgb(235, 80, 33),
					Color.FromRgb(250, 117, 69),
					Color.FromRgb(252, 184, 48),
					Color.FromRgb(41, 41, 41),
					Color.FromRgb(17, 17, 17)
				};

				return new ColorTheme(foreground, background);
			}
		}
	}
}
