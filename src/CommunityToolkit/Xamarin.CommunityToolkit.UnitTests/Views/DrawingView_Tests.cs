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
	}
}