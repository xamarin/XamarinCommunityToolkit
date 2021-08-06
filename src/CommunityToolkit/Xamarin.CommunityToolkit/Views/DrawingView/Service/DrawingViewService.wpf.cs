﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Xamarin.Forms.Platform.WPF;
using Color = Xamarin.Forms.Color;
using Point = Xamarin.Forms.Point;
using Size = Xamarin.Forms.Size;

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

			var resizedImage = MaxResizeImage(image, (float)imageSize.Width, (float)imageSize.Height);
			using (resizedImage)
			{
				var stream = new MemoryStream();
				resizedImage.Save(stream, ImageFormat.Jpeg);
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
			if (image is null)
			{
				return Stream.Null;
			}

			var resizedImage = MaxResizeImage(image, (float)imageSize.Width, (float)imageSize.Height);
			using (resizedImage)
			{
				var stream = new MemoryStream();
				resizedImage.Save(stream, ImageFormat.Jpeg);
				stream.Position = 0;
				return stream;
			}
		}

		static Bitmap? GetImageInternal(ICollection<Point> points,
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

			var bm = new Bitmap((int)drawingWidth, (int)drawingHeight);
			using var gr = Graphics.FromImage(bm);
			var drawingBackgroundColor = XamarinColorToDrawingColor(backgroundColor);
			gr.Clear(drawingBackgroundColor);
			using var pen = new Pen(XamarinColorToDrawingColor(strokeColor), lineWidth);
			var path = new GraphicsPath();
			var pointsCount = points.Count;
			for (var i = 0; i < pointsCount - 1; i++)
			{
				var p1 = XamarinPointToDrawingPoint(points.ElementAt(i));
				var p2 = XamarinPointToDrawingPoint(points.ElementAt(i + 1));
				path.AddLine(p1.X - (float)minPointX, p1.Y - (float)minPointY, p2.X - (float)minPointX,
					p2.Y - (float)minPointY);
			}

			gr.DrawPath(pen, path);

			return bm;
		}

		static System.Drawing.Color XamarinColorToDrawingColor(Color xamarinColor)
		{
			var mediaColor = xamarinColor.ToMediaColor();
			return System.Drawing.Color.FromArgb(mediaColor.A, mediaColor.R, mediaColor.G, mediaColor.B);
		}

		static PointF XamarinPointToDrawingPoint(Point xamarinPoint) =>
			new PointF((float)xamarinPoint.X, (float)xamarinPoint.Y);

		static Image MaxResizeImage(Image sourceImage, float maxWidth, float maxHeight)
		{
			var sourceSize = sourceImage.Size;
			var maxResizeFactor = Math.Max(maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);
			if (maxResizeFactor > 1)
			{
				return sourceImage;
			}

			var width = maxResizeFactor * sourceSize.Width;
			var height = maxResizeFactor * sourceSize.Height;
			var bm = new Bitmap((int)width, (int)height);
			using var gr = Graphics.FromImage(bm);
			gr.DrawImage(sourceImage, new Rectangle(0, 0, bm.Width, bm.Height));

			return bm;
		}

		static Bitmap? GetImageInternal(IList<Line> lines,
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

			var bm = new Bitmap((int)drawingWidth, (int)drawingHeight);
			using (var gr = Graphics.FromImage(bm))
			{
				var drawingBackgroundColor = XamarinColorToDrawingColor(backgroundColor);
				gr.Clear(drawingBackgroundColor);

				foreach (var line in lines)
				{
					using (var pen = new Pen(XamarinColorToDrawingColor(line.LineColor), line.LineWidth))
					{
						var path = new GraphicsPath();
						var pointsCount = line.Points.Count;
						for (var i = 0; i < pointsCount - 1; i++)
						{
							var p1 = XamarinPointToDrawingPoint(line.Points.ElementAt(i));
							var p2 = XamarinPointToDrawingPoint(line.Points.ElementAt(i + 1));
							path.AddLine(p1.X - (float)minPointX, p1.Y - (float)minPointY, p2.X - (float)minPointX,
										 p2.Y - (float)minPointY);
						}

						gr.DrawPath(pen, path);
					}
				}
			}

			return bm;
		}
	}
}