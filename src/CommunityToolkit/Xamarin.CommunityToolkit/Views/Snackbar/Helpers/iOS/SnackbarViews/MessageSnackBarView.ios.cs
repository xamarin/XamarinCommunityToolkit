using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBar;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBarViews
{
	class MessageSnackBarView : BaseSnackBarView
	{
		public MessageSnackBarView(NativeSnackBar snackBar)
			: base(snackBar)
		{
		}

		protected override void Initialize()
		{
			base.Initialize();

			var messageLabel = new UILabel
			{
				Text = SnackBar.Message,
				Lines = 0,
				AdjustsFontSizeToFitWidth = true,
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

			StackView.AddArrangedSubview(messageLabel);
		}
	}
}