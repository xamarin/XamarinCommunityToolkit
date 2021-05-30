using System;

namespace CommunityToolkit.Maui.Effects
{
	public class TouchStateChangedEventArgs : EventArgs
	{
		internal TouchStateChangedEventArgs(TouchState state)
			=> State = state;

		public TouchState State { get; }
	}
}