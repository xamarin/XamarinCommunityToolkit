using System;
using Xamarin.Forms.Platform.Tizen;
using ElmSharp;
using Tizen.Multimedia;
using Xamarin.Forms;
using CommunityToolkit = Xamarin.CommunityToolkit.UI.Views;
using XForms = Xamarin.Forms.Forms;

[assembly: ExportRenderer(typeof(CommunityToolkit::MediaElement), typeof(MediaElementRenderer))]
namespace Xamarin.CommunityToolkit.Tizen.UI.Views
{
	public class MediaElementRenderer : ViewRenderer<CommunityToolkit::MediaElement, LayoutCanvas>, IMediaViewProvider, IVideoOutput
	{
		MediaPlayer player;
		MediaView mediaView;
		View controller;
		EvasObject nativeController;

		CommunityToolkit::IMediaElementController Controller => Element;

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

		protected override void OnElementChanged(ElementChangedEventArgs<CommunityToolkit::MediaElement> e)
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

		protected void OnSeekRequested(object sender, CommunityToolkit::SeekRequested e) => player.Seek((int)e.Position.TotalMilliseconds);

		protected void OnStateRequested(object sender, CommunityToolkit::StateRequested e)
		{
			switch (e.State)
			{
				case CommunityToolkit::MediaElementState.Playing:
					player.Start();
					break;
				case CommunityToolkit::MediaElementState.Paused:
					player.Pause();
					break;
				case CommunityToolkit::MediaElementState.Stopped:
					player.Stop();
					break;
			}
		}

		protected void OnPositionRequested(object sender, EventArgs e) => Controller.Position = TimeSpan.FromMilliseconds(player.Position);

		protected void OnPlaybackStarted(object sender, EventArgs e) => Controller.CurrentState = CommunityToolkit::MediaElementState.Playing;

		protected void OnPlaybackPaused(object sender, EventArgs e) => Controller.CurrentState = CommunityToolkit::MediaElementState.Paused;

		protected void OnPlaybackStopped(object sender, EventArgs e) => Controller.CurrentState = CommunityToolkit::MediaElementState.Stopped;

		protected void OnPlaybackCompleted(object sender, EventArgs e) => Controller.OnMediaEnded();

		protected void OnBufferingProgressUpdated(object sender, BufferingProgressUpdatedEventArgs e)
		{
			if (e.Progress == 1.0)
			{
				switch (player.State)
				{
					case PlaybackState.Paused:
						Controller.CurrentState = CommunityToolkit::MediaElementState.Paused;
						break;
					case PlaybackState.Playing:
						Controller.CurrentState = CommunityToolkit::MediaElementState.Playing;
						break;
					case PlaybackState.Stopped:
						Controller.CurrentState = CommunityToolkit::MediaElementState.Stopped;
						break;
				}
			}
			else if (Controller.CurrentState != CommunityToolkit::MediaElementState.Buffering && e.Progress >= 0)
			{
				Controller.CurrentState = CommunityToolkit::MediaElementState.Buffering;
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
