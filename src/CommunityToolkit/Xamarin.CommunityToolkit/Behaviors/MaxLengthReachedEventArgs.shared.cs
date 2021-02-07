using System;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class MaxLengthReachedEventArgs : EventArgs
	{
		public string Text { get; }

		public MaxLengthReachedEventArgs(string text)
			=> Text = text;
	}
}