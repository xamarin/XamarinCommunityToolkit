using System;
using System.Globalization;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Platform.Tizen;
using Xamarin.Forms.Platform.Tizen.Native;
using ElmSharp;
using Tizen.Multimedia;
using XForms = Xamarin.Forms.Forms;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Tizen.UI.Views
{
	public class MediaElementRenderer : ViewRenderer<MediaElement, LayoutCanvas>, IMediaViewProvider, IVideoOutput
	{
		MediaPlayer player;
		MediaView mediaView;
		View controller;
		EvasObject nativeController;

		IMediaElementController Controller => Element as IMediaElementController;

		Forms.VisualElement IVideoOutput.MediaView => Element;

		View IVideoOutput.Controller
		{
			get => controller;
			set
			{
				if (controller != null)
				{
					controller.Parent = null;
					Control.Children.Remove(nativeController);
					nativeController.Unrealize();
				}

				controller = value;

				if (controller != null)
				{
					controller.Parent = Element;
					nativeController = Platform.GetOrCreateRenderer(controller).NativeView;
					Control.Children.Add(nativeController);
				}
			}
		}

		VideoOuputType IVideoOutput.OuputType => VideoOuputType.Buffer;

		MediaView IMediaViewProvider.GetMediaView() => mediaView;

		protected override void OnElementChanged(ElementChangedEventArgs<MediaElement> e)
		{
			if (e.OldElement != null)
			{
				e.OldElement.SeekRequested -= OnSeekRequested;
				e.OldElement.StateRequested -= OnStateRequested;
				e.OldElement.PositionRequested -= OnPositionRequested;
			}

			if (Control == null)
			{
				SetNativeControl(new LayoutCanvas(XForms.NativeParent));
				mediaView = new MediaView(XForms.NativeParent);
				Control.LayoutUpdated += (s, evt) => OnLayout();
				Control.Children.Add(mediaView);
				Control.AllowFocus(true);

				player = new MediaPlayer()
				{
					VideoOutput = this
				};
				player.PlaybackStarted += OnPlaybackStarted;
				player.PlaybackPaused += OnPlaybackPaused;
				player.PlaybackStopped += OnPlaybackStopped;
				player.PlaybackCompleted += OnPlaybackCompleted;
				player.BufferingProgressUpdated += OnBufferingProgressUpdated;
				player.ErrorOccurred += OnErrorOccurred;
				player.MediaPrepared += OnMediaPrepared;
				player.BindingContext = Element;
				player.SetBinding(MediaPlayer.SourceProperty, "Source");
				player.SetBinding(MediaPlayer.UsesEmbeddingControlsProperty, "ShowsPlaybackControls");
				player.SetBinding(MediaPlayer.AutoPlayProperty, "AutoPlay");
				player.SetBinding(MediaPlayer.VolumeProperty, "Volume");
				player.SetBinding(MediaPlayer.IsLoopingProperty, "IsLooping");
				player.SetBinding(MediaPlayer.AspectModeProperty, new Binding
				{
					Path = "Aspect",
					Converter = new AspectToDisplayAspectModeConverter()
				});
				BindableObject.SetInheritedBindingContext(player, Element.BindingContext);

				Element.SeekRequested += OnSeekRequested;
				Element.StateRequested += OnStateRequested;
				Element.PositionRequested += OnPositionRequested;
			}
			base.OnElementChanged(e);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Element.SeekRequested -= OnSeekRequested;
				Element.StateRequested -= OnStateRequested;
				Element.PositionRequested -= OnPositionRequested;
				mediaView?.Unrealize();
				if (player != null)
				{
					player.PlaybackStarted -= OnPlaybackStarted;
					player.PlaybackPaused -= OnPlaybackPaused;
					player.PlaybackStopped -= OnPlaybackStopped;
					player.PlaybackCompleted -= OnPlaybackCompleted;
					player.BufferingProgressUpdated -= OnBufferingProgressUpdated;
					player.ErrorOccurred -= OnErrorOccurred;
					player.MediaPrepared -= OnMediaPrepared;
					player.Dispose();
				}
				if (controller != null)
				{
					controller.Parent = null;
					Platform.SetRenderer(controller, null);
				}
				nativeController?.Unrealize();
				Control?.Unrealize();
			}
			base.Dispose(disposing);
		}

		protected void OnLayout()
		{
			mediaView.Geometry = Control.Geometry;
			controller?.Layout(Element.Bounds);
			if (nativeController != null)
				nativeController.Geometry = Control.Geometry;
		}

		protected void OnSeekRequested(object sender, SeekRequested e) => player.Seek((int)e.Position.TotalMilliseconds);

		protected void OnStateRequested(object sender, StateRequested e)
		{
			switch (e.State)
			{
				case MediaElementState.Playing:
					player.Start();
					break;
				case MediaElementState.Paused:
					player.Pause();
					break;
				case MediaElementState.Stopped:
					player.Stop();
					break;
			}
		}

		protected void OnPositionRequested(object sender, EventArgs e) => Controller.Position = TimeSpan.FromMilliseconds(player.Position);

		protected void OnPlaybackStarted(object sender, EventArgs e) => Controller.CurrentState = MediaElementState.Playing;

		protected void OnPlaybackPaused(object sender, EventArgs e) => Controller.CurrentState = MediaElementState.Paused;

		protected void OnPlaybackStopped(object sender, EventArgs e) => Controller.CurrentState = MediaElementState.Stopped;

		protected void OnPlaybackCompleted(object sender, EventArgs e) => Controller.OnMediaEnded();

		protected void OnBufferingProgressUpdated(object sender, BufferingProgressUpdatedEventArgs e)
		{
			if (e.Progress == 1.0)
			{
				switch (player.State)
				{
					case PlaybackState.Paused:
						Controller.CurrentState = MediaElementState.Paused;
						break;
					case PlaybackState.Playing:
						Controller.CurrentState = MediaElementState.Playing;
						break;
					case PlaybackState.Stopped:
						Controller.CurrentState = MediaElementState.Stopped;
						break;
				}
			}
			else if (Controller.CurrentState != MediaElementState.Buffering && e.Progress >= 0)
			{
				Controller.CurrentState = MediaElementState.Buffering;
			}
			Controller.BufferingProgress = e.Progress;
		}

		protected void OnErrorOccurred(object sender, EventArgs e) => Controller.OnMediaFailed();

		protected async void OnMediaPrepared(object sender, EventArgs e)
		{
			Controller.OnMediaOpened();
			Controller.Duration = TimeSpan.FromMilliseconds(player.Duration);
			var videoSize = await player.GetVideoSize();
			Controller.VideoWidth = (int)videoSize.Width;
			Controller.VideoHeight = (int)videoSize.Height;
		}
	}
}
