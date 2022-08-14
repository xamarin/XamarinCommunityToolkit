using System;
using System.ComponentModel;
using System.Linq;
using Cairo;
using Gdk;
using Gtk;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;
using Xamarin.Forms.Platform.GTK.Extensions;
using Point = Xamarin.Forms.Point;

[assembly: ExportRenderer(typeof(DrawingView), typeof(DrawingViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// GTK renderer for <see cref="Xamarin.CommunityToolkit.UI.Views.DrawingViewRenderer"/>
	/// </summary>
	public class DrawingViewRenderer : ViewRenderer<DrawingView, VBox>
	{
		bool disposed;

		DrawingArea? area;
		bool isDrawing;
		PointD point;
		PointD previousPoint;
		ImageSurface? surface;
		Line? currentLine;

		protected override void OnElementChanged(ElementChangedEventArgs<DrawingView> e)
		{
			base.OnElementChanged(e);
			if (Control == null && Element != null)
			{
				var vBox = new VBox();
				surface = new ImageSurface(Format.Argb32, 500, 500);
				area = new DrawingArea();

				area.ModifyBg(StateType.Normal, Element.BackgroundColor.ToGtkColor());
				area.AddEvents((int)EventMask.PointerMotionMask |
							   (int)EventMask.ButtonPressMask |
							   (int)EventMask.ButtonReleaseMask);

				point = new PointD(500.0, 500.0);
				isDrawing = false;

				vBox.Add(area);
				Element.Lines.CollectionChanged += OnLinesCollectionChanged;
				SetNativeControl(vBox);
			}

			if (e.OldElement != null && area is not null)
			{
				area.ExposeEvent -= OnDrawingAreaExposed;
				area.ButtonPressEvent -= OnMousePress;
				area.ButtonReleaseEvent -= OnMouseRelease;
				area.MotionNotifyEvent -= OnMouseMotion;
			}

			if (e.NewElement != null && area is not null)
			{
				area.ExposeEvent += OnDrawingAreaExposed;
				area.ButtonPressEvent += OnMousePress;
				area.ButtonReleaseEvent += OnMouseRelease;
				area.MotionNotifyEvent += OnMouseMotion;
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == DrawingView.LinesProperty.PropertyName)
			{
				surface = new ImageSurface(Format.Argb32, Convert.ToInt32(Element.Width),
										   Convert.ToInt32(Element.Height));
				LoadPoints(surface);
			}
		}

		void OnLinesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => LoadPoints(surface);

		void OnDrawingAreaExposed(object source, ExposeEventArgs args)
		{
			if (area is null)
			{
				return;
			}

			Context ctx;
			using (ctx = CairoHelper.Create(area.GdkWindow))
			{
				ctx.SetSource(new SurfacePattern(surface));
				ctx.Paint();
			}

			if (isDrawing)
			{
				using (ctx = CairoHelper.Create(area.GdkWindow))
					DrawPoint(ctx, point);
			}
		}

		void OnMousePress(object source, ButtonPressEventArgs args)
		{
			surface = new ImageSurface(Format.Argb32, Convert.ToInt32(Element.Width), Convert.ToInt32(Element.Height));

			point.X = args.Event.X;
			point.Y = args.Event.Y;
			currentLine = new Line()
			{
				Points = new System.Collections.ObjectModel.ObservableCollection<Point>()
				{
					new Point(point.X, point.Y)
				}
			};
			previousPoint = point;
			isDrawing = true;
			area?.QueueDraw();
			if (!Element.MultiLineMode)
				Element.Lines.Clear();
		}

		void OnMouseRelease(object source, ButtonReleaseEventArgs args)
		{
			point.X = args.Event.X;
			point.Y = args.Event.Y;
			isDrawing = false;

			using var ctx = new Context(surface);
			DrawPoint(ctx, point);

			area?.QueueDraw();
			if (currentLine != null)
			{
				Element.Lines.Add(currentLine);
				Element.OnDrawingLineCompleted(currentLine);
			}

			if (Element.ClearOnFinish)
				Element.Lines.Clear();
		}

		void OnMouseMotion(object source, MotionNotifyEventArgs args)
		{
			if (isDrawing)
			{
				point.X = args.Event.X;
				point.Y = args.Event.Y;

				using var ctx = new Context(surface);
				DrawPoint(ctx, point);

				area?.QueueDraw();
			}
		}

		void DrawPoint(Context ctx, PointD pointD)
		{
			if (currentLine is not null)
			{
				ctx.SetSourceRGBA(currentLine.LineColor.R, currentLine.LineColor.G, currentLine.LineColor.B, currentLine.LineColor.A);
				ctx.LineWidth = currentLine.LineWidth;
				currentLine.Points.Add(new Point(pointD.X, pointD.Y));
			}

			ctx.MoveTo(previousPoint);
			previousPoint = pointD;
			ctx.LineTo(pointD);
			ctx.Stroke();
		}

		void LoadPoints(ImageSurface? imageSurface)
		{
			if (imageSurface is null)
				return;

			var lines = Element.Lines;
			if (lines.Count > 0)
			{
				foreach (var line in lines)
				{
					var newPointsPath = line.EnableSmoothedPath
						? line.Points.SmoothedPathWithGranularity(line.Granularity)
						: line.Points;
					var stylusPoints = newPointsPath.Select(stylusPoint => new PointD(stylusPoint.X, stylusPoint.Y)).ToList();
					if (stylusPoints is { Count: > 0 })
					{
						previousPoint = stylusPoints[0];
						using var ctx = new Context(imageSurface);

						foreach (var stylusPoint in stylusPoints)
							DrawPoint(ctx, stylusPoint);

						area?.QueueDraw();
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				if (area is not null)
				{
					area.ExposeEvent -= OnDrawingAreaExposed;
					area.ButtonPressEvent -= OnMousePress;
					area.ButtonReleaseEvent -= OnMouseRelease;
					area.MotionNotifyEvent -= OnMouseMotion;
					area.Dispose();
				}

				surface?.Dispose();
				if (Element != null)
					Element.Lines.CollectionChanged -= OnLinesCollectionChanged;
			}

			disposed = true;

			base.Dispose(disposing);
		}
	}
}