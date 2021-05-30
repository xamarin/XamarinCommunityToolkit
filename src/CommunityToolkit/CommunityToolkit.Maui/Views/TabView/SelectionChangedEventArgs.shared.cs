using System;

namespace CommunityToolkit.Maui.UI.Views
{
	public class TabSelectionChangedEventArgs : EventArgs
	{
		public int NewPosition { get; set; }

		public int OldPosition { get; set; }
	}
}