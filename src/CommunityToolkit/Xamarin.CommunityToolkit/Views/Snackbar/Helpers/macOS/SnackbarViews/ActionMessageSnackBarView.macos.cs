using System;
using AppKit;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackBarViews
{
	class ActionMessageSnackBarView : MessageSnackBarView
	{
		public ActionMessageSnackBarView(NativeSnackBar snackBar)
			: base(snackBar)
		{
		}

		// Gets the maximum width of the action button. Possible values 0 to 1.
		protected virtual nfloat ActionButtonMaxWidth => 1f;

		protected override void Initialize()
		{
			base.Initialize();
			foreach (var action in SnackBar.Actions)
			{
				var actionButton = new NSButton
				{
					Title = action.ActionButtonText,
					WantsLayer = true,
					LineBreakMode = SnackBar.Appearance.DismissButtonLineBreakMode,
				};
				if (SnackBar.Appearance.ButtonBackgroundColor != SnackBarAppearance.DefaultColor)
				{
					actionButton.Layer.BackgroundColor = action.Appearance.ButtonBackgroundColor.CGColor;
				}

				if (SnackBar.Appearance.ButtonFont != SnackBarAppearance.DefaultFont)
				{
					actionButton.Font = action.Appearance.ButtonFont;
				}

				actionButton.Activated += async (s, e) =>
				{
					await action.Action();
					Dismiss();
				};
				StackView.AddArrangedSubview(actionButton);
			}
		}
	}
}