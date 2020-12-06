using System;
using AppKit;
using Xamarin.Forms.Platform.MacOS;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS
{
	class SnackBarAppearance
	{
		public NSColor BackgroundColor { get; set; } = DefaultColor;

		public NSColor TextForeground { get; set; } = DefaultColor;

		public NSColor ButtonBackgroundColor { get; set; } = DefaultColor;

		public NSColor ButtonForegroundColor { get; set; } = DefaultColor;

		public nfloat CornerRadius { get; set; } = 5;

		public NSFont TextFont { get; set; } = DefaultFont;

		public NSFont ButtonFont { get; set; } = DefaultFont;

		public NSLineBreakMode DismissButtonLineBreakMode { get; set; } = NSLineBreakMode.ByWordWrapping;

		public NSTextAlignment MessageTextAlignment { get; set; } = NSTextAlignment.Left;

		public static NSColor DefaultColor = Forms.Color.Default.ToNSColor();

		public static NSFont DefaultFont = Forms.Font.Default.ToNSFont();
	}
}