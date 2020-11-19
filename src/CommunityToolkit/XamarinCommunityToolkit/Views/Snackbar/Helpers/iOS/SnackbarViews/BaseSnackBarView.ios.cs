using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.Extensions;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBar
{
	abstract class BaseSnackBarView : UIView
	{
		public BaseSnackBarView(IOSSnackBar snackBar)
		{
			SnackBar = snackBar;
		}

		public NSLayoutConstraint BottomConstraint { get; protected set; }

		public NSLayoutConstraint CenterXConstraint { get; protected set; }

		public NSLayoutConstraint CenterYConstraint { get; protected set; }

		public NSLayoutConstraint LeadingConstraint { get; protected set; }

		public virtual UIView ParentView => SnackBar.ParentController != null
			? SnackBar.ParentController.View
			: UIApplication.SharedApplication.KeyWindow;

		public NSLayoutConstraint TopConstraint { get; protected set; }

		public NSLayoutConstraint TrailingConstraint { get; protected set; }

		protected IOSSnackBar SnackBar { get; }

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
			BottomConstraint = this.SafeBottomAnchor()
				.ConstraintEqualTo(GetBottomAnchor(), -SnackBar.Layout.MarginBottom);
			BottomConstraint.Active = true;

			TopConstraint = this.SafeTopAnchor()
				.ConstraintGreaterThanOrEqualTo(GetTopAnchor(), SnackBar.Layout.MarginTop);
			TopConstraint.Active = true;

			LeadingConstraint = this.SafeLeadingAnchor()
				.ConstraintGreaterThanOrEqualTo(ParentView.SafeLeadingAnchor(), SnackBar.Layout.MarginLeading);
			TrailingConstraint = this.SafeTrailingAnchor()
				.ConstraintLessThanOrEqualTo(ParentView.SafeTrailingAnchor(), -SnackBar.Layout.MarginTrailing);
			CenterXConstraint = this.SafeCenterXAnchor().ConstraintEqualTo(ParentView.SafeCenterXAnchor());

			NSLayoutConstraint.ActivateConstraints(
				new[] { LeadingConstraint, TrailingConstraint, CenterXConstraint });
		}

		protected virtual NSLayoutYAxisAnchor GetBottomAnchor()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) || SnackBar.ParentController == null)
			{
				return ParentView.SafeBottomAnchor();
			}

			return SnackBar.ParentController.BottomLayoutGuide.GetTopAnchor();
		}

		protected virtual NSLayoutYAxisAnchor GetCenterYAnchor()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) || SnackBar.ParentController == null)
			{
				return ParentView.SafeCenterYAnchor();
			}

			return SnackBar.ParentController.View?.CenterYAnchor;
		}

		protected virtual NSLayoutYAxisAnchor GetTopAnchor()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0) || SnackBar.ParentController == null)
			{
				return ParentView.SafeTopAnchor();
			}

			return SnackBar.ParentController.TopLayoutGuide.GetBottomAnchor();
		}

		protected virtual void Initialize()
		{
			BackgroundColor = SnackBar.Appearance.Color;
			Layer.CornerRadius = SnackBar.Appearance.CornerRadius;
			TranslatesAutoresizingMaskIntoConstraints = false;
		}
	}
}