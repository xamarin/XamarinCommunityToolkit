using System;
using AppKit;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS
{
	class SnackBarAppearance
	{
		public NSColor BackgroundColor { get; set; } = NSColor.SystemGrayColor;

		public NSColor TextForeground { get; set; } = NSColor.SystemGrayColor;

		public NSColor ButtonForegroundColor { get; set; } = NSColor.SystemGrayColor;

		public NSColor ButtonBackgroundColor { get; set; } = NSColor.SystemGrayColor;

		public nfloat CornerRadius { get; set; } = 5;

		public string TextFontName { get; set; } = "Arial";

		public nfloat TextFontSize { get; set; } = 5;

		public string ButtonFontName { get; set; } = "Arial";

		public nfloat ButtonFontSize { get; set; } = 5;

		public NSLineBreakMode DismissButtonLineBreakMode { get; set; } = NSLineBreakMode.ByWordWrapping;

		public NSTextAlignment MessageTextAlignment { get; set; } = NSTextAlignment.Left;
	}
}