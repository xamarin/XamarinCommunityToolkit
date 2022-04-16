using System;using Microsoft.Extensions.Logging;

namespace Xamarin.CommunityToolkit.Effects
{
	public class TouchInteractionStatusChangedEventArgs : EventArgs
	{
		internal TouchInteractionStatusChangedEventArgs(TouchInteractionStatus touchInteractionStatus)
			=> TouchInteractionStatus = touchInteractionStatus;

		public TouchInteractionStatus TouchInteractionStatus { get; }
	}
}