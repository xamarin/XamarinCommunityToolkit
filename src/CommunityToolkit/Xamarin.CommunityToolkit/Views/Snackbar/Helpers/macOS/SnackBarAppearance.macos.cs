using AppKit;
using CoreGraphics;
using Xamarin.Forms.Platform.MacOS;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS
{
	class NativeSnackBarAppearance
	{
		public NSColor Background { get; set; } = NSColor.SystemGrayColor;

		public NSColor Foreground { get; set; } = DefaultColor;

		public NSFont Font { get; set; } = DefaultFont;

		public CGRect Padding { get; set; } = DefaultPadding;

		public NSTextAlignment TextAlignment { get; set; } = NSTextAlignment.Left;

		public static NSColor DefaultColor { get; } = Forms.Color.Default.ToNSColor();

		public static NSFont DefaultFont { get; } = Forms.Font.Default.ToNSFont();

		public static CGRect DefaultPadding { get; } = new CGRect(0, 0, 0, 0);
	}

	static class NativeSnackButtonAppearance
	{
		public static NSLineBreakMode LineBreakMode { get; set; } = NSLineBreakMode.TruncatingMiddle;

		public static NSColor DefaultColor { get; } = Forms.Color.Default.ToNSColor();

		public static NSFont DefaultFont { get; } = Forms.Font.Default.ToNSFont();
	}
}