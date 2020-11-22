using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

[assembly: ExportRenderer(typeof(DrawingView), typeof(DrawingViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class DrawingViewRenderer : ViewRenderer<DrawingView, InkCanvas>
	{
		InkCanvas canvas;

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == DrawingView.PointsProperty.PropertyName)
			{
				canvas.Strokes.StrokesChanged -= Strokes_StrokesChanged;
				canvas.Strokes.Clear();
				LoadPoints();
				canvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<DrawingView> e)
		{
			base.OnElementChanged(e);
			if (Control == null && Element != null)
			{
				canvas = new InkCanvas
				{
					DefaultDrawingAttributes =
					{
						Color = Element.LineColor.ToMediaColor(),
						Width = Element.LineWidth,
						Height = Element.LineWidth
					},
					Background = Element.BackgroundColor.ToBrush()
				};
				Element.Points.CollectionChanged += (sender, args) =>
				{
					LoadPoints();
				};
				SetNativeControl(canvas);
			}

			if (e.OldElement != null)
			{
				// Unsubscribe
				canvas.Strokes.StrokesChanged -= Strokes_StrokesChanged;
				if (Control != null)
				{
					Control.PreviewMouseDown -= Control_PreviewMouseDown;
				}
			}

			if (e.NewElement != null)
			{
				// Subscribe
				canvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
				if (Control != null)
				{
					Control.PreviewMouseDown += Control_PreviewMouseDown;
				}
			}
		}

		void Control_PreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			canvas.Strokes.Clear();
			Element.Points.Clear();
		}

		void Strokes_StrokesChanged(object sender, StrokeCollectionChangedEventArgs e)
		{
			if (e.Added.Any())
			{
				var points = e.Added.First().StylusPoints.Select(point => new Point(point.X, point.Y)).ToList();
				Element.Points.Clear();
				foreach (var point in points)
				{
					Element.Points.Add(point);
				}

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
		}

		void LoadPoints()
		{
			var stylusPoints = Element?.Points?.Select(point => new StylusPoint(point.X, point.Y)).ToList();
			if (stylusPoints != null && stylusPoints.Any())
			{
				var stroke = new Stroke(new StylusPointCollection(stylusPoints), canvas.DefaultDrawingAttributes);
				canvas.Strokes.Add(stroke);
			}
		}
	}
}