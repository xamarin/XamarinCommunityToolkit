using AppKit;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackBarViews
{
	abstract class BaseSnackBarView : NSView
	{
		public BaseSnackBarView(MacOSSnackBar snackBar) => SnackBar = snackBar;

		public NSLayoutConstraint BottomConstraint { get; protected set; }

		public NSLayoutConstraint CenterXConstraint { get; protected set; }

		public NSLayoutConstraint CenterYConstraint { get; protected set; }

		public NSLayoutConstraint LeadingConstraint { get; protected set; }

		public virtual NSView ParentView => NSApplication.SharedApplication.KeyWindow.ContentView;

		public NSLayoutConstraint TopConstraint { get; protected set; }

		public NSLayoutConstraint TrailingConstraint { get; protected set; }

		protected MacOSSnackBar SnackBar { get; }

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
			BottomConstraint =
				BottomAnchor.ConstraintEqualToAnchor(ParentView.BottomAnchor, -SnackBar.Layout.MarginBottom);
			BottomConstraint.Active = true;

			TopConstraint =
				TopAnchor.ConstraintGreaterThanOrEqualToAnchor(ParentView.TopAnchor, SnackBar.Layout.MarginTop);
			TopConstraint.Active = true;

			LeadingConstraint =
				LeadingAnchor.ConstraintGreaterThanOrEqualToAnchor(ParentView.LeadingAnchor,
					SnackBar.Layout.MarginLeading);
			TrailingConstraint =
				TrailingAnchor.ConstraintGreaterThanOrEqualToAnchor(ParentView.TrailingAnchor,
					-SnackBar.Layout.MarginTrailing);
			CenterXConstraint = CenterXAnchor.ConstraintEqualToAnchor(ParentView.CenterXAnchor);

			NSLayoutConstraint.ActivateConstraints(new[] { LeadingConstraint, TrailingConstraint, CenterXConstraint });
		}

		protected virtual void Initialize()
		{
			WantsLayer = true;
			if (Layer != null)
			{
				Layer.BackgroundColor = SnackBar.Appearance.Color.CGColor;
				Layer.CornerRadius = SnackBar.Appearance.CornerRadius;
			}

			TranslatesAutoresizingMaskIntoConstraints = false;
		}
	}
}