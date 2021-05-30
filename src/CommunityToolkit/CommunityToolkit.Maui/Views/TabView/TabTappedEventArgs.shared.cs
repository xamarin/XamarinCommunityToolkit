using System;

namespace CommunityToolkit.Maui.UI.Views
{
	public class TabTappedEventArgs : EventArgs
	{
		public TabTappedEventArgs(int position) => Position = position;

		public int Position { get; set; }
	}
}