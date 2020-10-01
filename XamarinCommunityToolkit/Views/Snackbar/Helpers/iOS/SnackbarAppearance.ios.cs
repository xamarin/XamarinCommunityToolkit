using System;
using UIKit;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS
{
	class SnackbarAppearance
	{
		public UIColor Color { get; set; } = UIColor.SystemGrayColor;

		public nfloat CornerRadius { get; set; } = 5;

		public UILineBreakMode DismissButtonLineBreakMode { get; set; } = UILineBreakMode.MiddleTruncation;

		public UITextAlignment MessageTextAlignment { get; set; } = UITextAlignment.Left;
	}
}