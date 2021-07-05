using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class DrawingCompletedEventArgs : EventArgs
	{
		public IEnumerable<Point> Points { get; }

		public DrawingCompletedEventArgs(IEnumerable<Point> points)
        {
			Points = points;
        }
	}
}
