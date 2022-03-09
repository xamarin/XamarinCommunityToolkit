using System.ComponentModel;
using System.Linq;
using ElmSharp;
using SkiaSharp;
using SkiaSharp.Views.Tizen;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using Point = Xamarin.Forms.Point;

[assembly: ExportRenderer(typeof(DrawingView), typeof(DrawingViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// Tizen renderer for <see cref="DrawingViewRenderer"/>
	/// </summary>
	public class DrawingViewRenderer : ViewRenderer<DrawingView, SKCanvasView>
	{
		SKCanvasView? canvasView;
		Line? currentLine;
		bool isDrawing;
		bool disposed;

		protected override void OnElementChanged(ElementChangedEventArgs<DrawingView> e)
		{
			base.OnElementChanged(e);
			if (Control == null && Element != null)
			{
				canvasView = new SKCanvasView(Forms.Forms.NativeParent)
				{
					BackgroundColor = Element.BackgroundColor.ToNative()
				};
				canvasView.Show();
				Element.Lines.CollectionChanged += OnLinesCollectionChanged;
				SetNativeControl(canvasView);
			}

			if (e.OldElement != null && canvasView is not null)
			{
				canvasView.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseDown, MouseDown);
				canvasView.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseUp, MouseUp);
				canvasView.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseMove, MouseMove);
				canvasView.PaintSurface -= OnPaintSurface;
			}

			if (e.NewElement != null && canvasView is not null)
			{
				canvasView.PaintSurface += OnPaintSurface;
				canvasView.EvasCanvas.AddEventAction(EvasObjectCallbackType.MouseDown, MouseDown);
				canvasView.EvasCanvas.AddEventAction(EvasObjectCallbackType.MouseUp, MouseUp);
				canvasView.EvasCanvas.AddEventAction(EvasObjectCallbackType.MouseMove, MouseMove);
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == DrawingView.LinesProperty.PropertyName && canvasView is not null)
			{
				canvasView.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseUp, MouseUp);
				LoadPoints();
				canvasView.EvasCanvas.AddEventAction(EvasObjectCallbackType.MouseUp, MouseUp);
			}
		}

		void OnLinesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => LoadPoints();

		void MouseMove()
		{
			if (isDrawing && canvasView is not null)
			{
				var point = canvasView.EvasCanvas.Pointer;
				currentLine?.Points.Add(new Point(point.X, point.Y));
				canvasView.Invalidate();
			}
		}

		void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e) => DrawPath(e.Surface.Canvas);

		void MouseDown()
		{
			if (Element == null)
				return;

			if (!Element.MultiLineMode)
				Clear();

			isDrawing = true;
		}

		void MouseUp()
		{
			if (Element == null)
				return;

			isDrawing = false;

			if (currentLine != null)
			{
				Element.Lines.Add(currentLine);
				Element.OnDrawingLineCompleted(currentLine);
			}

			if (Element.ClearOnFinish)
				Clear();
		}

		void LoadPoints()
		{
			if (Element == null)
				return;

			canvasView?.Invalidate();
		}

		void DrawPath(SKCanvas canvas)
		{
			canvas.Clear(SKColor.Empty);
			if (Element.Lines.Count == 0)
				return;

			foreach (var line in Element.Lines)
			{
				using var strokePaint = new SKPaint
				{
					Style = SKPaintStyle.Stroke,
					Color = line.LineColor.ToNative().ToSKColor(),
					StrokeWidth = line.LineWidth,
					IsAntialias = true
				};

				var newPointsPath = line.EnableSmoothedPath
					? line.Points.SmoothedPathWithGranularity(line.Granularity)
					: line.Points;
				var skPoints = newPointsPath.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray();
				using var path = new SKPath();
				path.MoveTo(skPoints[0]);

				foreach (var point in skPoints)
					path.LineTo(point);

				canvas.DrawPath(path, strokePaint);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;
			disposed = true;

			if (Element != null)
			{
				Element.Lines.CollectionChanged -= OnLinesCollectionChanged;
			}

			if (canvasView is not null)
			{
				canvasView.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseDown, MouseDown);
				canvasView.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseUp, MouseUp);
				canvasView.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseMove, MouseMove);
				canvasView.PaintSurface -= OnPaintSurface;
			}

			base.Dispose(disposing);
		}

		void Clear()
		{
			currentLine = new Line()
			{
				Points = new System.Collections.ObjectModel.ObservableCollection<Point>()
			};

			Element.Lines.Clear();
			canvasView?.Invalidate();
		}
	}
}