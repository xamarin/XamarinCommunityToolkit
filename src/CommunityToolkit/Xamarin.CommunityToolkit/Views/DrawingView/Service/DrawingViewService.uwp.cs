using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage.Streams;
using Windows.UI.Input.Inking;
using Microsoft.Graphics.Canvas;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

namespace Xamarin.CommunityToolkit.UI.Views
{
	static class DrawingViewService
	{
		/// <summary>
		/// Get image stream from points
		/// </summary>
		/// <param name="points">Drawing points</param>
		/// <param name="imageSize">Image size</param>
		/// <param name="lineWidth">Line Width</param>
		/// <param name="strokeColor">Line color</param>
		/// <param name="backgroundColor">Image background color</param>
		/// <returns>Image stream</returns>
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

			using (image)
			{
				var fileStream = new InMemoryRandomAccessStream();
				image.SaveAsync(fileStream, CanvasBitmapFileFormat.Jpeg).GetAwaiter().GetResult();

				var stream = fileStream.AsStream();
				stream.Position = 0;

				return stream;
			}
		}

		static CanvasRenderTarget GetImageInternal(IList<Point> points,
			float lineWidth,
			Color lineColor,
			Color backgroundColor)
		{
			var minPointX = points.Min(p => p.X);
			var minPointY = points.Min(p => p.Y);
			var drawingWidth = points.Max(p => p.X) - minPointX;
			var drawingHeight = points.Max(p => p.Y) - minPointY;
			const int minSize = 1;
			if (drawingWidth < minSize || drawingHeight < minSize)
			{
				throw new Exception("Image is too small");
			}

			var device = CanvasDevice.GetSharedDevice();
			var offscreen = new CanvasRenderTarget(device, (int)drawingWidth, (int)drawingHeight, 96);

			using var session = offscreen.CreateDrawingSession();
			session.Clear(backgroundColor.ToWindowsColor());
			var strokeBuilder = new InkStrokeBuilder();
			var inkDrawingAttributes = new InkDrawingAttributes
			{
				Color = lineColor.ToWindowsColor(),
				Size = new Windows.Foundation.Size(lineWidth, lineWidth)
			};
			strokeBuilder.SetDefaultDrawingAttributes(inkDrawingAttributes);
			var strokes = new[]
			{
				strokeBuilder.CreateStroke(
					points.Select(p => new Windows.Foundation.Point(p.X - minPointX, p.Y - minPointY)))
			};
			session.DrawInk(strokes);

			return offscreen;
		}
	}
}