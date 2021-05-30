namespace CommunityToolkit.Maui.UI.Views.Helpers.macOS.SnackBarViews
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
				StackView?.AddArrangedSubview(actionButton);
			}
		}
	}
}