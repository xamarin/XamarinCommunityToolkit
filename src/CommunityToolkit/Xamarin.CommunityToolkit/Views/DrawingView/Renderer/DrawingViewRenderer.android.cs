using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Point = Xamarin.Forms.Point;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(DrawingView), typeof(DrawingViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// Android renderer for <see cref="Xamarin.CommunityToolkit.UI.Views.DrawingViewRenderer"/>
	/// </summary>
	public class DrawingViewRenderer : ViewRenderer<DrawingView, View>
	{
		bool disposed;

		readonly Paint canvasPaint;
		readonly Paint drawPaint;
		readonly Path drawPath;
		Bitmap? canvasBitmap;
		Canvas? drawCanvas;
		Line? currentLine;

		public DrawingViewRenderer(Context context)
			: base(context)
		{
			drawPath = new Path();
			drawPaint = new Paint
			{
				AntiAlias = true
			};

			drawPaint.SetStyle(Paint.Style.Stroke);
			drawPaint.StrokeJoin = Paint.Join.Round;
			drawPaint.StrokeCap = Paint.Cap.Round;

			canvasPaint = new Paint
			{
				Dither = true
			};
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == DrawingView.LinesProperty.PropertyName)
				LoadLines();
		}

		protected override void OnElementChanged(ElementChangedEventArgs<DrawingView> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
			{
				SetBackgroundColor(Element.BackgroundColor.ToAndroid());
				drawPaint.Color = Element.DefaultLineColor.ToAndroid();
				drawPaint.StrokeWidth = Element.DefaultLineWidth;
				Element.Lines.CollectionChanged += OnLinesCollectionChanged;
			}
		}

		void OnLinesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => LoadLines();

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			const int minW = 1;
			const int minH = 1;
			w = w < minW ? minW : w;
			h = h < minH ? minH : h;

			base.OnSizeChanged(w, h, oldw, oldh);

			canvasBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888 ?? throw new NullReferenceException("Unable to create Bitmap config"));
			if (canvasBitmap is not null)
			{
				drawCanvas = new Canvas(canvasBitmap);
				LoadLines();
			}
		}

		protected override void OnDraw(Canvas? canvas)
		{
			base.OnDraw(canvas);

			if (canvas is not null && canvasBitmap is not null)
			{
				canvas.DrawBitmap(canvasBitmap, 0, 0, canvasPaint);
				canvas.DrawPath(drawPath, drawPaint);
			}
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			var touchX = e.GetX();
			var touchY = e.GetY();

			switch (e.Action)
			{
				case MotionEventActions.Down:
					Parent?.RequestDisallowInterceptTouchEvent(true);
					if (!Element.MultiLineMode)
					{
						Element.Lines.Clear();
					}

					currentLine = new Line()
					{
						Points = new System.Collections.ObjectModel.ObservableCollection<Point>()
						{
							new (touchX, touchY)
						}
					};

					drawPath.MoveTo(touchX, touchY);
					break;
				case MotionEventActions.Move:
					if (touchX > 0 && touchY > 0 && touchX < drawCanvas?.Width && touchY < drawCanvas?.Height)
						drawPath.LineTo(touchX, touchY);

					currentLine?.Points.Add(new Point(touchX, touchY));
					break;
				case MotionEventActions.Up:
					Parent?.RequestDisallowInterceptTouchEvent(false);
					drawCanvas?.DrawPath(drawPath, drawPaint);
					drawPath.Reset();
					if (currentLine != null)
					{
						Element.Lines.Add(currentLine);
						Element.OnDrawingLineCompleted(currentLine);
					}

					if (Element.ClearOnFinish)
						Element.Lines.Clear();

					currentLine = null;
					break;
				default:
					return false;
			}

			Invalidate();

			return true;
		}

		public override bool OnInterceptTouchEvent(MotionEvent ev)
		{
			if (!Enabled || Element?.IsEnabled == false)
				return true;

			return base.OnInterceptTouchEvent(ev);
		}

		IList<Point> NormalizePoints(IEnumerable<Point> points)
		{
			var newPoints = new List<Point>();
			foreach (var point in points)
			{
				var pointX = point.X;
				var pointY = point.Y;
				if (pointX < 0)
				{
					pointX = 0;
				}

				if (pointX > drawCanvas?.Width)
				{
					pointX = drawCanvas?.Width ?? 0;
				}

				if (point.Y < 0)
				{
					pointY = 0;
				}

				if (pointY > drawCanvas?.Height)
				{
					pointY = drawCanvas?.Height ?? 0;
				}

				newPoints.Add(new Point(pointX, pointY));
			}

			return newPoints;
		}

		void LoadLines()
		{
			if (drawCanvas is null)
				return;

			drawCanvas.DrawColor(Element.BackgroundColor.ToAndroid());
			drawPath.Reset();
			var lines = Element.Lines;
			if (lines.Count > 0)
			{
				Draw(lines, drawCanvas, drawPath);
			}

			Invalidate();
		}

		void Draw(IEnumerable<Line> lines, in Canvas canvas, Path? path = null)
		{
			foreach (var line in lines)
			{
				path ??= new Path();
				var points = NormalizePoints(line.EnableSmoothedPath
					? line.Points.SmoothedPathWithGranularity(line.Granularity)
					: line.Points);
				path.MoveTo((float)points[0].X, (float)points[0].Y);
				foreach (var (x, y) in points)
				{
					var pointX = (float)x;
					var pointY = (float)y;

					path.LineTo(pointX, pointY);
				}

				canvas.DrawPath(path, drawPaint);
				path.Reset();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				drawCanvas?.Dispose();
				drawPaint.Dispose();
				drawPath.Dispose();
				canvasBitmap?.Dispose();
				canvasPaint.Dispose();
				if (Element != null)
					Element.Lines.CollectionChanged -= OnLinesCollectionChanged;
			}

			disposed = true;

			base.Dispose(disposing);
		}
	}
}