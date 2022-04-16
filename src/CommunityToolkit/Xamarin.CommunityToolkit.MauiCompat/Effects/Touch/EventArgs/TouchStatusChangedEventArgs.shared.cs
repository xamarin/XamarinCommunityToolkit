using System;using Microsoft.Extensions.Logging;

namespace Xamarin.CommunityToolkit.Effects
{
	public class TouchStatusChangedEventArgs : EventArgs
	{
		internal TouchStatusChangedEventArgs(TouchStatus status)
			=> Status = status;

		public TouchStatus Status { get; }
	}
}