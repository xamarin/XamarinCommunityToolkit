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
        InkCanvas canvas;
        InkDrawingAttributes inkDrawingAttributes;

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == DrawingView.PointsProperty.PropertyName)
            {
                canvas.InkPresenter.StrokesCollected -= InkPresenter_StrokesCollected;
                canvas.InkPresenter.StrokeContainer.Clear();
                LoadPoints();
                canvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            }
        }

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
				Element.Points.CollectionChanged += (sender, args) =>
				{
					LoadPoints();
				};
				SetNativeControl(canvas);
            }

            if (e.OldElement != null)
            {
                // Unsubscribe
                canvas.InkPresenter.StrokeInput.StrokeStarted -= StrokeInput_StrokeStarted;
                canvas.InkPresenter.StrokesCollected -= InkPresenter_StrokesCollected;
            }

            if (e.NewElement != null)
            {
                // Subscribe
                canvas.InkPresenter.StrokeInput.StrokeStarted += StrokeInput_StrokeStarted;
                canvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            }
        }

        void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            var points = args.Strokes.First()
                             .GetInkPoints()
                             .Select(point => new Point(point.Position.X, point.Position.Y))
                             .ToList();

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

        void StrokeInput_StrokeStarted(InkStrokeInput sender, PointerEventArgs args)
        {
            canvas.InkPresenter.StrokeContainer.Clear();
            Element.Points.Clear();
        }

        void LoadPoints()
        {
            var stylusPoints = Element?.Points?.Select(point => new Windows.Foundation.Point(point.X, point.Y))
                                      .ToList();
            if (stylusPoints != null && stylusPoints.Any())
            {
                var strokeBuilder = new InkStrokeBuilder();
                strokeBuilder.SetDefaultDrawingAttributes(inkDrawingAttributes);
                var stroke = strokeBuilder.CreateStroke(stylusPoints);
                canvas.InkPresenter.StrokeContainer.AddStroke(stroke);
            }
		}
	}
}