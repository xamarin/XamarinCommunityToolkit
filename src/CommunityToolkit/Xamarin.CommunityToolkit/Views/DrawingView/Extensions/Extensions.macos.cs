using AppKit;
using CoreGraphics;

namespace Xamarin.CommunityToolkit.UI.Views.macOS
{
	public static class Extensions
	{
		public static void MoveTo(this NSBezierPath path, double x, double y) => path.MoveTo(new CGPoint(x, y));

		public static void LineTo(this NSBezierPath path, double x, double y) => path.LineTo(new CGPoint(x, y));
	}
}