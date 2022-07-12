using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using AppKit;
using CoreGraphics;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

[assembly: ExportRenderer(typeof(DrawingView), typeof(DrawingViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// macOS renderer for <see cref="DrawingViewRenderer"/>
	/// </summary>
	public class DrawingViewRenderer : ViewRenderer<DrawingView, NSView>
	{
		readonly NSBezierPath currentPath;
		bool disposed;
		NSColor? lineColor;
		CGPoint previousPoint;
		Line? currentLine;

		public DrawingViewRenderer() => currentPath = new NSBezierPath();

		protected override void OnElementChanged(ElementChangedEventArgs<DrawingView> e)
		{
			base.OnElementChanged(e);
			if (Element != null)
			{
				WantsLayer = true;
				if (Layer is not null)
					Layer.BackgroundColor = Element.BackgroundColor.ToCGColor();

				currentPath.LineWidth = Element.DefaultLineWidth;
				lineColor = Element.DefaultLineColor.ToNSColor();
				Element.Lines.CollectionChanged += OnLinesCollectionChanged;
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == DrawingView.LinesProperty.PropertyName)
				LoadPoints();
		}

		void OnLinesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => LoadPoints();

		public override void MouseDown(NSEvent theEvent)
		{
			Element.Lines.CollectionChanged -= OnLinesCollectionChanged;
			if (!Element.MultiLineMode)
			{
				Element.Lines.Clear();
				currentPath.RemoveAllPoints();
			}

			previousPoint = theEvent.LocationInWindow;
			currentPath.MoveTo(previousPoint);
			currentLine = new Line
			{
				Points = new ObservableCollection<Point>
				{
					new (previousPoint.X, previousPoint.Y)
				}
			};

			UpdateDisplay();
			Element.Lines.CollectionChanged += OnLinesCollectionChanged;
		}

		public override void MouseUp(NSEvent theEvent)
		{
			if (currentLine != null)
			{
				Element.Lines.Add(currentLine);
				Element.OnDrawingLineCompleted(currentLine);
			}

			if (Element.ClearOnFinish)
				Element.Lines.Clear();

			currentLine = null;
		}

		public override void MouseDragged(NSEvent theEvent)
		{
			var currentPoint = theEvent.LocationInWindow;
			AddPointToPath(currentPoint);
			currentLine?.Points.Add(currentPoint.ToPoint());
			UpdateDisplay();
		}

		public override void DrawRect(CGRect dirtyRect)
		{
			base.DrawRect(dirtyRect);
			lineColor?.SetStroke();
			currentPath.Stroke();
		}

		void AddPointToPath(CGPoint currentPoint) => currentPath.LineTo(currentPoint);

		void LoadPoints()
		{
			currentPath.RemoveAllPoints();
			foreach (var line in Element.Lines)
			{
				var newPointsPath = line.EnableSmoothedPath
					? line.Points.SmoothedPathWithGranularity(line.Granularity)
					: line.Points;
				var stylusPoints = newPointsPath.Select(point => new CGPoint(point.X, point.Y)).ToList();
				if (stylusPoints.Count > 0)
				{
					previousPoint = stylusPoints[0];
					currentPath.MoveTo(previousPoint);
					foreach (var point in stylusPoints)
						AddPointToPath(point);
				}
			}

			UpdateDisplay();
		}

		void UpdateDisplay()
		{
			if (Layer is not null)
				InvokeOnMainThread(Layer.SetNeedsDisplay);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				currentPath.Dispose();
				if (Element != null)
					Element.Lines.CollectionChanged -= OnLinesCollectionChanged;
			}

			disposed = true;

			base.Dispose(disposing);
		}
	}
}