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
	public class DrawingViewRenderer : ViewRenderer<DrawingView, SKCanvasView>
	{
		SKCanvasView? canvasView;
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
				Element.Points.CollectionChanged += OnPointsCollectionChanged;
				SetNativeControl(canvasView);
			}

			if (e.OldElement != null)
			{
				canvasView!.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseDown, MouseDown);
				canvasView.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseUp, MouseUp);
				canvasView.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseMove, MouseMove);
				canvasView.PaintSurface -= OnPaintSurface;
			}

			if (e.NewElement != null)
			{
				canvasView!.PaintSurface += OnPaintSurface;
				canvasView.EvasCanvas.AddEventAction(EvasObjectCallbackType.MouseDown, MouseDown);
				canvasView.EvasCanvas.AddEventAction(EvasObjectCallbackType.MouseUp, MouseUp);
				canvasView.EvasCanvas.AddEventAction(EvasObjectCallbackType.MouseMove, MouseMove);
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == DrawingView.PointsProperty.PropertyName)
			{
				canvasView!.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseUp, MouseUp);
				LoadPoints();
				canvasView.EvasCanvas.AddEventAction(EvasObjectCallbackType.MouseUp, MouseUp);
			}
		}

		void OnPointsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => LoadPoints();

		void MouseMove()
		{
			if (isDrawing)
			{
				var point = canvasView!.EvasCanvas.Pointer;
				Element.Points.Add(new Point(point.X, point.Y));
				canvasView.Invalidate();
			}
		}

		void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e) => DrawPath(e.Surface.Canvas);

		void MouseDown()
		{
			if (Element == null)
				return;

			Element.Points.Clear();
			canvasView?.Invalidate();
			isDrawing = true;
		}

		void MouseUp()
		{
			if (Element == null)
				return;

			isDrawing = false;

			if (Element.Points.Count > 0)
			{
				if (Element.DrawingCompletedCommand.CanExecute(null))
					Element.DrawingCompletedCommand.Execute(Element.Points);
			}

			if (Element.ClearOnFinish)
				Element.Points.Clear();
		}

		void LoadPoints()
		{
			if (Element == null)
				return;

			canvasView!.Invalidate();
		}

		void DrawPath(SKCanvas canvas)
		{
			canvas.Clear(SKColor.Empty);
			if (Element.Points.Count == 0)
				return;

			using var strokePaint = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = Element.LineColor.ToNative().ToSKColor(),
				StrokeWidth = Element.LineWidth,
				IsAntialias = true
			};

			var skPoints = Element.Points.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray();
			using var path = new SKPath();
			path.MoveTo(skPoints[0]);

			foreach (var point in skPoints)
				path.LineTo(point);

			canvas.DrawPath(path, strokePaint);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;
			disposed = true;

			if (Element != null)
			{
				Element.Points.CollectionChanged -= OnPointsCollectionChanged;
				canvasView!.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseDown, MouseDown);
				canvasView.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseUp, MouseUp);
				canvasView.EvasCanvas.DeleteEventAction(EvasObjectCallbackType.MouseMove, MouseMove);
				canvasView.PaintSurface -= OnPaintSurface;
			}

			base.Dispose(disposing);
		}
	}
}