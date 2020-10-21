using System;
using AppKit;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackbarViews
{
	class ActionMessageSnackbarView : MessageSnackbarView
	{
		readonly MacOSSnackBar snackbar;

		public ActionMessageSnackbarView(MacOSSnackBar snackbar)
			: base(snackbar) => this.snackbar = snackbar;

		public NSButton ActionButton { get; set; }

		// Gets the maximum width of the action button. Possible values 0 to 1.
		protected virtual nfloat ActionButtonMaxWidth => 1f;

		public override void RemoveFromSuperview()
		{
			base.RemoveFromSuperview();

			if (ActionButton != null)
			{
				ActionButton.Activated -= DismissButtonTouchUpInside;
			}
		}

		protected override void ConstrainChildren()
		{
			MessageLabel.TrailingAnchor
				.ConstraintLessThanOrEqualToAnchor(ActionButton.LeadingAnchor, -Snackbar.Layout.Spacing).Active = true;
			MessageLabel.LeadingAnchor.ConstraintEqualToAnchor(LeadingAnchor, Snackbar.Layout.PaddingLeading)
				.Active = true;
			MessageLabel.BottomAnchor.ConstraintEqualToAnchor(BottomAnchor, -Snackbar.Layout.PaddingBottom)
				.Active = true;
			MessageLabel.TopAnchor.ConstraintEqualToAnchor(TopAnchor, Snackbar.Layout.PaddingTop).Active = true;

			ActionButton.TrailingAnchor.ConstraintEqualToAnchor(TrailingAnchor, -Snackbar.Layout.PaddingTrailing)
				.Active = true;
			ActionButton.CenterYAnchor.ConstraintEqualToAnchor(CenterYAnchor).Active = true;

			// The following constraint makes sure that button is not wider than specified amount of available width
			ActionButton.WidthAnchor.ConstraintLessThanOrEqualToAnchor(WidthAnchor, ActionButtonMaxWidth, 0f).Active =
				true;

			ActionButton.SetContentCompressionResistancePriority(
				MessageLabel.GetContentCompressionResistancePriority(NSLayoutConstraintOrientation.Horizontal) + 1,
				NSLayoutConstraintOrientation.Horizontal);
		}

		protected override void Initialize()
		{
			base.Initialize();

			ActionButton = new NSButton
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Title = Snackbar.ActionButtonText,
				LineBreakMode = Snackbar.Appearance.DismissButtonLineBreakMode
			};
			ActionButton.Activated += DismissButtonTouchUpInside;
			AddSubview(ActionButton);
		}

		async void DismissButtonTouchUpInside(object sender, EventArgs e)
		{
			await snackbar.Action();
			Dismiss();
		}
	}
}