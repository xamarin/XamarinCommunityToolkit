using System.ComponentModel;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Size = Windows.Foundation.Size;

[assembly: ExportRenderer(typeof(DrawingView), typeof(DrawingViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class DrawingViewRenderer : ViewRenderer<DrawingView, InkCanvas>
	{
		InkCanvas? canvas;
		InkDrawingAttributes? inkDrawingAttributes;
		bool disposed;

		protected override void OnElementChanged(ElementChangedEventArgs<DrawingView> e)
		{
			base.OnElementChanged(e);
			if (Control == null && Element != null)
			{
				canvas = new InkCanvas();
				inkDrawingAttributes = new InkDrawingAttributes
				{
					Color = Element.LineColor.ToWindowsColor(),
					Size = new Size(Element.LineWidth, Element.LineWidth)
				};
				canvas.InkPresenter.UpdateDefaultDrawingAttributes(inkDrawingAttributes);
				canvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse |
													   CoreInputDeviceTypes.Pen |
													   CoreInputDeviceTypes.Touch;
				Element.Points.CollectionChanged += OnPointsCollectionChanged;
				SetNativeControl(canvas);
			}

			if (e.OldElement != null)
			{
				canvas!.InkPresenter.StrokeInput.StrokeStarted -= StrokeInput_StrokeStarted;
				canvas.InkPresenter.StrokesCollected -= OnInkPresenterStrokesCollected;
			}

			if (e.NewElement != null)
			{
				canvas!.InkPresenter.StrokeInput.StrokeStarted += StrokeInput_StrokeStarted;
				canvas.InkPresenter.StrokesCollected += OnInkPresenterStrokesCollected;
			}
		}

		void OnPointsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => LoadPoints();

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == DrawingView.PointsProperty.PropertyName)
			{
				canvas!.InkPresenter.StrokesCollected -= OnInkPresenterStrokesCollected;
				canvas.InkPresenter.StrokeContainer.Clear();
				LoadPoints();
				canvas.InkPresenter.StrokesCollected += OnInkPresenterStrokesCollected;
			}
		}

		void OnInkPresenterStrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
		{
			Element.Points.CollectionChanged -= OnPointsCollectionChanged;

			var points = args.Strokes.First()
							 .GetInkPoints()
							 .Select(point => new Point(point.Position.X, point.Position.Y))
							 .ToList();
			var elementPoints = Element.Points;

			elementPoints.Clear();

			foreach (var point in points)
				elementPoints.Add(point);

			Element.OnDrawingCompleted();

			if (Element.ClearOnFinish)
				Clear();

			Element.Points.CollectionChanged += OnPointsCollectionChanged;
		}

		void StrokeInput_StrokeStarted(InkStrokeInput sender, PointerEventArgs args)
		{
			Clear();
		}

		void LoadPoints()
		{
			var stylusPoints = Element?.Points?.Select(point => new Windows.Foundation.Point(point.X, point.Y)).ToList();
			if (stylusPoints is { Count: > 0 })
			{
				var strokeBuilder = new InkStrokeBuilder();
				strokeBuilder.SetDefaultDrawingAttributes(inkDrawingAttributes);
				var stroke = strokeBuilder.CreateStroke(stylusPoints);
				canvas!.InkPresenter.StrokeContainer.AddStroke(stroke);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			disposed = true;

			if (Element != null)
			{
				Element.Points.CollectionChanged -= OnPointsCollectionChanged;
				canvas!.InkPresenter.StrokeInput.StrokeStarted -= StrokeInput_StrokeStarted;
				canvas.InkPresenter.StrokesCollected -= OnInkPresenterStrokesCollected;
			}

			base.Dispose(disposing);
		}

		void Clear()
		{
			canvas!.InkPresenter.StrokeContainer.Clear();
			Element.Points.Clear();
		}
	}
}