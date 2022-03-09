using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// Extension methods to support <see cref="DrawingView"/>
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Get smoothed path.
		/// </summary>
		public static ObservableCollection<Point> SmoothedPathWithGranularity(this ObservableCollection<Point> currentPoints,
			int granularity)
		{
			// not enough points to smooth effectively, so return the original path and points.
			if (currentPoints.Count < granularity + 2)
				return new ObservableCollection<Point>(currentPoints);

			var smoothedPoints = new ObservableCollection<Point>();

			// duplicate the first and last points as control points.
			currentPoints.Insert(0, currentPoints[0]);
			currentPoints.Add(currentPoints.Last());

			// add the first point
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
					smoothedPoints.Add(mid);
				}

				// add p2
				smoothedPoints.Add(p2);
			}

			// add the last point
			var last = currentPoints.Last();
			smoothedPoints.Add(last);
			return smoothedPoints;
		}

		static Point GetIntermediatePoint(Point p0, Point p1, Point p2, Point p3, in float t, in float tt, in float ttt) =>
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
	}
}