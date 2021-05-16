using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.UI.Views.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DrawingView), typeof(DrawingViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class DrawingViewRenderer : ViewRenderer<DrawingView, UIView>
	{
		bool disposed;
		UIBezierPath currentPath;
		UIColor? lineColor;
		CGPoint previousPoint;

		public DrawingViewRenderer() => currentPath = new UIBezierPath();

		protected override void OnElementChanged(ElementChangedEventArgs<DrawingView> e)
		{
			base.OnElementChanged(e);
			if (Element != null)
			{
				BackgroundColor = Element.BackgroundColor.ToUIColor();
				currentPath.LineWidth = Element.LineWidth;
				lineColor = Element.LineColor.ToUIColor();
				Element.Points.CollectionChanged += OnPointsCollectionChanged;
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == DrawingView.PointsProperty.PropertyName)
				LoadPoints();
		}

		void OnPointsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => LoadPoints();

		public override void TouchesBegan(NSSet touches, UIEvent? evt)
		{
			Element.Points.CollectionChanged -= OnPointsCollectionChanged;
			Element.Points.Clear();
			currentPath.RemoveAllPoints();

			var touch = (UITouch)touches.AnyObject;
			previousPoint = touch.PreviousLocationInView(this);
			currentPath.MoveTo(previousPoint);

			SetNeedsDisplay();

			Element.Points.CollectionChanged += OnPointsCollectionChanged;
		}

		public override void TouchesMoved(NSSet touches, UIEvent? evt)
		{
			var touch = (UITouch)touches.AnyObject;
			var currentPoint = touch.LocationInView(this);
			AddPointToPath(currentPoint);
		}

		void AddPointToPath(CGPoint currentPoint)
		{
			currentPath.AddLineTo(currentPoint);
			SetNeedsDisplay();
			Element.Points.CollectionChanged -= OnPointsCollectionChanged;
			var point = currentPoint.ToPoint();
			Element.Points.Add(point);
			Element.Points.CollectionChanged += OnPointsCollectionChanged;
		}

		public override void TouchesEnded(NSSet touches, UIEvent? evt)
		{
			UpdatePath();
			if (Element.Points.Count > 0)
			{
				if (Element.DrawingCompletedCommand.CanExecute(null))
					Element.DrawingCompletedCommand.Execute(Element.Points);
			}

			if (Element.ClearOnFinish)
				Element.Points.Clear();
		}

		public override void TouchesCancelled(NSSet touches, UIEvent? evt) => InvokeOnMainThread(SetNeedsDisplay);

		public override void Draw(CGRect rect)
		{
			lineColor!.SetStroke();
			currentPath.Stroke();
		}

		void LoadPoints()
		{
			var stylusPoints = Element.Points.Select(point => new CGPoint(point.X, point.Y)).ToList();
			currentPath.RemoveAllPoints();
			if (stylusPoints.Count > 0)
			{
				previousPoint = stylusPoints[0];
				currentPath.MoveTo(previousPoint);
				foreach (var point in stylusPoints)
					AddPointToPath(point);

				UpdatePath();
			}
		}

		void UpdatePath()
		{
			var smoothedPoints = Element.EnableSmoothedPath
				? SmoothedPathWithGranularity(Element.Points, Element.Granularity, ref currentPath)
				: new ObservableCollection<Point>(Element.Points);

			Element.Points.CollectionChanged -= OnPointsCollectionChanged;
			SetNeedsDisplay();
			Element.Points.Clear();

			foreach (var point in smoothedPoints)
				Element.Points.Add(point);

			Element.Points.CollectionChanged += OnPointsCollectionChanged;
		}

		ObservableCollection<Point> SmoothedPathWithGranularity(ObservableCollection<Point> currentPoints,
			int granularity,
			ref UIBezierPath smoothedPath)
		{
			// not enough points to smooth effectively, so return the original path and points.
			if (currentPoints.Count < granularity + 2)
				return new ObservableCollection<Point>(currentPoints);

			// create a new bezier path to hold the smoothed path.
			smoothedPath.RemoveAllPoints();
			var smoothedPoints = new ObservableCollection<Point>();

			// duplicate the first and last points as control points.
			currentPoints.Insert(0, currentPoints[0]);
			currentPoints.Add(currentPoints[^1]);

			// add the first point
			smoothedPath.MoveTo(currentPoints[0].X, currentPoints[0].Y);
			smoothedPoints.Add(currentPoints[0]);

			var currentPointsCount = currentPoints.Count;
			for (var index = 1; index < currentPointsCount - 2; index++)
			{
				var p0 = currentPoints[index - 1];
				var p1 = currentPoints[index];
				var p2 = currentPoints[index + 1];
				var p3 = currentPoints[index + 2];

				// add n points starting at p1 + dx/dy up until p2 using Catmull-Rom splines
				for (var i = 1; i < granularity; i++)
				{
					var t = i * (1f / granularity);
					var tt = t * t;
					var ttt = tt * t;

					// intermediate point
					var mid = GetIntermediatePoint(p0, p1, p2, p3, t, tt, ttt);
					smoothedPath.LineTo(mid.X, mid.Y);
					smoothedPoints.Add(mid);
				}

				// add p2
				smoothedPath.LineTo(p2.X, p2.Y);
				smoothedPoints.Add(p2);
			}

			// add the last point
			var last = currentPoints[^1];
			smoothedPath.LineTo(last.X, last.Y);
			smoothedPoints.Add(last);
			return smoothedPoints;
		}

		Point GetIntermediatePoint(Point p0, Point p1, Point p2, Point p3, in float t, in float tt, in float ttt) =>
			new Point
			{
				X = 0.5f *
					((2f * p1.X) +
					 ((p2.X - p0.X) * t) +
					 (((2f * p0.X) - (5f * p1.X) + (4f * p2.X) - p3.X) * tt) +
					 (((3f * p1.X) - p0.X - (3f * p2.X) + p3.X) * ttt)),
				Y = 0.5f *
					((2 * p1.Y) +
					 ((p2.Y - p0.Y) * t) +
					 (((2 * p0.Y) - (5 * p1.Y) + (4 * p2.Y) - p3.Y) * tt) +
					 (((3 * p1.Y) - p0.Y - (3 * p2.Y) + p3.Y) * ttt))
			};

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				currentPath.Dispose();
				if (Element != null)
					Element.Points.CollectionChanged -= OnPointsCollectionChanged;
			}

			disposed = true;

			base.Dispose(disposing);
		}
	}
}