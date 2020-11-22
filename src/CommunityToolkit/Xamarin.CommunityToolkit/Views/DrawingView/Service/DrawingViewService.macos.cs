using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppKit;
using CoreGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

namespace Xamarin.CommunityToolkit.UI.Views
{
	static class DrawingViewService
	{
		public static Stream GetImageStream(IList<Point> points,
			Size imageSize,
			float lineWidth,
			Color strokeColor,
			Color backgroundColor)
		{
			if (points == null || points.Count < 2)
			{
				return Stream.Null;
			}

			var image = GetImageInternal(points, lineWidth, strokeColor, backgroundColor);
			return image == null ? Stream.Null : image.AsTiff().AsStream();
		}

		static NSImage GetImageInternal(IList<Point> points,
			float lineWidth,
			Color strokeColor,
			Color backgroundColor)
		{
			var minPointX = points.Min(p => p.X);
			var minPointY = points.Min(p => p.Y);
			var drawingWidth = points.Max(p => p.X) - minPointX;
			var drawingHeight = points.Max(p => p.Y) - minPointY;
			const int minSize = 1;
			if (drawingWidth < minSize || drawingHeight < minSize)
			{
				return null;
			}

			var imageSize = new CGSize(drawingWidth, drawingHeight);

			NSImage image;
			using (var context = new CGBitmapContext(IntPtr.Zero, (nint)drawingWidth, (nint)drawingHeight, 8,
													 (nint)drawingWidth * 4,
													 NSColorSpace.GenericRGBColorSpace.ColorSpace,
													 CGImageAlphaInfo.PremultipliedFirst))
			{
				context.SetFillColor(backgroundColor.ToCGColor());
				context.FillRect(new CGRect(CGPoint.Empty, imageSize));

				context.SetStrokeColor(strokeColor.ToCGColor());
				context.SetLineWidth(lineWidth);
				context.SetLineCap(CGLineCap.Round);
				context.SetLineJoin(CGLineJoin.Round);

				context.AddLines(points.Select(p => new CGPoint(p.X - minPointX, p.Y - minPointY)).ToArray());
				context.StrokePath();
				using (var cgImage = context.ToImage())
				{
					image = new NSImage(cgImage, imageSize);
				}
			}

			return image;
		}
	}
}