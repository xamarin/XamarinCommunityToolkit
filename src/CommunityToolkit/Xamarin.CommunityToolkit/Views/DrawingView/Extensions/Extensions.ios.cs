using CoreGraphics;
using UIKit;

namespace Xamarin.CommunityToolkit.UI.Views.iOS
{
	public static class Extensions
	{
		public static void MoveTo(this UIBezierPath path, double x, double y) => path.MoveTo(new CGPoint(x, y));

		public static void LineTo(this UIBezierPath path, double x, double y) => path.AddLineTo(new CGPoint(x, y));
	}
}