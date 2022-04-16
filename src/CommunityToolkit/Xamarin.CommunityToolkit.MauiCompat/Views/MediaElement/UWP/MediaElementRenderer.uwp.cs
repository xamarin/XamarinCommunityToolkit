using System;using Microsoft.Extensions.Logging;
using Windows.System.Display;
using Windows.UI.Xaml;
using Xamarin.CommunityToolkit.Core;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using Controls = Windows.UI.Xaml.Controls;
using ToolKitMediaElement = Xamarin.CommunityToolkit.UI.Views.MediaElement;
using ToolKitMediaElementRenderer = Xamarin.CommunityToolkit.UI.Views.MediaElementRenderer;
using WMediaElementState = Windows.UI.Xaml.Media.MediaElementState;

[assembly: ExportRenderer(typeof(ToolKitMediaElement), typeof(ToolKitMediaElementRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class MediaElementRenderer : ViewRenderer<ToolKitMediaElement, Controls.MediaElement>
	{
		long bufferingProgressChangedToken;
		long positionChangedToken;
		readonly DisplayRequest request = new DisplayRequest();

		protected virtual void ReleaseControl()
		{
			if (Control == null)
				return;

			if (bufferingProgressChangedToken != 0)
			{
				Control.UnregisterPropertyChangedCallback(Controls.MediaElement.BufferingProgressProperty, bufferingProgressChangedToken);
				bufferingProgressChangedToken = 0;
			}

			if (positionChangedToken != 0)
			{
				Control.UnregisterPropertyChangedCallback(Controls.MediaElement.PositionProperty, positionChangedToken);
				positionChangedToken = 0;
			}

			Element.SeekRequested -= SeekRequested;
			Element.StateRequested -= StateRequested;
			Element.PositionRequested -= PositionRequested;

			Control.CurrentStateChanged -= ControlCurrentStateChanged;
			Control.SeekCompleted -= ControlSeekCompleted;
			Control.MediaOpened -= ControlMediaOpened;
			Control.MediaEnded -= ControlMediaEnded;
			Control.MediaFailed -= ControlMediaFailed;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			ReleaseControl();
		}

		protected override void OnElementChanged(Microsoft.Maui.Controls.Platform.ElementChangedEventArgs<ToolKitMediaElement> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
				ReleaseControl();

			if (e.NewElement != null)
			{
				SetNativeControl(new Controls.MediaElement());
				Control.HorizontalAlignment = HorizontalAlignment.Stretch;
				Control.VerticalAlignment = VerticalAlignment.Stretch;

				Control.AreTransportControlsEnabled = Element.ShowsPlaybackControls;
				Control.AutoPlay = Element.AutoPlay;
				Control.IsLooping = Element.IsLooping;
				Control.Stretch = Element.Aspect.ToStretch();

				bufferingProgressChangedToken = Control.RegisterPropertyChangedCallback(Controls.MediaElement.BufferingProgressProperty, BufferingProgressChanged);
				positionChangedToken = Control.RegisterPropertyChangedCallback(Controls.MediaElement.PositionProperty, PositionChanged);

				Element.SeekRequested += SeekRequested;
				Element.StateRequested += StateRequested;
				Element.PositionRequested += PositionRequested;
				Control.SeekCompleted += ControlSeekCompleted;
				Control.CurrentStateChanged += ControlCurrentStateChanged;
				Control.MediaOpened += ControlMediaOpened;
				Control.MediaEnded += ControlMediaEnded;
				Control.MediaFailed += ControlMediaFailed;

				UpdateSource();
			}
		}

		void PositionRequested(object? sender, EventArgs e)
		{
			if (Control != null)
				Controller.Position = Control.Position;
		}

		IMediaElementController Controller => Element as IMediaElementController;

		void StateRequested(object? sender, StateRequested e)
		{
			if (Control != null)
			{
				switch (e.State)
				{
					case MediaElementState.Playing:
						Control.Play();
						break;

					case MediaElementState.Paused:
						if (Control.CanPause)
							Control.Pause();
						break;

					case MediaElementState.Stopped:
						Control.Stop();
						break;
				}

				Controller.Position = Control.Position;
			}
		}

		void SeekRequested(object? sender, SeekRequested e)
		{
			if (Control != null && Control.CanSeek)
			{
				Control.Position = e.Position;
				Controller.Position = Control.Position;
			}
		}

		void ControlMediaFailed(object? sender, ExceptionRoutedEventArgs e) => Controller?.OnMediaFailed();

		void ControlMediaEnded(object? sender, RoutedEventArgs e)
		{
			if (Control != null)
				Controller.Position = Control.Position;

			Controller.CurrentState = MediaElementState.Stopped;
			Controller.OnMediaEnded();
		}

		void ControlMediaOpened(object? sender, RoutedEventArgs e)
		{
			Controller.Duration = Control.NaturalDuration.HasTimeSpan ? Control.NaturalDuration.TimeSpan : (TimeSpan?)null;
			Controller.VideoHeight = Control.NaturalVideoHeight;
			Controller.VideoWidth = Control.NaturalVideoWidth;
			Control.Volume = Element.Volume;
			Control.Stretch = Element.Aspect.ToStretch();

			Controller.OnMediaOpened();
		}

		void ControlCurrentStateChanged(object? sender, RoutedEventArgs e)
		{
			if (Element == null || Control == null)
				return;

			switch (Control.CurrentState)
			{
				case WMediaElementState.Playing:
					if (Element.KeepScreenOn)
						request.RequestActive();
					break;

				case WMediaElementState.Paused:
				case WMediaElementState.Stopped:
				case WMediaElementState.Closed:
					if (Element.KeepScreenOn)
						request.RequestRelease();
					break;
			}

			Controller.CurrentState = FromWindowsMediaElementState(Control.CurrentState);
		}

		protected static MediaElementState FromWindowsMediaElementState(WMediaElementState state) => state switch
		{
			WMediaElementState.Buffering => MediaElementState.Buffering,
			WMediaElementState.Closed => MediaElementState.Closed,
			WMediaElementState.Opening => MediaElementState.Opening,
			WMediaElementState.Paused => MediaElementState.Paused,
			WMediaElementState.Playing => MediaElementState.Playing,
			WMediaElementState.Stopped => MediaElementState.Stopped,
			_ => throw new ArgumentOutOfRangeException(),
		};

		void BufferingProgressChanged(DependencyObject sender, DependencyProperty dp)
		{
			if (Control != null)
				Controller.BufferingProgress = Control.BufferingProgress;
		}

		void PositionChanged(DependencyObject sender, DependencyProperty dp)
		{
			if (Control != null)
				Controller.Position = Control.Position;
		}

		void ControlSeekCompleted(object? sender, RoutedEventArgs e)
		{
			if (Control != null)
			{
				Controller.Position = Control.Position;
				Controller.OnSeekCompleted();
			}
		}

		protected override void OnElementPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(ToolKitMediaElement.Aspect):
					Control.Stretch = Element.Aspect.ToStretch();
					break;

				case nameof(ToolKitMediaElement.AutoPlay):
					Control.AutoPlay = Element.AutoPlay;
					break;

				case nameof(ToolKitMediaElement.IsLooping):
					Control.IsLooping = Element.IsLooping;
					break;

				case nameof(ToolKitMediaElement.KeepScreenOn):
					if (Element.KeepScreenOn)
					{
						if (Control.CurrentState == WMediaElementState.Playing)
							request.RequestActive();
					}
					else
						request.RequestRelease();
					break;

				case nameof(ToolKitMediaElement.ShowsPlaybackControls):
					Control.AreTransportControlsEnabled = Element.ShowsPlaybackControls;
					break;

				case nameof(ToolKitMediaElement.Source):
					UpdateSource();
					break;

				case nameof(ToolKitMediaElement.Width):
					Width = Math.Max(0, Element.Width);
					break;

				case nameof(ToolKitMediaElement.Height):
					Height = Math.Max(0, Element.Height);
					break;

				case nameof(ToolKitMediaElement.Volume):
					Control.Volume = Element.Volume;
					break;
			}

			base.OnElementPropertyChanged(sender, e);
		}

		protected virtual void UpdateSource()
		{
			if (Element.Source == null)
			{
				Control.Stop();
				Control.Source = null;
				return;
			}

			if (Element.Source is UriMediaSource uriSource)
				Control.Source = uriSource.Uri;
			else if (Element.Source is FileMediaSource fileSource)
				Control.Source = new Uri(fileSource.File);
		}
	}
}