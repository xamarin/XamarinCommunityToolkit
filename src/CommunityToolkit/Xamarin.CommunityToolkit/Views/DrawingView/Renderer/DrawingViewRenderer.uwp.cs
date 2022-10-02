using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
	/// <summary>
	/// UWP renderer for <see cref="Xamarin.CommunityToolkit.UI.Views.DrawingViewRenderer"/>
	/// </summary>
	public class DrawingViewRenderer : ViewRenderer<DrawingView, InkCanvas>
	{
		InkCanvas? canvas;
		bool disposed;

		protected override void OnElementChanged(ElementChangedEventArgs<DrawingView> e)
		{
			base.OnElementChanged(e);
			if (Control == null && Element != null)
			{
				canvas = new InkCanvas();
				var inkDrawingAttributes = new InkDrawingAttributes
				{
					Color = Element.DefaultLineColor.ToWindowsColor(),
					Size = new Size(Element.DefaultLineWidth, Element.DefaultLineWidth)
				};
				canvas.InkPresenter.UpdateDefaultDrawingAttributes(inkDrawingAttributes);
				canvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse |
													   CoreInputDeviceTypes.Pen |
													   CoreInputDeviceTypes.Touch;

				Element.Lines.CollectionChanged += OnCollectionChanged;
				SetNativeControl(canvas);

				canvas.InkPresenter.StrokeInput.StrokeStarted += StrokeInput_StrokeStarted;
				canvas.InkPresenter.StrokesCollected += OnInkPresenterStrokesCollected;
			}

			if (e.OldElement != null)
			{
				if (canvas is not null)
				{
					canvas.InkPresenter.StrokeInput.StrokeStarted -= StrokeInput_StrokeStarted;
					canvas.InkPresenter.StrokesCollected -= OnInkPresenterStrokesCollected;
				}

				if (Element is not null)
				{
					Element.Lines.CollectionChanged -= OnCollectionChanged;
				}
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == DrawingView.LinesProperty.PropertyName && canvas is not null)
			{
				canvas.InkPresenter.StrokesCollected -= OnInkPresenterStrokesCollected;
				canvas.InkPresenter.StrokeContainer.Clear();
				LoadLines();
				canvas.InkPresenter.StrokesCollected += OnInkPresenterStrokesCollected;
			}
		}

		void OnInkPresenterStrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs e)
		{
			Element.Lines.CollectionChanged -= OnCollectionChanged;
			if (e.Strokes.Count > 0)
			{
				if (!Element.MultiLineMode)
				{
					Element.Lines.Clear();
				}

				var lines = Element.MultiLineMode ? e.Strokes : new List<InkStroke>() { e.Strokes.First() };

				foreach (var line in lines)
				{
					var points = line.GetInkPoints().Select(point => new Point(point.Position.X, point.Position.Y));
					Element.Lines.Add(new Line()
					{
						Points = new ObservableCollection<Point>(points),
						LineColor = Color.FromRgba(line.DrawingAttributes.Color.R, line.DrawingAttributes.Color.G,
							line.DrawingAttributes.Color.B, line.DrawingAttributes.Color.A),
						LineWidth = (float)line.DrawingAttributes.Size.Width
					});
				}

				if (Element.Lines.Count > 0)
				{
					var lastLine = Element.Lines.Last();
					Element.OnDrawingLineCompleted(lastLine);
				}

				if (Element.ClearOnFinish)
				{
					Element.Lines.CollectionChanged -= OnCollectionChanged;
					Clear(true);
					Element.Lines.CollectionChanged += OnCollectionChanged;
				}
			}

			Element.Lines.CollectionChanged += OnCollectionChanged;
		}

		void StrokeInput_StrokeStarted(InkStrokeInput sender, PointerEventArgs args) => Clear();

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (canvas is not null)
			{
				canvas.InkPresenter.StrokesCollected -= OnInkPresenterStrokesCollected;
				canvas.InkPresenter.StrokeContainer.Clear();
				LoadLines();
				canvas.InkPresenter.StrokesCollected += OnInkPresenterStrokesCollected;
			}
		}

		void LoadLines()
		{
			var lines = Element.MultiLineMode
				? Element.Lines
				: Element.Lines.Any()
					? new ObservableCollection<Line> { Element.Lines.LastOrDefault() }
					: new ObservableCollection<Line>();
			foreach (var line in lines)
			{
				var newPointsPath = line.EnableSmoothedPath
					? line.Points.SmoothedPathWithGranularity(line.Granularity)
					: line.Points;
				var stylusPoints = newPointsPath.Select(point => new Windows.Foundation.Point(point.X, point.Y)).ToList();
				if (stylusPoints is { Count: > 0 })
				{
					var strokeBuilder = new InkStrokeBuilder();
					var inkDrawingAttributes = new InkDrawingAttributes
					{
						Color = line.LineColor.ToWindowsColor(),
						Size = new Size(line.LineWidth, line.LineWidth)
					};
					strokeBuilder.SetDefaultDrawingAttributes(inkDrawingAttributes);
					var stroke = strokeBuilder.CreateStroke(stylusPoints);
					canvas?.InkPresenter.StrokeContainer.AddStroke(stroke);
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			disposed = true;

			if (Element != null)
			{
				Element.Lines.CollectionChanged -= OnCollectionChanged;
			}

			if (canvas is not null)
			{
				canvas.InkPresenter.StrokeInput.StrokeStarted -= StrokeInput_StrokeStarted;
				canvas.InkPresenter.StrokesCollected -= OnInkPresenterStrokesCollected;
			}

			base.Dispose(disposing);
		}

		void Clear(bool force = false)
		{
			if (!Element.MultiLineMode || force)
			{
				canvas?.InkPresenter.StrokeContainer.Clear();
				Element.Lines.Clear();
			}
		}
	}
}