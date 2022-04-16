using System;using Microsoft.Extensions.Logging;
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
		public PaddedLabel(System.Runtime.InteropServices.NFloat left, System.Runtime.InteropServices.NFloat top, System.Runtime.InteropServices.NFloat right, System.Runtime.InteropServices.NFloat bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public System.Runtime.InteropServices.NFloat Left { get; }

		public System.Runtime.InteropServices.NFloat Top { get; }

		public System.Runtime.InteropServices.NFloat Right { get; }

		public System.Runtime.InteropServices.NFloat Bottom { get; }

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