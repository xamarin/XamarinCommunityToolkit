using System.Linq;
using NUnit.Framework;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Views
{
	public class DrawingView_Tests
	{
		[Test]
		public void AllInstancesShouldHaveTheirOwnLines()
		{
			var drawingViewOne = new DrawingView();
			var drawingViewTwo = new DrawingView();

			drawingViewOne.Lines.Add(new Line());

			drawingViewTwo.Lines.Clear();

			Assert.IsNotEmpty(drawingViewOne.Lines);
			Assert.IsFalse(ReferenceEquals(drawingViewOne.Lines, drawingViewTwo.Lines));
		}

		[Test]
		public void AllLineInstancesShouldHaveTheirOwnPoints()
		{
			var drawingView = new DrawingView();

			var line1 = new Line();
			var line2 = new Line();
			drawingView.Lines.Add(line1);
			drawingView.Lines.Add(line2);

			line1.Points.Add(Point.Zero);

			line2.Points.Clear();

			Assert.IsNotEmpty(line1.Points);
			Assert.IsFalse(ReferenceEquals(line1.Points, line2.Points));
		}

		[Test]
		public void DrawingLineCompletedCommandShouldFireWhenSet()
		{
			Line? line = null;
			var drawingView = new DrawingView();

			drawingView.DrawingLineCompletedCommand = new Command<Line?>((l) => line = l);

			var newLine = new Line();
			drawingView.Lines.Add(newLine);
			drawingView.OnDrawingLineCompleted(newLine);

			Assert.NotNull(line);
			Assert.AreEqual(drawingView.Lines.Count(), 1);
		}

		[Test]
		public void DrawingLineCompletedCommandShouldNotFireWhenLineNotSet()
		{
			Line? line = null;
			var drawingView = new DrawingView();

			drawingView.DrawingLineCompletedCommand = new Command<Line?>((l) => line = l);

			drawingView.OnDrawingLineCompleted(null);

			Assert.Null(line);
		}

		[Test]
		public void DrawingLineCompletedShouldFireWhenSubscribed()
		{
			Line? line = null;
			var drawingView = new DrawingView();

			drawingView.DrawingLineCompleted += (sender, args) => line = args.Line;
			var newLine = new Line();
			drawingView.Lines.Add(newLine);
			drawingView.OnDrawingLineCompleted(newLine);

			Assert.NotNull(line);
			Assert.AreEqual(drawingView.Lines.Count(), 1);
		}

		[Test]
		public void DrawingLineCompletedShouldNotFireWhenSubscribedAndLineNotSet()
		{
			Line? line = null;
			var drawingView = new DrawingView();

			drawingView.DrawingLineCompleted += (sender, args) => line = args.Line;

			drawingView.OnDrawingLineCompleted(null);

			Assert.Null(line);
		}
	}
}
