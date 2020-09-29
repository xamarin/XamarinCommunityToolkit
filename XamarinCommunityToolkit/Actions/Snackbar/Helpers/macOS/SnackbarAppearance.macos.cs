using System;
using AppKit;

namespace Xamarin.CommunityToolkit.Actions.Snackbar.Helpers.macOS
{
	class SnackbarAppearance
	{
		public NSColor Color { get; set; } = NSColor.SystemGrayColor;

		public nfloat CornerRadius { get; set; } = 5;

		public NSLineBreakMode DismissButtonLineBreakMode { get; set; } = NSLineBreakMode.ByWordWrapping;

		public NSTextAlignment MessageTextAlignment { get; set; } = NSTextAlignment.Left;
	}
}