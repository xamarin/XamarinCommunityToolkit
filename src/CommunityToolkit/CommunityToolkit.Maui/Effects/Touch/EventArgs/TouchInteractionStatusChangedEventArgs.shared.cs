using System;

namespace CommunityToolkit.Maui.Effects
{
	public class TouchInteractionStatusChangedEventArgs : EventArgs
	{
		internal TouchInteractionStatusChangedEventArgs(TouchInteractionStatus touchInteractionStatus)
			=> TouchInteractionStatus = touchInteractionStatus;

		public TouchInteractionStatus TouchInteractionStatus { get; }
	}
}