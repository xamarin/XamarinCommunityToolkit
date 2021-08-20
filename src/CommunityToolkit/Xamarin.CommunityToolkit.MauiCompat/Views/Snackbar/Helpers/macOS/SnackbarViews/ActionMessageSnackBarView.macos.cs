using CoreGraphics;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackBarViews
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
			foreach (var actionButton in SnackBar.Actions)
			{
				StackView?.AddArrangedSubview(actionButton);
			}
		}
	}
}