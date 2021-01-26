using System;
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

		// Gets the maximum width of the action button. Possible values 0 to 1.
		protected virtual nfloat ActionButtonMaxWidth => 1f;

		protected override void Initialize()
		{
			base.Initialize();

			foreach (var action in SnackBar.Actions)
			{
				var actionButton = new UIButton(UIButtonType.System);
				if (action.Appearance.Background != NativeSnackButtonAppearance.DefaultColor)
				{
					actionButton.BackgroundColor = action.Appearance.Background;
				}

				if (action.Appearance.Foreground != NativeSnackButtonAppearance.DefaultColor)
				{
					actionButton.SetTitleColor(action.Appearance.Foreground, UIControlState.Normal);
				}

				if (action.Appearance.Font != NativeSnackButtonAppearance.DefaultFont)
				{
					actionButton.Font = action.Appearance.Font;
				}

				actionButton.SetTitle(action.ActionButtonText, UIControlState.Normal);
				actionButton.TitleLabel.LineBreakMode = action.Appearance.LineBreakMode;
				actionButton.TouchUpInside += async (s, e) =>
				{
					await action.Action();
					Dismiss();
				};

				StackView.AddArrangedSubview(actionButton);
			}
		}
	}
}