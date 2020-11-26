using System;
using UIKit;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS
{
	class SnackBarAppearance
	{
		public UIColor BackgroundColor { get; set; } = UIColor.SystemGrayColor;

		public UIColor TextForeground { get; set; } = UIColor.SystemGrayColor;
		
		public UIColor ButtonForegroundColor { get; set; } = UIColor.SystemGrayColor;

		public UIColor ButtonBackgroundColor { get; set; } = UIColor.SystemGrayColor;

		public nfloat CornerRadius { get; set; } = 5;

		public string TextFontName { get; set; } = "Arial";

		public nfloat TextFontSize { get; set; } = 5;

		public string ButtonFontName { get; set; } = "Arial";

		public nfloat ButtonFontSize { get; set; } = 5;

		public UILineBreakMode DismissButtonLineBreakMode { get; set; } = UILineBreakMode.MiddleTruncation;

		public UITextAlignment MessageTextAlignment { get; set; } = UITextAlignment.Left;
	}
}