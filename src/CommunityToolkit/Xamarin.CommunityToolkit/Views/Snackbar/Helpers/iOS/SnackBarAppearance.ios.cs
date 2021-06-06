using System;
using UIKit;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms.Platform.iOS;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS
{
	class NativeSnackBarAppearance
	{
		public UIColor Background { get; set; } = XCT.IsiOS13OrNewer ? UIColor.SystemGrayColor : UIColor.Gray;

		public UIColor Foreground { get; set; } = DefaultColor;

		public UIFont Font { get; set; } = DefaultFont;

		public UITextAlignment TextAlignment { get; set; } = UITextAlignment.Left;

		public static UIColor DefaultColor { get; } = XCT.IsiOS13OrNewer ? Forms.Color.Default.ToUIColor() : UIColor.White;

		public static UIFont DefaultFont { get; } = Forms.Font.Default.ToUIFont();
	}

	static class NativeSnackButtonAppearance
	{
		public static UILineBreakMode LineBreakMode { get; set; } = UILineBreakMode.MiddleTruncation;

		public static UIColor DefaultColor { get; } = Forms.Color.Default.ToUIColor();

		public static UIFont DefaultFont { get; } = Forms.Font.Default.ToUIFont();
	}
}