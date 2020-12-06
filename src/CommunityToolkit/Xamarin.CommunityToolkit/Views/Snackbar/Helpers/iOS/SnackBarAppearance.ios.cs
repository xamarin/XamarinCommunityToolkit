using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS
{
	class SnackBarAppearance
	{
		public UIColor BackgroundColor { get; set; } = DefaultColor;

		public UIColor TextForeground { get; set; } = DefaultColor;

		public UIColor ButtonBackgroundColor { get; set; } = DefaultColor;

		public UIColor ButtonForegroundColor { get; set; } = DefaultColor;

		public nfloat CornerRadius { get; set; } = 5;

		public UIFont TextFont { get; set; } = DefaultFont;

		public UIFont ButtonFont { get; set; } = DefaultFont;

		public UILineBreakMode DismissButtonLineBreakMode { get; set; } = UILineBreakMode.MiddleTruncation;

		public UITextAlignment MessageTextAlignment { get; set; } = UITextAlignment.Left;

		public static UIColor DefaultColor = Forms.Color.Default.ToUIColor();

		public static UIFont DefaultFont = Forms.Font.Default.ToUIFont();
	}
}