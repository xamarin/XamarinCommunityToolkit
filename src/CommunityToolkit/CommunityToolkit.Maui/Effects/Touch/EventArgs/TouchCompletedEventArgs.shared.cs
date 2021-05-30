using System;

namespace CommunityToolkit.Maui.Effects
{
	public class TouchCompletedEventArgs : EventArgs
	{
		internal TouchCompletedEventArgs(object? parameter)
			=> Parameter = parameter;

		public object? Parameter { get; }
	}
}