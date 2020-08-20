using System;
using System.ComponentModel;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class SeekRequested : EventArgs
	{
		public TimeSpan Position { get; }

		public SeekRequested(TimeSpan position) => Position = position;
	}
}