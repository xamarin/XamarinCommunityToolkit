using System;using Microsoft.Extensions.Logging;
using AppKit;
using CoreGraphics;
using Xamarin.CommunityToolkit.Views.Snackbar.Helpers;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.SnackBarViews
{
	abstract class BaseSnackBarView : NSView
	{
		protected BaseSnackBarView(NativeSnackBar snackBar) => SnackBar = snackBar;

		public NSView? AnchorView { get; set; }

		public NSView? ParentView => NSApplication.SharedApplication.KeyWindow.ContentView;

		protected NativeSnackBar SnackBar { get; }

		protected NativeRoundedStackView? StackView { get; set; }

		public void Dismiss() => RemoveFromSuperview();

		public void Setup(CGRect cornerRadius)
		{
			Initialize(cornerRadius);
			ConstraintInParent();
		}

		void ConstraintInParent()
		{
			_ = ParentView ?? throw new InvalidOperationException($"{nameof(BaseSnackBarView)}.{nameof(Initialize)} not called");
			_ = StackView ?? throw new InvalidOperationException($"{nameof(BaseSnackBarView)}.{nameof(Initialize)} not called");

			if (AnchorView is null)
			{
				BottomAnchor.ConstraintEqualToAnchor(ParentView.BottomAnchor, -SnackBar.Layout.MarginBottom).Active = true;
				TopAnchor.ConstraintGreaterThanOrEqualToAnchor(ParentView.TopAnchor, SnackBar.Layout.MarginTop).Active = true;
			}
			else
			{
				BottomAnchor.ConstraintEqualToAnchor(AnchorView.BottomAnchor, -SnackBar.Layout.MarginBottom).Active = true;
			}

			LeadingAnchor.ConstraintGreaterThanOrEqualToAnchor(ParentView.LeadingAnchor, SnackBar.Layout.MarginLeft).Active = true;
			TrailingAnchor.ConstraintGreaterThanOrEqualToAnchor(ParentView.TrailingAnchor, -SnackBar.Layout.MarginRight).Active = true;
			CenterXAnchor.ConstraintEqualToAnchor(ParentView.CenterXAnchor).Active = true;

			StackView.LeadingAnchor.ConstraintEqualToAnchor(LeadingAnchor, SnackBar.Layout.PaddingLeft).Active = true;
			StackView.TrailingAnchor.ConstraintEqualToAnchor(TrailingAnchor, -SnackBar.Layout.PaddingRight).Active = true;
			StackView.BottomAnchor.ConstraintEqualToAnchor(BottomAnchor, -SnackBar.Layout.PaddingBottom).Active = true;
			StackView.TopAnchor.ConstraintEqualToAnchor(TopAnchor, SnackBar.Layout.PaddingTop).Active = true;
		}

		protected virtual void Initialize(CGRect cornerRadius)
		{
			StackView = new NativeRoundedStackView(cornerRadius.Left, cornerRadius.Top, cornerRadius.Right, cornerRadius.Bottom)
			{
				WantsLayer = true
			};

			AddSubview(StackView);

			if (SnackBar.Appearance.Background != NativeSnackBarAppearance.DefaultColor && StackView.Layer != null)
			{
				StackView.Layer.BackgroundColor = SnackBar.Appearance.Background.CGColor;
			}

			StackView.Orientation = NSUserInterfaceLayoutOrientation.Horizontal;
			StackView.TranslatesAutoresizingMaskIntoConstraints = false;
			StackView.Spacing = SnackBar.Layout.Spacing;
			TranslatesAutoresizingMaskIntoConstraints = false;
		}
	}
}