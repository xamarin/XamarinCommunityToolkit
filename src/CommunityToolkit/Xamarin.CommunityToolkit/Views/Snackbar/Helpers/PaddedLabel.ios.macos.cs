using System;
using CoreGraphics;
#if __IOS__
using UIKit;
#else
using AppKit;
#endif

namespace Xamarin.CommunityToolkit.UI.Views.Helpers
{
#if __IOS__
	class PaddedLabel : UILabel
#else
	class PaddedLabel : NSTextField
#endif
	{
		public PaddedLabel(nfloat left, nfloat top, nfloat right, nfloat bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public nfloat Left { get; }

		public nfloat Top { get; }

		public nfloat Right { get; }

		public nfloat Bottom { get; }

		public override CGSize IntrinsicContentSize => new CGSize(
			base.IntrinsicContentSize.Width + Left + Right,
			base.IntrinsicContentSize.Height + Top + Bottom);

#if __IOS__
		public override void DrawText(CGRect rect)
		{
			var insets = new UIEdgeInsets(Top, Left, Bottom, Right);
			base.DrawText(insets.InsetRect(rect));
		}
#endif
	}
}