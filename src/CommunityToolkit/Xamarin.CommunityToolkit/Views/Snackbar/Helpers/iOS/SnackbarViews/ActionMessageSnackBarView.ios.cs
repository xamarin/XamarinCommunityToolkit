using System;
using CoreGraphics;
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

			foreach (var actionButton in SnackBar.Actions)
			{
				StackView.AddArrangedSubview(actionButton);
			}
		}
	}
}