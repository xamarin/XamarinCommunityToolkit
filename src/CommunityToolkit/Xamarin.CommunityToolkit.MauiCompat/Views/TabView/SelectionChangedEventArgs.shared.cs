using System;using Microsoft.Extensions.Logging;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class TabSelectionChangedEventArgs : EventArgs
	{
		public int NewPosition { get; set; }

		public int OldPosition { get; set; }
	}
}