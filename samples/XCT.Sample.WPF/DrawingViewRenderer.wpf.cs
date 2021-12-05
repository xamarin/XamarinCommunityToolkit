using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Sample.WPF;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WPF;

[assembly: ExportRenderer(typeof(DrawingView), typeof(DrawingViewRenderer))]

namespace Xamarin.CommunityToolkit.Sample.WPF
{
	public class DrawingViewRenderer : ViewRenderer<DrawingView, InkCanvas>
	{
		InkCanvas? canvas;

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == DrawingView.LinesProperty.PropertyName)
			{
				canvas!.Strokes.StrokesChanged -= OnStrokesChanged;
				canvas.Strokes.Clear();
				LoadLines();
				canvas.Strokes.StrokesChanged += OnStrokesChanged;
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<DrawingView> e)
		{
			base.OnElementChanged(e);
			if (Control == null && Element != null)
			{
				canvas = new InkCanvas
				{
					Background = Element.BackgroundColor.ToBrush(),
					DefaultDrawingAttributes = new()
					{
						Color = Element.DefaultLineColor.ToMediaColor(),
						Width = Element.DefaultLineWidth,
						Height = Element.DefaultLineWidth
					}
				};
				Element.Lines.CollectionChanged += OnCollectionChanged;
				SetNativeControl(canvas);

				canvas.Strokes.StrokesChanged += OnStrokesChanged;
				Control!.PreviewMouseDown += OnPreviewMouseDown;
			}

			if (e.OldElement != null)
			{
				canvas!.Strokes.StrokesChanged -= OnStrokesChanged;
				Element!.Lines.CollectionChanged -= OnCollectionChanged;
				if (Control != null)
					Control.PreviewMouseDown -= OnPreviewMouseDown;
			}
		}

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			canvas!.Strokes.StrokesChanged -= OnStrokesChanged;
			canvas.Strokes.Clear();
			LoadLines();
			canvas.Strokes.StrokesChanged += OnStrokesChanged;
		}

		void OnPreviewMouseDown(object sender, MouseButtonEventArgs e) => Clear();

		void Clear(bool force = false)
		{
			if (!Element.MultiLineMode || force)
			{
				canvas!.Strokes.Clear();
				Element.Lines.Clear();
			}
		}

		void OnStrokesChanged(object sender, StrokeCollectionChangedEventArgs e)
		{
			Element.Lines.CollectionChanged -= OnCollectionChanged;
			if (e.Added.Count > 0)
			{
				if (!Element.MultiLineMode)
				{
					Element.Lines.Clear();
				}

				var lines = Element.MultiLineMode ? e.Added : new StrokeCollection() { e.Added.First() };

				foreach (var line in lines)
				{
					var points = line.StylusPoints.Select(point => new Point(point.X, point.Y)).ToList();
					Element.Lines.Add(new Line()
					{
						Points = new ObservableCollection<Point>(points),
						LineColor = Color.FromRgba(line.DrawingAttributes.Color.R, line.DrawingAttributes.Color.G,
							line.DrawingAttributes.Color.B, line.DrawingAttributes.Color.A),
						LineWidth = (float)line.DrawingAttributes.Width
					});
				}

				if (Element.Lines.Count > 0)
				{
					var lastLine = Element.Lines.Last();
					if (Element.DrawingLineCompletedCommand?.CanExecute(lastLine) ?? false)
						Element.DrawingLineCompletedCommand.Execute(lastLine);
				}

				if (Element.ClearOnFinish)
				{
					Element.Lines.CollectionChanged -= OnCollectionChanged;
					Clear(true);
					canvas!.Strokes.StrokesChanged += OnStrokesChanged;
				}
			}

			Element.Lines.CollectionChanged += OnCollectionChanged;
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
				var stylusPoints = line.Points.Select(point => new StylusPoint(point.X, point.Y)).ToList();
				if (stylusPoints is { Count: > 0 })
				{
					var stroke = new Stroke(new StylusPointCollection(stylusPoints))
					{
						DrawingAttributes = new()
						{
							Color = line.LineColor.ToMediaColor(),
							Width = line.LineWidth,
							Height = line.LineWidth
						}
					};
					canvas!.Strokes.Add(stroke);
				}
			}
		}
	}
}