using System;
using System.Threading.Tasks;
#if __IOS__
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS;
using UIKit;
#elif __MACOS__
using CoreGraphics;
using Xamarin.CommunityToolkit.UI.Views.Helpers.macOS;
using AppKit;
#endif

namespace Xamarin.CommunityToolkit.Views.Snackbar.Helpers
{
#if __IOS__
	class NativeSnackButton : UIButton
#else
	class NativeSnackButton : NSButton
#endif
	{
		public NativeSnackButton(double left, double top, double right, double bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
			LineBreakMode = NativeSnackButtonAppearance.LineBreakMode;
#if __IOS__
			ContentEdgeInsets = new UIEdgeInsets((nfloat)top, (nfloat)left, (nfloat)bottom, (nfloat)right);
			TouchUpInside += async (s, e) =>
			{
				if (SnackButtonAction != null)
					await SnackButtonAction();
			};
		}
#else
			WantsLayer = true;
			Activated += async (s, e) =>
			{
				if (SnackButtonAction != null)
					await SnackButtonAction();
			};
		}

		public override CGSize IntrinsicContentSize => new CGSize(
			base.IntrinsicContentSize.Width + Left + Right,
			base.IntrinsicContentSize.Height + Top + Bottom);
#endif

		public double Left { get; }

		public double Top { get; }

		public double Right { get; }

		public double Bottom { get; }

		public Func<Task>? SnackButtonAction { get; protected set; }

		public NativeSnackButton SetAction(Func<Task> action)
		{
			SnackButtonAction = action;
			return this;
		}

		public NativeSnackButton SetActionButtonText(string title)
		{
#if __IOS__
			SetTitle(title, UIControlState.Normal);
#else
			Title = title;
#endif
			return this;
		}
	}
}