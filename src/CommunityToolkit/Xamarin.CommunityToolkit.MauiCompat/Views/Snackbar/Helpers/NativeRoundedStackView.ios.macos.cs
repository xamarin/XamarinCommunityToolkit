using System;using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
#if __IOS__
using Xamarin.CommunityToolkit.UI.Views.Helpers.iOS;
using UIKit;
#elif __MACOS__
using Xamarin.CommunityToolkit.UI.Views.Helpers.macOS;
using Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.Extensions;
using AppKit;
#endif

namespace Xamarin.CommunityToolkit.Views.Snackbar.Helpers
{
#if __IOS__
	class NativeRoundedStackView : UIStackView
#else
	class NativeRoundedStackView : NSStackView
#endif
	{
		public System.Runtime.InteropServices.NFloat Left { get; }

		public System.Runtime.InteropServices.NFloat Top { get; }

		public System.Runtime.InteropServices.NFloat Right { get; }

		public System.Runtime.InteropServices.NFloat Bottom { get; }

		public NativeRoundedStackView(System.Runtime.InteropServices.NFloat left, System.Runtime.InteropServices.NFloat top, System.Runtime.InteropServices.NFloat right, System.Runtime.InteropServices.NFloat bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

#if __IOS__
		public override void Draw(CGRect rect)
		{
			ClipsToBounds = true;
#else
		public override void DrawRect(CGRect rect)
		{
			WantsLayer = true;
#endif
			var path = GetRoundedPath(rect, Left, Top, Right, Bottom);
			var maskLayer = new CAShapeLayer { Frame = rect, Path = path };
			Layer!.Mask = maskLayer;
			Layer.MasksToBounds = true;
		}

		CGPath? GetRoundedPath(CGRect rect, System.Runtime.InteropServices.NFloat left, System.Runtime.InteropServices.NFloat top, System.Runtime.InteropServices.NFloat right, System.Runtime.InteropServices.NFloat bottom)
		{
#if __IOS__
			var path = new UIBezierPath();
#else
			var path = new NSBezierPath();
#endif
			path.MoveTo(new CGPoint(rect.Width - right, rect.Y));

			path.AddArc(new CGPoint((float)rect.X + rect.Width - right, (float)rect.Y + right), (System.Runtime.InteropServices.NFloat)right, (float)(Math.PI * 1.5), (float)Math.PI * 2, true);
			path.AddLineTo(new CGPoint(rect.Width, rect.Height - bottom));

			path.AddArc(new CGPoint((float)rect.X + rect.Width - bottom, (float)rect.Y + rect.Height - bottom), (System.Runtime.InteropServices.NFloat)bottom, 0, (float)(Math.PI * .5), true);
			path.AddLineTo(new CGPoint(left, rect.Height));

			path.AddArc(new CGPoint((float)rect.X + left, (float)rect.Y + rect.Height - left), (System.Runtime.InteropServices.NFloat)left, (float)(Math.PI * .5), (float)Math.PI, true);
			path.AddLineTo(new CGPoint(rect.X, top));

			path.AddArc(new CGPoint((float)rect.X + top, (float)rect.Y + top), (System.Runtime.InteropServices.NFloat)top, (float)Math.PI, (float)(Math.PI * 1.5), true);

			path.ClosePath();

#if __IOS__
			return path.CGPath;
#else
			return path.ToCGPath();
#endif
		}
	}
}