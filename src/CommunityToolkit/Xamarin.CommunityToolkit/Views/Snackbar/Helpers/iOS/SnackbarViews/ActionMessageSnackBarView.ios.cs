using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBarViews;
using Xamarin.CommunityToolkit.Views.Snackbar.Helpers;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS
{
	class ActionMessageSnackBarView : MessageSnackBarView
	{
		public ActionMessageSnackBarView(NativeSnackBar snackBar)
			: base(snackBar)
		{
		}

		protected override void Initialize()
		{
			base.Initialize();

			foreach (var actionButton in SnackBar.Actions)
			{
				StackView.AddArrangedSubview(actionButton);
			}
		}
	}
}