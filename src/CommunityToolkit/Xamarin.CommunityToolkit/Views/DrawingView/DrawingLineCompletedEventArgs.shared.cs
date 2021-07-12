using System;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class DrawingLineCompletedEventArgs : EventArgs
	{
		public Line Line { get; }

		public DrawingLineCompletedEventArgs(Line line)
		{
			Line = line;
		}
	}
}