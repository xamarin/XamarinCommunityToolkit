using AppKit;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackBarViews
{
	abstract class BaseSnackBarView : NSView
	{
		protected BaseSnackBarView(NativeSnackBar snackBar) => SnackBar = snackBar;

		public virtual NSView ParentView => NSApplication.SharedApplication.KeyWindow.ContentView;

		protected NativeSnackBar SnackBar { get; }

		public NSStackView? StackView { get; set; }

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
			LeadingAnchor.ConstraintGreaterThanOrEqualToAnchor(ParentView.LeadingAnchor, SnackBar.Layout.MarginLeft).Active = true;
			TrailingAnchor.ConstraintGreaterThanOrEqualToAnchor(ParentView.TrailingAnchor, -SnackBar.Layout.MarginRight).Active = true;
			CenterXAnchor.ConstraintEqualToAnchor(ParentView.CenterXAnchor).Active = true;

			if (StackView != null)
			{
				StackView.LeadingAnchor.ConstraintEqualToAnchor(LeadingAnchor, SnackBar.Layout.PaddingLeft).Active = true;
				StackView.TrailingAnchor.ConstraintEqualToAnchor(TrailingAnchor, -SnackBar.Layout.PaddingRight).Active = true;
				StackView.BottomAnchor.ConstraintEqualToAnchor(BottomAnchor, -SnackBar.Layout.PaddingBottom).Active = true;
				StackView.TopAnchor.ConstraintEqualToAnchor(TopAnchor, SnackBar.Layout.PaddingTop).Active = true;
			}
		}

		protected virtual void Initialize()
		{
			StackView = new NSStackView
			{
				WantsLayer = true
			};

			AddSubview(StackView);

			if (SnackBar.Appearance.Background != NativeSnackBarAppearance.DefaultColor && StackView.Layer != null)
			{
				StackView.Layer.BackgroundColor = SnackBar.Appearance.Background.CGColor;
			}

			StackView.Orientation = NSUserInterfaceLayoutOrientation.Horizontal;
			StackView.TranslatesAutoresizingMaskIntoConstraints = false;
			StackView.Spacing = SnackBar.Layout.Spacing;
			TranslatesAutoresizingMaskIntoConstraints = false;
		}
	}
}