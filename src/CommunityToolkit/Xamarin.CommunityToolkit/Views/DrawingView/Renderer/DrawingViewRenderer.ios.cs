using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DrawingView), typeof(DrawingViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// iOS renderer for <see cref="DrawingViewRenderer"/>
	/// </summary>
	public class DrawingViewRenderer : ViewRenderer<DrawingView, UIView>
	{
		readonly UIBezierPath currentPath;
		bool disposed;
		UIColor? lineColor;
		CGPoint previousPoint;
		Line? currentLine;
		readonly List<ScrollViewRenderer> scrollViewParentRenderers = new();

		public DrawingViewRenderer() => currentPath = new UIBezierPath();

		protected override void OnElementChanged(ElementChangedEventArgs<DrawingView> e)
		{
			base.OnElementChanged(e);
			if (Element != null)
			{
				BackgroundColor = Element.BackgroundColor.ToUIColor();
				currentPath.LineWidth = Element.DefaultLineWidth;
				lineColor = Element.DefaultLineColor.ToUIColor();
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

		public override void TouchesBegan(NSSet touches, UIEvent? evt)
		{
			DetectScrollViews();
			SetParentTouches(false);

			Element.Lines.CollectionChanged -= OnLinesCollectionChanged;
			if (!Element.MultiLineMode)
			{
				Element.Lines.Clear();
				currentPath.RemoveAllPoints();
			}

			var touch = (UITouch)touches.AnyObject;
			previousPoint = touch.PreviousLocationInView(this);
			currentPath.MoveTo(previousPoint);
			currentLine = new Line
			{
				Points = new ObservableCollection<Point>
				{
					new (previousPoint.X, previousPoint.Y)
				}
			};

			SetNeedsDisplay();

			Element.Lines.CollectionChanged += OnLinesCollectionChanged;
		}

		public override void TouchesMoved(NSSet touches, UIEvent? evt)
		{
			var touch = (UITouch)touches.AnyObject;
			var currentPoint = touch.LocationInView(this);
			AddPointToPath(currentPoint);
			SetNeedsDisplay();
			currentLine?.Points.Add(currentPoint.ToPoint());
		}

		public override void TouchesEnded(NSSet touches, UIEvent? evt)
		{
			if (currentLine != null)
			{
				Element.Lines.Add(currentLine);
				Element.OnDrawingLineCompleted(currentLine);
			}

			if (Element.ClearOnFinish)
				Element.Lines.Clear();

			currentLine = null;
			SetParentTouches(true);
		}

		public override void TouchesCancelled(NSSet touches, UIEvent? evt)
		{
			currentLine = null;
			SetNeedsDisplay();
			SetParentTouches(true);
		}

		public override void Draw(CGRect rect)
		{
			lineColor?.SetStroke();
			currentPath.Stroke();
		}

		void AddPointToPath(CGPoint currentPoint) => currentPath.AddLineTo(currentPoint);

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

			SetNeedsDisplay();
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

		void DetectScrollViews()
		{
			if (scrollViewParentRenderers.Any())
				return;

			var parent = Superview;

			while (parent != null)
			{
				if (parent.GetType() == typeof(ScrollViewRenderer))
					scrollViewParentRenderers.Add((ScrollViewRenderer)parent);

				parent = parent.Superview;
			}
		}

		void SetParentTouches(bool enabled)
		{
			foreach (var scrollViewParentRenderer in scrollViewParentRenderers)
			{
				scrollViewParentRenderer.ScrollEnabled = enabled;
			}
		}
	}
}