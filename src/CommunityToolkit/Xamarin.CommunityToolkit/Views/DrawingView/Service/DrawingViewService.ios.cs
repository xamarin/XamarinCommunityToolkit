using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

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
			if (image == null)
			{
				return Stream.Null;
			}

			var resizedImage = MaxResizeImage(image, (float)imageSize.Width, (float)imageSize.Height);
			return resizedImage.AsJPEG().AsStream();
		}

		static UIImage GetImageInternal(IList<Point> points,
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
				return null;
			}

			context.SetFillColor(backgroundColor.ToCGColor());
			context.FillRect(new CGRect(CGPoint.Empty, imageSize));

			context.SetStrokeColor(strokeColor.ToCGColor());
			context.SetLineWidth(lineWidth);
			context.SetLineCap(CGLineCap.Round);
			context.SetLineJoin(CGLineJoin.Round);

			context.AddLines(points.Select(p => new CGPoint(p.X - minPointX, p.Y - minPointY)).ToArray());
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