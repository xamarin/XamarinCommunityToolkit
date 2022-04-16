using System;using Microsoft.Extensions.Logging;

namespace Xamarin.CommunityToolkit.UI.Views
{
	/// <summary>
	/// Contains last drawing line
	/// </summary>
	public class DrawingLineCompletedEventArgs : EventArgs
	{
		/// <summary>
		/// Last drawing line
		/// </summary>
		public Line? Line { get; }

		/// <summary>
		/// Initializes last drawing line
		/// </summary>
		/// <param name="line">Last drawing line</param>
		public DrawingLineCompletedEventArgs(Line? line)
		{
			Line = line;
		}
	}
}