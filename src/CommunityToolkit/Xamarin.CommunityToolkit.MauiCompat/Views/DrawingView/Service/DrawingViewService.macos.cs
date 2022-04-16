using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AppKit;
using CoreGraphics;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.MacOS;

namespace Xamarin.CommunityToolkit.UI.Views
{
	static class DrawingViewService
	{
		public static Stream GetImageStream(IList<Line>? lines,
			Size imageSize,
			Color backgroundColor)
		{
			if (lines == null)
			{
				return Stream.Null;
			}

			var image = GetImageInternal(lines, backgroundColor);
			if (image is null)
			{
				return Stream.Null;
			}

			return image.AsTiff()?.AsStream() ?? Stream.Null;
		}

		/// <summary>
		/// Get image stream from points
		/// </summary>
		/// <param name="points">Drawing points</param>
		/// <param name="imageSize">Image size</param>
		/// <param name="lineWidth">Line Width</param>
		/// <param name="strokeColor">Line color</param>
		/// <param name="backgroundColor">Image background color</param>
		/// <returns>Image stream</returns>
		public static Stream GetImageStream(IList<Point>? points,
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
			if (image is null)
			{
				return Stream.Null;
			}

			return image.AsTiff()?.AsStream() ?? Stream.Null;
		}

		static NSImage? GetImageInternal(IList<Point> points,
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

			using var context = new CGBitmapContext(IntPtr.Zero, (nint)drawingWidth, (nint)drawingHeight, 8,
				(nint)drawingWidth * 4,
				NSColorSpace.GenericRGBColorSpace.ColorSpace,
				CGImageAlphaInfo.PremultipliedFirst);
			context.SetFillColor(backgroundColor.ToCGColor());
			context.FillRect(new CGRect(CGPoint.Empty, imageSize));

			context.SetStrokeColor(strokeColor.ToCGColor());
			context.SetLineWidth(lineWidth);
			context.SetLineCap(CGLineCap.Round);
			context.SetLineJoin(CGLineJoin.Round);

			context.AddLines(points.Select(p => new CGPoint(p.X - minPointX, p.Y - minPointY)).ToArray());
			context.StrokePath();

			using var cgImage = context.ToImage() ?? throw new InvalidOperationException("Image Cannot be null");
			NSImage image = new(cgImage, imageSize);

			return image;
		}

		static NSImage? GetImageInternal(IList<Line> lines,
			Color backgroundColor)
		{
			var points = lines.SelectMany(x => x.Points).ToList();
			var minPointX = points.Min(p => p.X);
			var minPointY = points.Min(p => p.Y);
			var drawingWidth = points.Max(p => p.X) - minPointX;
			var drawingHeight = points.Max(p => p.Y) - minPointY;
			const int minSize = 1;
			if (drawingWidth < minSize || drawingHeight < minSize)
				return null;

			var imageSize = new CGSize(drawingWidth, drawingHeight);

			using var context = new CGBitmapContext(IntPtr.Zero, (nint)drawingWidth, (nint)drawingHeight, 8,
				(nint)drawingWidth * 4,
				NSColorSpace.GenericRGBColorSpace.ColorSpace,
				CGImageAlphaInfo.PremultipliedFirst);
			context.SetFillColor(backgroundColor.ToCGColor());
			context.FillRect(new CGRect(CGPoint.Empty, imageSize));

			foreach (var line in lines)
			{
				context.SetStrokeColor(line.LineColor.ToCGColor());
				context.SetLineWidth(line.LineWidth);
				context.SetLineCap(CGLineCap.Round);
				context.SetLineJoin(CGLineJoin.Round);

				var startPoint = line.Points.First();
				context.MoveTo((float)startPoint.X, (float)startPoint.Y);
				context.AddLines(line.Points.Select(p => new CGPoint(p.X - minPointX, p.Y - minPointY)).ToArray());
			}

			context.StrokePath();

			using var cgImage = context.ToImage() ?? throw new InvalidOperationException("Image Cannot Be Null");
			NSImage image = new(cgImage, imageSize);

			return image;
		}
	}
}