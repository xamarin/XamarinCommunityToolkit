using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using System.ComponentModel;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Xamarin.Forms.Platform.Android;
using Point = Xamarin.Forms.Point;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(DrawingView), typeof(DrawingViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class DrawingViewRenderer : ViewRenderer<DrawingView, View>
	{
		bool disposed;

		readonly Paint canvasPaint;
		readonly Paint drawPaint;
		readonly Path drawPath;
		Bitmap canvasBitmap;
		Canvas drawCanvas;

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
			if (e.PropertyName == DrawingView.PointsProperty.PropertyName)
			{
				LoadPoints();
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<DrawingView> e)
		{
			base.OnElementChanged(e);
			if (Element != null)
			{
				SetBackgroundColor(Element.BackgroundColor.ToAndroid());
				drawPaint.Color = Element.LineColor.ToAndroid();
				drawPaint.StrokeWidth = Element.LineWidth;
				Element.Points.CollectionChanged += (sender, args) =>
				{
					LoadPoints();
				};
			}
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);

			canvasBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
			drawCanvas = new Canvas(canvasBitmap);
			LoadPoints();
		}

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);

			canvas.DrawBitmap(canvasBitmap, 0, 0, canvasPaint);
			canvas.DrawPath(drawPath, drawPaint);
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			var touchX = e.GetX();
			var touchY = e.GetY();

			switch (e.Action)
			{
				case MotionEventActions.Down:
					Parent?.RequestDisallowInterceptTouchEvent(true);
					Element.Points.Clear();
					drawCanvas.DrawColor(Element.BackgroundColor.ToAndroid(), PorterDuff.Mode.Clear);
					drawPath.MoveTo(touchX, touchY);
					break;
				case MotionEventActions.Move:
					if (touchX > 0 && touchY > 0)
					{
						drawPath.LineTo(touchX, touchY);
					}

					Element.Points.Add(new Point(touchX, touchY));
					break;
				case MotionEventActions.Up:
					Parent?.RequestDisallowInterceptTouchEvent(false);
					drawCanvas.DrawPath(drawPath, drawPaint);
					drawPath.Reset();
					if (Element.Points.Any())
					{
						if (Element.DrawingCompletedCommand != null && Element.DrawingCompletedCommand.CanExecute(null))
						{
							Element.DrawingCompletedCommand.Execute(Element.Points);
						}
					}

					if (Element.ClearOnFinish)
					{
						Element.Points.Clear();
					}
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

		void LoadPoints()
		{
			drawCanvas.DrawColor(Element.BackgroundColor.ToAndroid(), PorterDuff.Mode.Clear);
			drawPath.Reset();
			if (Element.Points.Any())
			{
				drawPath.MoveTo((float)Element.Points[0].X, (float)Element.Points[0].Y);
				foreach (var (x, y) in Element.Points)
				{
					drawPath.LineTo((float)x, (float)y);
				}

				drawCanvas.DrawPath(drawPath, drawPaint);
				drawPath.Reset();

				Invalidate();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}

			if (disposing)
			{
				drawCanvas.Dispose();
				drawPaint.Dispose();
				drawPath.Dispose();
				canvasBitmap.Dispose();
				canvasPaint.Dispose();
			}

			disposed = true;

			base.Dispose(disposing);
		}
	}
}