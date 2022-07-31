using System;
using CoreGraphics;
using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBarViews;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS
{
	class ActionMessageSnackBarView : MessageSnackBarView
	{
		public ActionMessageSnackBarView(NativeSnackBar snackBar)
			: base(snackBar)
		{
		}

		protected override void Initialize(CGRect cornerRadius)
		{
			base.Initialize(cornerRadius);

			_ = StackView ?? throw new NullReferenceException();
			const float actionButtonPriority = 1000;
			foreach (var actionButton in SnackBar.Actions)
			{
				actionButton.SetContentHuggingPriority(actionButtonPriority, UILayoutConstraintAxis.Horizontal);
				actionButton.SetContentCompressionResistancePriority(actionButtonPriority, UILayoutConstraintAxis.Horizontal);
				StackView.AddArrangedSubview(actionButton);
			}
		}
	}
}