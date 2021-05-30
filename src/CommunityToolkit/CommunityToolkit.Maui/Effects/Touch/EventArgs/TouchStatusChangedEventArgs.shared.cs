using System;

namespace CommunityToolkit.Maui.Effects
{
	public class TouchStatusChangedEventArgs : EventArgs
	{
		internal TouchStatusChangedEventArgs(TouchStatus status)
			=> Status = status;

		public TouchStatus Status { get; }
	}
}