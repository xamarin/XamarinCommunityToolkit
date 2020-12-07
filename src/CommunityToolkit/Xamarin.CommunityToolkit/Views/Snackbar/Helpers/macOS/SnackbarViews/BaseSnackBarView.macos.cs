using AppKit;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackBarViews
{
	abstract class BaseSnackBarView : NSView
	{
		protected BaseSnackBarView(NativeSnackBar snackBar) => SnackBar = snackBar;

		public virtual NSView ParentView => NSApplication.SharedApplication.KeyWindow.ContentView;

		protected NativeSnackBar SnackBar { get; }

		public NSStackView StackView { get; set; }

		public virtual void Dismiss() => RemoveFromSuperview();

		public virtual void Setup()
		{
			Initialize();
			ConstrainInParent();
			ConstrainChildren();
		}

		protected virtual void ConstrainChildren()
		{
		}

		protected virtual void ConstrainInParent()
		{
			BottomAnchor.ConstraintEqualToAnchor(ParentView.BottomAnchor, -SnackBar.Layout.MarginBottom).Active = true;
			TopAnchor.ConstraintGreaterThanOrEqualToAnchor(ParentView.TopAnchor, SnackBar.Layout.MarginTop).Active = true;
			LeadingAnchor.ConstraintGreaterThanOrEqualToAnchor(ParentView.LeadingAnchor, SnackBar.Layout.MarginLeading).Active = true;
			TrailingAnchor.ConstraintGreaterThanOrEqualToAnchor(ParentView.TrailingAnchor, -SnackBar.Layout.MarginTrailing).Active = true;
			CenterXAnchor.ConstraintEqualToAnchor(ParentView.CenterXAnchor).Active = true;

			StackView.LeadingAnchor.ConstraintEqualToAnchor(LeadingAnchor, SnackBar.Layout.PaddingLeading).Active = true;
			StackView.TrailingAnchor.ConstraintEqualToAnchor(TrailingAnchor, -SnackBar.Layout.PaddingTrailing).Active = true;
			StackView.BottomAnchor.ConstraintEqualToAnchor(BottomAnchor, -SnackBar.Layout.PaddingBottom).Active = true;
			StackView.TopAnchor.ConstraintEqualToAnchor(TopAnchor, SnackBar.Layout.PaddingTop).Active = true;
		}

		protected virtual void Initialize()
		{
			StackView = new NSStackView();
			StackView.WantsLayer = true;
			AddSubview(StackView);
			if (SnackBar.Appearance.Background != NativeSnackBarAppearance.DefaultColor)
			{
				StackView.Layer.BackgroundColor = SnackBar.Appearance.Background.CGColor;
			}

			StackView.Orientation = NSUserInterfaceLayoutOrientation.Horizontal;
			StackView.TranslatesAutoresizingMaskIntoConstraints = false;
			StackView.Spacing = 5;
			TranslatesAutoresizingMaskIntoConstraints = false;
		}
	}
}