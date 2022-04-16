using System;using Microsoft.Extensions.Logging;

namespace Xamarin.CommunityToolkit.Effects
{
	public class TouchCompletedEventArgs : EventArgs
	{
		internal TouchCompletedEventArgs(object? parameter)
			=> Parameter = parameter;

		public object? Parameter { get; }
	}
}