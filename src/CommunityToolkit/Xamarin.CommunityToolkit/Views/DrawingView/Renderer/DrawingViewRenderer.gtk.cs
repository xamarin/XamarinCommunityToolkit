using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cairo;
using Gdk;
using global::Gtk;
using Xamarin.Forms.Platform.GTK;
using Xamarin.Forms.Platform.GTK.Extensions;
using Point = Xamarin.Forms.Point;

[assembly: ExportRenderer(typeof(DrawingView), typeof(DrawingViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class DrawingViewRenderer : ViewRenderer<DrawingView, VBox>
	{
		bool disposed;

		DrawingArea area;
		bool isDrawing;
		PointD point;
		PointD previousPoint;
		ImageSurface surface;

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == DrawingView.PointsProperty.PropertyName)
			{
				surface = new ImageSurface(Format.Argb32, Convert.ToInt32(Element.Width),
										   Convert.ToInt32(Element.Height));
				LoadPoints(surface);
			}
		}

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
				Element.Points.CollectionChanged += (sender, args) =>
				{
					LoadPoints(surface);
				};
				SetNativeControl(vBox);
			}

			if (e.OldElement != null)
			{
				area.ExposeEvent -= OnDrawingAreaExposed;
				area.ButtonPressEvent -= OnMousePress;
				area.ButtonReleaseEvent -= OnMouseRelease;
				area.MotionNotifyEvent -= OnMouseMotion;
			}

			if (e.NewElement != null)
			{
				area.ExposeEvent += OnDrawingAreaExposed;
				area.ButtonPressEvent += OnMousePress;
				area.ButtonReleaseEvent += OnMouseRelease;
				area.MotionNotifyEvent += OnMouseMotion;
			}
		}

		void OnDrawingAreaExposed(object source, ExposeEventArgs args)
		{
			Context ctx;
			using (ctx = CairoHelper.Create(area.GdkWindow))
			{
				ctx.SetSource(new SurfacePattern(surface));
				ctx.Paint();
			}

			if (isDrawing)
			{
				using (ctx = CairoHelper.Create(area.GdkWindow))
				{
					DrawPoint(ctx, point);
				}
			}
		}

		void OnMousePress(object source, ButtonPressEventArgs args)
		{
			surface = new ImageSurface(Format.Argb32, Convert.ToInt32(Element.Width), Convert.ToInt32(Element.Height));

			point.X = args.Event.X;
			point.Y = args.Event.Y;
			previousPoint = point;
			isDrawing = true;
			area.QueueDraw();
			Element.Points.Clear();
		}

		void OnMouseRelease(object source, ButtonReleaseEventArgs args)
		{
			point.X = args.Event.X;
			point.Y = args.Event.Y;

			isDrawing = false;

			using (var ctx = new Context(surface))
			{
				DrawPoint(ctx, point);
			}

			area.QueueDraw();
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
		}

		void OnMouseMotion(object source, MotionNotifyEventArgs args)
		{
			if (isDrawing)
			{
				point.X = args.Event.X;
				point.Y = args.Event.Y;

				using (var ctx = new Context(surface))
				{
					DrawPoint(ctx, point);
				}

				area.QueueDraw();
			}
		}

		void DrawPoint(Context ctx, PointD pointD)
		{
			ctx.SetSourceRGBA(Element.LineColor.R, Element.LineColor.G, Element.LineColor.B, Element.LineColor.A);
			ctx.LineWidth = Element.LineWidth;
			ctx.MoveTo(previousPoint);
			previousPoint = pointD;
			ctx.LineTo(pointD);
			ctx.Stroke();
			Element.Points.Add(new Point(pointD.X, pointD.Y));
		}

		void LoadPoints(ImageSurface imageSurface)
		{
			var stylusPoints = Element?.Points?.Select(stylusPoint => new PointD(stylusPoint.X, stylusPoint.Y))
									  .ToList();
			if (stylusPoints != null && stylusPoints.Any())
			{
				previousPoint = stylusPoints[0];
				using (var ctx = new Context(imageSurface))
				{
					foreach (var stylusPoint in stylusPoints)
					{
						DrawPoint(ctx, stylusPoint);
					}
				}

				area.QueueDraw();
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
				area.Dispose();
				surface.Dispose();
			}

			disposed = true;

			base.Dispose(disposing);
		}
	}
}