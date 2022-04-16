using AppKit;
using Microsoft.Maui.Controls.Compatibility.Platform.MacOS;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS
{
	class NativeSnackBarAppearance
	{
		public NSColor Background { get; set; } = NSColor.SystemGrayColor;

		public NSColor Foreground { get; set; } = DefaultColor;

		public NSFont Font { get; set; } = DefaultFont;

		public NSTextAlignment TextAlignment { get; set; } = NSTextAlignment.Left;

		public static NSColor DefaultColor { get; } = default(Microsoft.Maui.Graphics.Color).ToNSColor();

		public static NSFont DefaultFont { get; } = Forms.Font.Default.ToNSFont();
	}

	static class NativeSnackButtonAppearance
	{
		public static NSLineBreakMode LineBreakMode { get; set; } = NSLineBreakMode.TruncatingMiddle;

		public static NSColor DefaultColor { get; } = default(Microsoft.Maui.Graphics.Color).ToNSColor();

		public static NSFont DefaultFont { get; } = Forms.Font.Default.ToNSFont();
	}
}