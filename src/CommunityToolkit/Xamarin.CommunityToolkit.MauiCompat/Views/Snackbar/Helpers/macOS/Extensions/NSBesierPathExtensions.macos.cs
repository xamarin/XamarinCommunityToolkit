using System;using Microsoft.Extensions.Logging;
using AppKit;
using CoreGraphics;

namespace Xamarin.CommunityToolkit.UI.Views.Helpers.macOS.Extensions
{
	static class NSBezierPathExtensions
	{
		public static void AddLineTo(this NSBezierPath bezierPath, CGPoint point) => bezierPath.LineTo(point);

		public static void AddArc(this NSBezierPath bezierPath, CGPoint center, System.Runtime.InteropServices.NFloat radius, System.Runtime.InteropServices.NFloat startAngle, System.Runtime.InteropServices.NFloat endAngle, bool clockwise)
		{
			startAngle = (System.Runtime.InteropServices.NFloat)(startAngle * 180.0f / Math.PI);
			endAngle = (System.Runtime.InteropServices.NFloat)(endAngle * 180.0f / Math.PI);
			bezierPath.AppendPathWithArc(center, radius, startAngle, endAngle, !clockwise);
		}

		public static CGPath ToCGPath(this NSBezierPath bezierPath)
		{
			var path = new CGPath();

			for (var i = 0; i < bezierPath.ElementCount; i++)
			{
				var type = bezierPath.ElementAt(i, out var points);

				switch (type)
				{
					case NSBezierPathElement.MoveTo:
						path.MoveToPoint(points[0]);
						break;
					case NSBezierPathElement.LineTo:
						path.AddLineToPoint(points[0]);
						break;
					case NSBezierPathElement.CurveTo:
						path.AddCurveToPoint(points[0], points[1], points[2]);
						break;
					case NSBezierPathElement.ClosePath:
						path.CloseSubpath();
						break;
				}
			}

			return path;
		}
	}
}