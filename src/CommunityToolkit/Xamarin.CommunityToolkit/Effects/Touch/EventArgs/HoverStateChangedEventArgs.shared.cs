using System;

namespace Xamarin.CommunityToolkit.Effects
{
	public class HoverStateChangedEventArgs : EventArgs
	{
		internal HoverStateChangedEventArgs(HoverState state)
			=> State = state;

		public HoverState State { get; }
	}
}
