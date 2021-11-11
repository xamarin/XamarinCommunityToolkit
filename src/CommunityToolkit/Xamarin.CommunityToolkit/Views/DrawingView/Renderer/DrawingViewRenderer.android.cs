using System.ComponentModel;
using System.Linq;
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

			canvasBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888!)!;
			drawCanvas = new Canvas(canvasBitmap);
			LoadLines();
		}

		protected override void OnDraw(Canvas? canvas)
		{
			base.OnDraw(canvas);

			foreach (var line in Element.Lines)
			{
				var path = new Path();
				path.MoveTo((float)line.Points[0].X, (float)line.Points[0].Y);
				foreach (var (x, y) in line.Points)
					path.LineTo((float)x, (float)y);

				canvas?.DrawPath(path, drawPaint);
			}

			canvas?.DrawBitmap(canvasBitmap!, 0, 0, canvasPaint);
			canvas?.DrawPath(drawPath, drawPaint);
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
							new Point(touchX, touchY)
						}
					};

					drawCanvas!.DrawColor(Element.BackgroundColor.ToAndroid(), PorterDuff.Mode.Clear!);
					drawPath.MoveTo(touchX, touchY);
					break;
				case MotionEventActions.Move:
					if (touchX > 0 && touchY > 0 && touchX < drawCanvas!.Width && touchY < drawCanvas.Height)
					{
						drawPath.LineTo(touchX, touchY);
						currentLine!.Points.Add(new Point(touchX, touchY));
					}
					break;
				case MotionEventActions.Up:
					Parent?.RequestDisallowInterceptTouchEvent(false);
					drawCanvas!.DrawPath(drawPath, drawPaint);
					drawPath.Reset();
					if (currentLine != null)
					{
						Element.Lines.Add(currentLine);
						Element.OnDrawingLineCompleted(currentLine);
					}

					if (Element.ClearOnFinish)
						Element.Lines.Clear();

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

		void LoadLines()
		{
			drawCanvas!.DrawColor(Element.BackgroundColor.ToAndroid(), PorterDuff.Mode.Clear!);
			drawPath.Reset();
			var lines = Element.Lines;
			if (lines.Count > 0)
			{
				foreach (var line in lines)
				{
					drawPath.MoveTo((float)line.Points[0].X, (float)line.Points[0].Y);
					foreach (var (x, y) in line.Points)
						drawPath.LineTo((float)x, (float)y);

					drawCanvas.DrawPath(drawPath, drawPaint);
					drawPath.Reset();
				}
			}

			Invalidate();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				drawCanvas!.Dispose();
				drawPaint.Dispose();
				drawPath.Dispose();
				canvasBitmap!.Dispose();
				canvasPaint.Dispose();
				if (Element != null)
					Element.Lines.CollectionChanged -= OnLinesCollectionChanged;
			}

			disposed = true;

			base.Dispose(disposing);
		}
	}
}