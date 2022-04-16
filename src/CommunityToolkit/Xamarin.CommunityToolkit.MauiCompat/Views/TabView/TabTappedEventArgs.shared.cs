using System;using Microsoft.Extensions.Logging;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class TabTappedEventArgs : EventArgs
	{
		public TabTappedEventArgs(int position) => Position = position;

		public int Position { get; set; }
	}
}