using AppKit;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackBarViews
{
	class MessageSnackBarView : BaseSnackBarView
	{
		public MessageSnackBarView(MacOSSnackBar snackBar)
			: base(snackBar)
		{
		}

		public NSTextField MessageLabel { get; set; }

		protected override void ConstrainChildren()
		{
			base.ConstrainChildren();

			MessageLabel.LeadingAnchor.ConstraintEqualToAnchor(LeadingAnchor, SnackBar.Layout.PaddingLeading).Active =
				true;
			MessageLabel.TrailingAnchor.ConstraintEqualToAnchor(TrailingAnchor, -SnackBar.Layout.PaddingTrailing)
				.Active = true;
			MessageLabel.BottomAnchor.ConstraintEqualToAnchor(BottomAnchor, -SnackBar.Layout.PaddingBottom).Active =
				true;
			MessageLabel.TopAnchor.ConstraintEqualToAnchor(TopAnchor, SnackBar.Layout.PaddingTop).Active = true;
		}

		protected override void Initialize()
		{
			base.Initialize();

			MessageLabel = new NSTextField
			{
				StringValue = SnackBar.Message,
				Selectable = false,
				Font = SnackBar.Appearance.TextFont,
				BackgroundColor = SnackBar.Appearance.BackgroundColor,
				TextColor = SnackBar.Appearance.TextForeground,
				Alignment = SnackBar.Appearance.MessageTextAlignment,
				LineBreakMode = SnackBar.Appearance.DismissButtonLineBreakMode,
				TranslatesAutoresizingMaskIntoConstraints = false
			};
			AddSubview(MessageLabel);
		}
	}
}