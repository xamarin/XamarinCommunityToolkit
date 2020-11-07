using System;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class TabSelectionChangedEventArgs : EventArgs
	{
		public int NewPosition { get; set; }

		public int OldPosition { get; set; }
	}
}