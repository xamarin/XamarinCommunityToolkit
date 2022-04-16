using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Tizen;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Tizen;

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
			using (resizedImage)
			{
				var stream = resizedImage.Encode(SKEncodedImageFormat.Jpeg, 100).AsStream();
				stream.Position = 0;
				return stream;
			}
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
			using (resizedImage)
			{
				var stream = resizedImage.Encode(SKEncodedImageFormat.Jpeg, 100).AsStream();
				stream.Position = 0;
				return stream;
			}
		}

		static SKImage? GetImageInternal(IList<Point> points,
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

			var bm = new SKBitmap((int)drawingWidth, (int)drawingHeight);
			using (var gr = new SKCanvas(bm))
			{
				var drawingBackgroundColor = XamarinColorToDrawingColor(backgroundColor);
				gr.Clear(drawingBackgroundColor);
				using (var pen = new SKPaint
				{
					Color = XamarinColorToDrawingColor(strokeColor),
					StrokeWidth = lineWidth
				})
				{
					var pointsCount = points.Count;
					for (var i = 0; i < pointsCount - 1; i++)
					{
						var p1 = XamarinPointToDrawingPoint(points.ElementAt(i));
						var p2 = XamarinPointToDrawingPoint(points.ElementAt(i + 1));

						gr.DrawLine(p1.X - (float)minPointX, p1.Y - (float)minPointY, p2.X - (float)minPointX,
									p2.Y - (float)minPointY, pen);
					}
				}
			}

			return SKImage.FromBitmap(bm);
		}

		static SKColor XamarinColorToDrawingColor(Color xamarinColor) => xamarinColor.ToNative().ToSKColor();

		static SKPoint XamarinPointToDrawingPoint(Point xamarinPoint) => new SKPoint((float)xamarinPoint.X, (float)xamarinPoint.Y);

		static SKBitmap MaxResizeImage(SKImage sourceImage, float maxWidth, float maxHeight)
		{
			var sourceSize = new Size(sourceImage.Width, sourceImage.Height);
			var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
			if (maxResizeFactor > 1)
			{
				return SKBitmap.FromImage(sourceImage);
			}

			var width = maxResizeFactor * sourceSize.Width;
			var height = maxResizeFactor * sourceSize.Height;
			var bm = new SKBitmap((int)width, (int)height);
			using (var gr = new SKCanvas(bm))
			{
				gr.DrawImage(sourceImage, new SKRect(0, 0, bm.Width, bm.Height));
			}

			return bm;
		}

		static SKImage? GetImageInternal(IList<Line> lines,
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

			var bm = new SKBitmap((int)drawingWidth, (int)drawingHeight);
			using (var gr = new SKCanvas(bm))
			{
				var drawingBackgroundColor = XamarinColorToDrawingColor(backgroundColor);
				gr.Clear(drawingBackgroundColor);

				foreach (var line in lines)
				{
					using (var pen = new SKPaint
					{
						Color = XamarinColorToDrawingColor(line.LineColor),
						StrokeWidth = line.LineWidth
					})
					{
						var pointsCount = line.Points.Count;
						for (var i = 0; i < pointsCount - 1; i++)
						{
							var p1 = XamarinPointToDrawingPoint(line.Points.ElementAt(i));
							var p2 = XamarinPointToDrawingPoint(line.Points.ElementAt(i + 1));

							gr.DrawLine(p1.X - (float)minPointX, p1.Y - (float)minPointY, p2.X - (float)minPointX,
										p2.Y - (float)minPointY, pen);
						}
					}
				}
			}

			return SKImage.FromBitmap(bm);
		}
	}
}