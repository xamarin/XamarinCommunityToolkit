using System;
using System.ComponentModel;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class StateRequested : EventArgs
	{
		public MediaElementState State { get; }

		public StateRequested(MediaElementState state) => State = state;
	}
}