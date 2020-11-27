using System;

namespace Xamarin.CommunityToolkit.Effects
{
	public class TouchInteractionStatusChangedEventArgs : EventArgs
	{
		internal TouchInteractionStatusChangedEventArgs(TouchInteractionStatus touchInteractionStatus)
			=> TouchInteractionStatus = touchInteractionStatus;

		public TouchInteractionStatus TouchInteractionStatus { get; }
	}
}
