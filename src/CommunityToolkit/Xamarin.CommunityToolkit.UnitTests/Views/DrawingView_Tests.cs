using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class DrawingView_Tests
	{
		[Test]
		public void AllInstancesShouldHaveTheirOwnPoints()
		{
			var drawingViewOne = new DrawingView();
			var drawingViewTwo = new DrawingView();

			drawingViewOne.Points.Add(Point.Zero);

			drawingViewTwo.Points.Clear();

			Assert.IsNotEmpty(drawingViewOne.Points);
			Assert.IsFalse(ReferenceEquals(drawingViewOne.Points, drawingViewTwo.Points));
		}

		[Test]
		public void DrawingCompletedCommandShouldNotFireWhenSubscribedAndNoPointsSet()
		{
			IEnumerable<Point>? points = null;
			var drawingView = new DrawingView();

			drawingView.DrawingCompletedCommand = new Command((p) => points = (IEnumerable<Point>)p);

			drawingView.OnDrawingCompleted();

			Assert.Null(points);
		}

		[Test]
		public void DrawingCompletedShouldFireWhenSubscribed()
		{
			IEnumerable<Point>? points = null;
			var drawingView = new DrawingView();

			drawingView.DrawingCompleted += (sender, args) => points = args.Points;

			drawingView.Points.Add(Point.Zero);
			drawingView.OnDrawingCompleted();

			Assert.NotNull(points);
			Assert.AreEqual(points.Count(), 1);
		}

		[Test]
		public void DrawingCompletedShouldNotFireWhenSubscribedAndNoPointsSet()
		{
			IEnumerable<Point>? points = null;
			var drawingView = new DrawingView();

			drawingView.DrawingCompleted += (sender, args) => points = args.Points;

			drawingView.OnDrawingCompleted();

			Assert.Null(points);
		}

		[Test]
		public void PointsShouldClearOnceCompleteAndClearOnFinish()
		{
			var drawingView = new DrawingView { ClearOnFinish = true };

			drawingView.Points.Add(Point.Zero);
			drawingView.OnDrawingCompleted();

			Assert.IsEmpty(drawingView.Points);
		}

		[Test]
		public void PointsShouldNotClearOnceCompleteAndNotClearOnFinish()
		{
			var drawingView = new DrawingView();

			drawingView.Points.Add(Point.Zero);
			drawingView.OnDrawingCompleted();

			Assert.IsNotEmpty(drawingView.Points);
		}
	}
}
