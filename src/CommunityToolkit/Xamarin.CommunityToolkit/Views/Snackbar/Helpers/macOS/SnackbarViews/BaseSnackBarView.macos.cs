using AppKit;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackBarViews
{
	abstract class BaseSnackBarView : NSView
	{
		protected BaseSnackBarView(NativeSnackBar snackBar) => SnackBar = snackBar;

		public virtual NSView ParentView => NSApplication.SharedApplication.KeyWindow.ContentView;

		protected NativeSnackBar SnackBar { get; }

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
		}

		protected virtual void Initialize()
		{
			WantsLayer = true;
			if (Layer != null)
			{
				Layer.CornerRadius = SnackBar.Appearance.CornerRadius;
			}

			TranslatesAutoresizingMaskIntoConstraints = false;
		}
	}
}