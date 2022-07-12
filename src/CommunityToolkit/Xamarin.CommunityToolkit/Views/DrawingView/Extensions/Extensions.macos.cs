using AppKit;
using CoreGraphics;

namespace Xamarin.CommunityToolkit.UI.Views.macOS
{
	/// <summary>
	/// Extension methods to support <see cref="DrawingView"/>
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Move path to point
		/// </summary>
		/// <param name="path">NS Bezier Path</param>
		/// <param name="x">Target Point X</param>
		/// <param name="y">Target Point Y</param>
		public static void MoveTo(this NSBezierPath path, double x, double y) => path.MoveTo(new CGPoint(x, y));

		/// <summary>
		/// Add Line to path
		/// </summary>
		/// <param name="path">NS Bezier path</param>
		/// <param name="x">Target point X</param>
		/// <param name="y">Target point Y</param>
		public static void LineTo(this NSBezierPath path, double x, double y) => path.LineTo(new CGPoint(x, y));
	}
}