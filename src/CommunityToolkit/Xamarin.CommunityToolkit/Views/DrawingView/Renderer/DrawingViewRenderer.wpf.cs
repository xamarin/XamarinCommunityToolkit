using System;
using System.Collections.Specialized;
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
		InkCanvas? canvas;
		static bool isInitialized;

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == DrawingView.PointsProperty.PropertyName)
			{
				canvas!.Strokes.StrokesChanged -= OnStrokesChanged;
				canvas.Strokes.Clear();
				LoadPoints();
				canvas.Strokes.StrokesChanged += OnStrokesChanged;
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<DrawingView> e)
		{
			if (!isInitialized)
			{
				throw new Exception("Call Init method");
			}

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
				Element.Points.CollectionChanged += OnCollectionChanged;
				SetNativeControl(canvas);

				canvas.Strokes.StrokesChanged += OnStrokesChanged;
				Control!.PreviewMouseDown += OnPreviewMouseDown;
			}

			if (e.OldElement != null)
			{
				canvas!.Strokes.StrokesChanged -= OnStrokesChanged;
				Element!.Points.CollectionChanged -= OnCollectionChanged;
				if (Control != null)
					Control.PreviewMouseDown -= OnPreviewMouseDown;
			}
		}

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args) => LoadPoints();

		void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
		{
			canvas!.Strokes.Clear();
			Element.Points.Clear();
		}

		void OnStrokesChanged(object sender, StrokeCollectionChangedEventArgs e)
		{
			Element.Points.CollectionChanged -= OnCollectionChanged;
			if (e.Added.Count > 0)
			{
				var points = e.Added.First().StylusPoints.Select(point => new Point(point.X, point.Y));
				Element.Points.Clear();
				foreach (var point in points)
					Element.Points.Add(point);

				if (Element.Points.Count > 0)
				{
					if (Element.DrawingCompletedCommand.CanExecute(null))
						Element.DrawingCompletedCommand.Execute(Element.Points);
				}

				if (Element.ClearOnFinish)
					Element.Points.Clear();
			}
			Element.Points.CollectionChanged += OnCollectionChanged;
		}

		void LoadPoints()
		{
			var stylusPoints = Element?.Points.Select(point => new StylusPoint(point.X, point.Y)).ToList();
			if (stylusPoints is { Count: > 0 })
			{
				var stroke = new Stroke(new StylusPointCollection(stylusPoints), canvas!.DefaultDrawingAttributes);
				canvas.Strokes.Add(stroke);
			}
		}

		public static void Init()
		{
			isInitialized = true;
		}
	}
}