using UIKit;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBar;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.iOS.SnackBarViews
{
	class MessageSnackBarView : BaseSnackBarView
	{
		public MessageSnackBarView(IOSSnackBar snackBar)
			: base(snackBar)
		{
		}

		public UILabel MessageLabel { get; set; }

		protected override void ConstrainChildren()
		{
			base.ConstrainChildren();

			MessageLabel.SafeLeadingAnchor().ConstraintEqualTo(this.SafeLeadingAnchor(), SnackBar.Layout.PaddingLeading)
				.Active = true;
			MessageLabel.SafeTrailingAnchor()
				.ConstraintEqualTo(this.SafeTrailingAnchor(), -SnackBar.Layout.PaddingTrailing).Active = true;
			MessageLabel.SafeBottomAnchor().ConstraintEqualTo(this.SafeBottomAnchor(), -SnackBar.Layout.PaddingBottom)
				.Active = true;
			MessageLabel.SafeTopAnchor().ConstraintEqualTo(this.SafeTopAnchor(), SnackBar.Layout.PaddingTop).Active =
				true;
		}

		protected override void Initialize()
		{
			base.Initialize();

			MessageLabel = new UILabel
			{
				Text = SnackBar.Message,
				Lines = 0,
				AdjustsFontSizeToFitWidth = true,
				MinimumFontSize = 10f,
				TextAlignment = SnackBar.Appearance.MessageTextAlignment,
				TranslatesAutoresizingMaskIntoConstraints = false
			};
			AddSubview(MessageLabel);
		}
	}
}