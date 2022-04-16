using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreGraphics;
using UIKit;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.iOS;

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
			if (image == null)
			{
				return Stream.Null;
			}

			var resizedImage = MaxResizeImage(image, (float)imageSize.Width, (float)imageSize.Height);
			return resizedImage.AsJPEG().AsStream();
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
			if (image == null)
			{
				return Stream.Null;
			}

			var resizedImage = MaxResizeImage(image, (float)imageSize.Width, (float)imageSize.Height);
			return resizedImage.AsJPEG().AsStream();
		}

		static UIImage? GetImageInternal(IList<Point> points,
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
			UIGraphics.BeginImageContextWithOptions(imageSize, false, 1);

			var context = UIGraphics.GetCurrentContext();
			if (context == null)
			{
				throw new Exception("Current Context is null");
			}

			context.SetFillColor(Microsoft.Maui.Platform.ColorExtensions.ToCGColor(backgroundColor));
			context.FillRect(new CGRect(CGPoint.Empty, imageSize));

			context.SetStrokeColor(Microsoft.Maui.Platform.ColorExtensions.ToCGColor(strokeColor));
			context.SetLineWidth(lineWidth);
			context.SetLineCap(CGLineCap.Round);
			context.SetLineJoin(CGLineJoin.Round);

			context.AddLines(points.Select(p => new CGPoint(p.X - minPointX, p.Y - minPointY)).ToArray());
			context.StrokePath();

			var image = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return image;
		}

		static UIImage? GetImageInternal(IList<Line> lines,
			Color backgroundColor)
		{
			var points = lines.SelectMany(x => x.Points).ToList();
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
			UIGraphics.BeginImageContextWithOptions(imageSize, false, 1);

			var context = UIGraphics.GetCurrentContext();
			if (context == null)
			{
				throw new Exception("Current Context is null");
			}

			context.SetFillColor(Microsoft.Maui.Platform.ColorExtensions.ToCGColor(backgroundColor));
			context.FillRect(new CGRect(CGPoint.Empty, imageSize));
			foreach (var line in lines)
			{
				context.SetStrokeColor(Microsoft.Maui.Platform.ColorExtensions.ToCGColor(line.LineColor));
				context.SetLineWidth(line.LineWidth);
				context.SetLineCap(CGLineCap.Round);
				context.SetLineJoin(CGLineJoin.Round);

				var startPoint = line.Points.First();
				context.MoveTo((float)startPoint.X, (float)startPoint.Y);
				context.AddLines(line.Points.Select(p => new CGPoint(p.X - minPointX, p.Y - minPointY)).ToArray());
			}

			context.StrokePath();
			var image = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return image;
		}

		static UIImage MaxResizeImage(UIImage sourceImage, float maxWidth, float maxHeight)
		{
			var sourceSize = sourceImage.Size;
			var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
			if (maxResizeFactor > 1)
			{
				return sourceImage;
			}

			var width = maxResizeFactor * sourceSize.Width;
			var height = maxResizeFactor * sourceSize.Height;
			UIGraphics.BeginImageContext(new CGSize(width, height));
			sourceImage.Draw(new CGRect(0, 0, width, height));
			var resultImage = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return resultImage;
		}
	}
}