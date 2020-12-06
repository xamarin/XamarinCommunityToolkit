using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBar;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBarViews
{
	class MessageSnackBarView : BaseSnackBarView
	{
		public MessageSnackBarView(NativeSnackBar snackBar)
			: base(snackBar)
		{
		}

		protected UIStackView StackView { get; set; }

		protected override void ConstrainChildren()
		{
			base.ConstrainChildren();

			StackView.SafeLeadingAnchor().ConstraintEqualTo(this.SafeLeadingAnchor(), SnackBar.Layout.PaddingLeading).Active = true;
			StackView.SafeTrailingAnchor().ConstraintEqualTo(this.SafeTrailingAnchor(), -SnackBar.Layout.PaddingTrailing).Active = true;
			StackView.SafeBottomAnchor().ConstraintEqualTo(this.SafeBottomAnchor(), -SnackBar.Layout.PaddingBottom).Active = true;
			StackView.SafeTopAnchor().ConstraintEqualTo(this.SafeTopAnchor(), SnackBar.Layout.PaddingTop).Active = true;
		}

		protected override void Initialize()
		{
			base.Initialize();

			StackView = new UIStackView();
			AddSubview(StackView);
			if (SnackBar.Appearance.BackgroundColor != SnackBarAppearance.DefaultColor)
			{
				StackView.BackgroundColor = SnackBar.Appearance.BackgroundColor;
			}

			StackView.Axis = UILayoutConstraintAxis.Horizontal;
			StackView.TranslatesAutoresizingMaskIntoConstraints = false;
			StackView.Spacing = 5;

			var messageLabel = new UILabel
			{
				Text = SnackBar.Message,
				Lines = 0,
				AdjustsFontSizeToFitWidth = true,
				TextAlignment = SnackBar.Appearance.MessageTextAlignment
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