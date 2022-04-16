using System;using Microsoft.Extensions.Logging;

namespace Xamarin.CommunityToolkit.Effects
{
	public class LongPressCompletedEventArgs : EventArgs
	{
		internal LongPressCompletedEventArgs(object? parameter)
			=> Parameter = parameter;

		public object? Parameter { get; }
	}
}