using AppKit;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackBarViews
{
	class MessageSnackBarView : BaseSnackBarView
	{
		public MessageSnackBarView(NativeSnackBar snackBar)
			: base(snackBar)
		{
		}

		public NSStackView StackView { get; set; }

		protected override void ConstrainChildren()
		{
			base.ConstrainChildren();
			StackView.LeadingAnchor.ConstraintEqualToAnchor(LeadingAnchor, SnackBar.Layout.PaddingLeading).Active = true;
			StackView.TrailingAnchor.ConstraintEqualToAnchor(TrailingAnchor, -SnackBar.Layout.PaddingTrailing).Active = true;
			StackView.BottomAnchor.ConstraintEqualToAnchor(BottomAnchor, -SnackBar.Layout.PaddingBottom).Active = true;
			StackView.TopAnchor.ConstraintEqualToAnchor(TopAnchor, SnackBar.Layout.PaddingTop).Active = true;
		}

		protected override void Initialize()
		{
			base.Initialize();
			StackView = new NSStackView();
			StackView.NeedsLayout = true;
			AddSubview(StackView);
			if (SnackBar.Appearance.BackgroundColor != SnackBarAppearance.DefaultColor)
			{
				StackView.Layer.BackgroundColor = SnackBar.Appearance.BackgroundColor.CGColor;
			}

			StackView.Orientation = NSUserInterfaceLayoutOrientation.Horizontal;
			StackView.TranslatesAutoresizingMaskIntoConstraints = false;
			StackView.Spacing = 5;
			var messageLabel = new NSTextField
			{
				StringValue = SnackBar.Message,
				Selectable = false,
				Alignment = SnackBar.Appearance.MessageTextAlignment,
				LineBreakMode = SnackBar.Appearance.DismissButtonLineBreakMode,
				TranslatesAutoresizingMaskIntoConstraints = false
			};
			if (SnackBar.Appearance.BackgroundColor != SnackBarAppearance.DefaultColor)
			{
				messageLabel.BackgroundColor = SnackBar.Appearance.BackgroundColor;
			}

			if (SnackBar.Appearance.TextForeground != SnackBarAppearance.DefaultColor)
			{
				messageLabel.TextColor = SnackBar.Appearance.TextForeground;
			}

			if (SnackBar.Appearance.TextFont != SnackBarAppearance.DefaultFont)
			{
				messageLabel.Font = SnackBar.Appearance.TextFont;
			}

			StackView.AddArrangedSubview(messageLabel);
		}
	}
}