using System;
using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackbarViews;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS
{
	class ActionMessageSnackbarView : MessageSnackbarView
	{
		readonly IOSSnackBar snackbar;

		public ActionMessageSnackbarView(IOSSnackBar snackbar)
			: base(snackbar) => this.snackbar = snackbar;

		public UIButton ActionButton { get; set; }

		// Gets the maximum width of the action button. Possible values 0 to 1.
		protected virtual nfloat ActionButtonMaxWidth => 1f;

		public override void RemoveFromSuperview()
		{
			base.RemoveFromSuperview();

			if (ActionButton != null)
			{
				ActionButton.TouchUpInside -= DismissButtonTouchUpInside;
			}
		}

		protected override void ConstrainChildren()
		{
			MessageLabel.SafeTrailingAnchor()
				.ConstraintEqualTo(ActionButton.SafeLeadingAnchor(), -Snackbar.Layout.Spacing).Active = true;
			MessageLabel.SafeLeadingAnchor().ConstraintEqualTo(this.SafeLeadingAnchor(), Snackbar.Layout.PaddingLeading)
				.Active = true;
			MessageLabel.SafeBottomAnchor().ConstraintEqualTo(this.SafeBottomAnchor(), -Snackbar.Layout.PaddingBottom)
				.Active = true;
			MessageLabel.SafeTopAnchor().ConstraintEqualTo(this.SafeTopAnchor(), Snackbar.Layout.PaddingTop).Active =
				true;

			ActionButton.SafeTrailingAnchor()
				.ConstraintEqualTo(this.SafeTrailingAnchor(), -Snackbar.Layout.PaddingTrailing).Active = true;
			ActionButton.SafeCenterYAnchor().ConstraintEqualTo(this.SafeCenterYAnchor()).Active = true;

			// The following constraint makes sure that button is not wider than specified amount of available width
			ActionButton.SafeWidthAnchor().ConstraintLessThanOrEqualTo(this.SafeWidthAnchor(), ActionButtonMaxWidth, 0f)
				.Active = true;

			ActionButton.SetContentCompressionResistancePriority(
				MessageLabel.ContentCompressionResistancePriority(UILayoutConstraintAxis.Horizontal) + 1,
				UILayoutConstraintAxis.Horizontal);
		}

		protected override void Initialize()
		{
			base.Initialize();

			ActionButton = new UIButton(UIButtonType.System) { TranslatesAutoresizingMaskIntoConstraints = false };
			ActionButton.SetTitle(Snackbar.ActionButtonText, UIControlState.Normal);
			ActionButton.TitleLabel.LineBreakMode = Snackbar.Appearance.DismissButtonLineBreakMode;
			ActionButton.TouchUpInside += DismissButtonTouchUpInside;
			AddSubview(ActionButton);
		}

		async void DismissButtonTouchUpInside(object sender, EventArgs e)
		{
			await snackbar.Action();
			Dismiss();
		}
	}
}