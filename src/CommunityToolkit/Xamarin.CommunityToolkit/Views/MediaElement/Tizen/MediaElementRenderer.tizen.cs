using System;
using System.Threading.Tasks;
using ElmSharp;
using Tizen.Multimedia;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;

[assembly: ExportRenderer(typeof(MediaElement), typeof(MediaElementRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class MediaElementRenderer : ViewRenderer<MediaElement, LayoutCanvas>, IMediaViewProvider, IVideoOutput
	{
		bool disposed;
		MediaPlayer? mediaPlayer;
		MediaView? mediaView;
		View? controller;
		EvasObject? nativeController;

		IMediaElementController Controller => Element;

		VisualElement IVideoOutput.MediaView => Element;

		View? IVideoOutput.Controller
		{
			get => controller;
			set
			{
				if (controller != null)
				{
					controller.Parent = null;
					if (nativeController != null)
					{
						Control.Children.Remove(nativeController);
						nativeController.Unrealize();
					}
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

		MediaView? IMediaViewProvider.GetMediaView() => mediaView;

		protected override void OnElementChanged(ElementChangedEventArgs<MediaElement> e)
		{
			if (e.OldElement != null)
			{
				e.OldElement.SeekRequested -= OnSeekRequested;
				e.OldElement.StateRequested -= OnStateRequested;
			}
			if (Control == null)
			{
				SetNativeControl(new LayoutCanvas(Forms.Forms.NativeParent));
				if (Control != null)
				{
					mediaView = new MediaView(Forms.Forms.NativeParent);
					Control.LayoutUpdated += OnLayoutUpdated;
					Control.Children.Add(mediaView);
					Control.AllowFocus(true);
				}

				mediaPlayer = new MediaPlayer()
				{
					VideoOutput = this
				};

				mediaPlayer.PlaybackStarted += OnPlaybackStarted;
				mediaPlayer.PlaybackPaused += OnPlaybackPaused;
				mediaPlayer.PlaybackStopped += OnPlaybackStopped;
				mediaPlayer.PlaybackCompleted += OnPlaybackCompleted;
				mediaPlayer.BufferingProgressUpdated += OnBufferingProgressUpdated;
				mediaPlayer.ErrorOccurred += OnErrorOccurred;
				mediaPlayer.MediaPrepared += OnMediaPrepared;

				mediaPlayer.BindingContext = Element;
				mediaPlayer.SetBinding(MediaPlayer.SourceProperty, nameof(Element.Source));
				mediaPlayer.SetBinding(MediaPlayer.UsesEmbeddingControlsProperty, nameof(Element.ShowsPlaybackControls));
				mediaPlayer.SetBinding(MediaPlayer.AutoPlayProperty, nameof(Element.AutoPlay));
				mediaPlayer.SetBinding(MediaPlayer.VolumeProperty, nameof(Element.Volume));
				mediaPlayer.SetBinding(MediaPlayer.IsLoopingProperty, nameof(Element.IsLooping));
				mediaPlayer.SetBinding(MediaPlayer.AspectModeProperty, new Binding
				{
					Path = nameof(Element.Aspect),
					Converter = new AspectToDisplayAspectModeConverter()
				});
				BindableObject.SetInheritedBindingContext(mediaPlayer, Element.BindingContext);

				Element.SetBinding(MediaElement.PositionProperty, new Binding
				{
					Path = nameof(Element.Position),
					Source = mediaPlayer,
					Converter = new IntToTimeSpanConverter()
				});

				Element.SeekRequested += OnSeekRequested;
				Element.StateRequested += OnStateRequested;
			}
			base.OnElementChanged(e);
		}

		void OnLayoutUpdated(object sender, Forms.Platform.Tizen.Native.LayoutEventArgs e) => OnLayout();

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			disposed = true;

			if (disposing)
			{
				Controller.OnMediaEnded();
				Element.SeekRequested -= OnSeekRequested;
				Element.StateRequested -= OnStateRequested;
				mediaView?.Unrealize();
				if (mediaPlayer != null)
				{
					mediaPlayer.PlaybackStarted -= OnPlaybackStarted;
					mediaPlayer.PlaybackPaused -= OnPlaybackPaused;
					mediaPlayer.PlaybackStopped -= OnPlaybackStopped;
					mediaPlayer.PlaybackCompleted -= OnPlaybackCompleted;
					mediaPlayer.BufferingProgressUpdated -= OnBufferingProgressUpdated;
					mediaPlayer.ErrorOccurred -= OnErrorOccurred;
					mediaPlayer.MediaPrepared -= OnMediaPrepared;
					mediaPlayer.Dispose();
				}
				if (controller != null)
				{
					controller.Parent = null;
					Platform.SetRenderer(controller, null);
				}
				nativeController?.Unrealize();
				if (Control != null)
				{
					Control.LayoutUpdated -= OnLayoutUpdated;
					Control?.Unrealize();
				}
			}
			base.Dispose(disposing);
		}

		protected void OnLayout()
		{
			if (mediaView != null)
				mediaView.Geometry = Control.Geometry;

			controller?.Layout(Element.Bounds);

			if (nativeController != null)
				nativeController.Geometry = Control.Geometry;
		}

		async void OnSeekRequested(object sender, SeekRequested e)
		{
			if (mediaPlayer == null)
			{
				return;
			}

			await mediaPlayer.Seek((int)e.Position.TotalMilliseconds);
			Controller.OnSeekCompleted();
		}

		async void OnStateRequested(object sender, StateRequested e)
		{
			switch (e.State)
			{
				case MediaElementState.Playing:
					await (mediaPlayer?.Start() ?? Task.CompletedTask);
					break;
				case MediaElementState.Paused:
					mediaPlayer?.Pause();
					break;
				case MediaElementState.Stopped:
					await (mediaPlayer?.Stop() ?? Task.CompletedTask);
					break;
			}
		}

		protected void OnPlaybackStarted(object sender, EventArgs e)
		{
			Controller.CurrentState = MediaElementState.Playing;
		}

		protected void OnPlaybackPaused(object sender, EventArgs e)
		{
			Controller.CurrentState = MediaElementState.Paused;
		}

		protected void OnPlaybackStopped(object sender, EventArgs e)
		{
			Controller.CurrentState = MediaElementState.Stopped;
		}

		protected void OnPlaybackCompleted(object sender, EventArgs e)
		{
			Controller.OnMediaEnded();
		}

		protected void OnBufferingProgressUpdated(object sender, BufferingProgressUpdatedEventArgs e)
		{
			if (e.Progress == 1.0)
			{
				switch (mediaPlayer?.State)
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

		protected void OnErrorOccurred(object sender, EventArgs e)
		{
			Controller.OnMediaFailed();
		}

		protected async void OnMediaPrepared(object sender, EventArgs e)
		{
			if (mediaPlayer != null)
			{
				Controller.Duration = TimeSpan.FromMilliseconds(mediaPlayer.Duration);
				var videoSize = await mediaPlayer.GetVideoSize();
				Controller.VideoWidth = (int)videoSize.Width;
				Controller.VideoHeight = (int)videoSize.Height;
				Controller.OnMediaOpened();
			}
		}
	}

	public interface IMediaViewProvider
	{
		MediaView? GetMediaView();
	}
}