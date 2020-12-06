using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.Extensions;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBar
{
	abstract class BaseSnackBarView : UIView
	{
		public BaseSnackBarView(NativeSnackBar snackBar) => SnackBar = snackBar;

		public virtual UIView ParentView => SnackBar.ParentController != null
			? SnackBar.ParentController.View
			: UIApplication.SharedApplication.KeyWindow;

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
			this.SafeBottomAnchor().ConstraintEqualTo(GetBottomAnchor(), -SnackBar.Layout.MarginBottom).Active = true;
			this.SafeTopAnchor().ConstraintGreaterThanOrEqualTo(GetTopAnchor(), SnackBar.Layout.MarginTop).Active = true;
			this.SafeLeadingAnchor().ConstraintGreaterThanOrEqualTo(ParentView.SafeLeadingAnchor(), SnackBar.Layout.MarginLeading).Active = true;
			this.SafeTrailingAnchor().ConstraintLessThanOrEqualTo(ParentView.SafeTrailingAnchor(), -SnackBar.Layout.MarginTrailing).Active = true;
			this.SafeCenterXAnchor().ConstraintEqualTo(ParentView.SafeCenterXAnchor()).Active = true;
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
			Layer.CornerRadius = SnackBar.Appearance.CornerRadius;
			TranslatesAutoresizingMaskIntoConstraints = false;
		}
	}
}