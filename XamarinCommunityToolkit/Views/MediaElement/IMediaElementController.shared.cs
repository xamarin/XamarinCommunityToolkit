using System;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public interface IMediaElementController
	{
		double BufferingProgress { get; set; }
		MediaElementState CurrentState { get; set; }
		TimeSpan? Duration { get; set; }
		TimeSpan Position { get; set; }
		int VideoHeight { get; set; }
		int VideoWidth { get; set; }
		double Volume { get; set; }

		void OnMediaEnded();
		void OnMediaFailed();
		void OnMediaOpened();
		void OnSeekCompleted();
	}
}