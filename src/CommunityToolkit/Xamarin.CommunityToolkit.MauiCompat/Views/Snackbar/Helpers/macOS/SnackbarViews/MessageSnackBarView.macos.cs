using CoreGraphics;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackBarViews
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
				StringValue = SnackBar.Message,
				Selectable = false,
				Alignment = SnackBar.Appearance.TextAlignment,
				TranslatesAutoresizingMaskIntoConstraints = false
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

			StackView?.AddArrangedSubview(messageLabel);
		}
	}
}