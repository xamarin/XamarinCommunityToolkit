using System;
using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
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
	class NativeRoundedStackView : UIStackView
#else
	class NativeRoundedStackView : NSStackView
#endif
	{
		public NativeRoundedStackView(nfloat cornerWidth, nfloat cornerHeight)
		{
			CornerWidth = cornerWidth;
			CornerHeight = cornerHeight;
#if __IOS__
			ClipsToBounds = true;
#else
			WantsLayer = true;
#endif

			SetCornerRadius(Bounds);
		}

		public nfloat CornerWidth{ get; }

		public nfloat CornerHeight { get; }

		void SetCornerRadius(CGRect rect)
		{
#if __IOS__
			var path = UIBezierPath.FromRoundedRect(rect, UIRectCorner.AllCorners, new CGSize(CornerWidth, CornerHeight)).CGPath;
#else
			var path = CGPath.FromRoundedRect(rect, CornerWidth, CornerHeight);
#endif

			var maskLayer = new CAShapeLayer();

			maskLayer.Frame = rect;

			maskLayer.Path = path;

			Layer!.Mask = maskLayer;

			Layer.MasksToBounds = true;

	}
	}
}