using System;using Microsoft.Extensions.Logging;
using CoreGraphics;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBar;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBarViews
{
	class MessageSnackBarView : BaseSnackBarView
	{
		public MessageSnackBarView(NativeSnackBar snackBar)
			: base(snackBar)
		{
		}

		protected override void Initialize(CGRect cornerRadius)
		{
			base.Initialize(cornerRadius);

			var messageLabel = new PaddedLabel(SnackBar.Layout.PaddingLeft,
				SnackBar.Layout.PaddingTop,
				SnackBar.Layout.PaddingRight,
				SnackBar.Layout.PaddingBottom)
			{
				Text = SnackBar.Message,
				Lines = 0,
				TextAlignment = SnackBar.Appearance.TextAlignment
			};
			if (SnackBar.Appearance.Background != NativeSnackBarAppearance.DefaultColor)
			{
				messageLabel.BackgroundColor = SnackBar.Appearance.Background;
			}

			if (SnackBar.Appearance.Foreground != NativeSnackBarAppearance.DefaultColor)
			{
				messageLabel.TextColor = SnackBar.Appearance.Foreground;
			}

			if (SnackBar.Appearance.Font != NativeSnackBarAppearance.DefaultFont)
			{
				messageLabel.Font = SnackBar.Appearance.Font;
			}

			_ = StackView ?? throw new NullReferenceException();
			StackView.AddArrangedSubview(messageLabel);
		}
	}
}