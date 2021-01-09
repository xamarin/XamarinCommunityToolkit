using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS
{
	class NativeSnackBarAppearance
	{
		public UIColor Background { get; set; } = UIColor.SystemBackgroundColor;

		public UIColor Foreground { get; set; } = DefaultColor;

		public UIFont Font { get; set; } = DefaultFont;

		public UITextAlignment TextAlignment { get; set; } = UITextAlignment.Left;

		public static UIColor DefaultColor { get; } = Forms.Color.Default.ToUIColor();

		public static UIFont DefaultFont { get; } = Forms.Font.Default.ToUIFont();
	}

	class NativeSnackButtonAppearance
	{
		public UIColor Background { get; set; } = DefaultColor;

		public UIColor Foreground { get; set; } = DefaultColor;

		public UIFont Font { get; set; } = DefaultFont;

		public UILineBreakMode LineBreakMode { get; set; } = UILineBreakMode.MiddleTruncation;

		public static UIColor DefaultColor { get; } = Forms.Color.Default.ToUIColor();

		public static UIFont DefaultFont { get; } = Forms.Font.Default.ToUIFont();
	}
}