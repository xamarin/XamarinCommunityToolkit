using System;
using System.IO;
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;
using XCT = Xamarin.CommunityToolkit.Core;
using ToolKitMediaElement = Xamarin.CommunityToolkit.UI.Views.MediaElement;
using ToolKitMediaElementRenderer = Xamarin.CommunityToolkit.UI.Views.MediaElementRenderer;

[assembly: ExportRenderer(typeof(ToolKitMediaElement), typeof(ToolKitMediaElementRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class MediaElementRenderer : ViewRenderer<ToolKitMediaElement, UIView>
	{
		IMediaElementController Controller => Element;

		protected readonly AVPlayerViewController avPlayerViewController = new AVPlayerViewController();
		protected NSObject playedToEndObserver;
		protected NSObject statusObserver;
		protected NSObject rateObserver;
		protected NSObject volumeObserver;
		bool idleTimerDisabled = false;

		public MediaElementRenderer() =>
			playedToEndObserver = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, PlayedToEnd);

		protected virtual void SetKeepScreenOn(bool value)
		{
			if (value)
			{
				if (!UIApplication.SharedApplication.IdleTimerDisabled)
				{
					idleTimerDisabled = true;
					UIApplication.SharedApplication.IdleTimerDisabled = true;
				}
			}
			else if (idleTimerDisabled)
			{
				idleTimerDisabled = false;
				UIApplication.SharedApplication.IdleTimerDisabled = false;
			}
		}

		protected virtual void UpdateSource()
		{
			if (Element.Source != null)
			{
				AVAsset asset = null;

				if (Element.Source is XCT.UriMediaSource uriSource)
				{
					if (uriSource.Uri.Scheme == "ms-appx")
					{
						if (uriSource.Uri.LocalPath.Length <= 1)
							return;

						// used for a file embedded in the application package
						asset = AVAsset.FromUrl(NSUrl.FromFilename(uriSource.Uri.LocalPath.Substring(1)));
					}
					else if (uriSource.Uri.Scheme == "ms-appdata")
					{
						var filePath = ResolveMsAppDataUri(uriSource.Uri);

						if (string.IsNullOrEmpty(filePath))
							throw new ArgumentException("Invalid Uri", "Source");

						asset = AVAsset.FromUrl(NSUrl.FromFilename(filePath));
					}
					else
						asset = AVUrlAsset.Create(NSUrl.FromString(uriSource.Uri.AbsoluteUri));
				}
				else
				{
					if (Element.Source is XCT.FileMediaSource fileSource)
						asset = AVAsset.FromUrl(NSUrl.FromFilename(fileSource.File));
				}

				var item = new AVPlayerItem(asset);
				RemoveStatusObserver();

				statusObserver = (NSObject)item.AddObserver("status", NSKeyValueObservingOptions.New, ObserveStatus);

				if (avPlayerViewController.Player != null)
					avPlayerViewController.Player.ReplaceCurrentItemWithPlayerItem(item);
				else
				{
					avPlayerViewController.Player = new AVPlayer(item);
					rateObserver = (NSObject)avPlayerViewController.Player.AddObserver("rate", NSKeyValueObservingOptions.New, ObserveRate);
					volumeObserver = (NSObject)avPlayerViewController.Player.AddObserver("volume", NSKeyValueObservingOptions.New, ObserveVolume);
				}

				if (Element.AutoPlay)
					Play();
			}
			else
			{
				if (Element.CurrentState == MediaElementState.Playing || Element.CurrentState == MediaElementState.Buffering)
				{
					avPlayerViewController.Player.Pause();
					Controller.CurrentState = MediaElementState.Stopped;
				}
			}
		}

		protected string ResolveMsAppDataUri(Uri uri)
		{
			if (uri.Scheme == "ms-appdata")
			{
				string filePath;

				if (uri.LocalPath.StartsWith("/local"))
				{
					var libraryPath = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.LibraryDirectory, NSSearchPathDomain.User)[0].Path;
					filePath = Path.Combine(libraryPath, uri.LocalPath.Substring(7));
				}
				else if (uri.LocalPath.StartsWith("/temp"))
					filePath = Path.Combine(Path.GetTempPath(), uri.LocalPath.Substring(6));
				else
					throw new ArgumentException("Invalid Uri", "Source");

				return filePath;
			}
			else
				throw new ArgumentException("uri");
		}

		protected void RemoveStatusObserver()
		{
			if (statusObserver != null)
			{
				try
				{
					avPlayerViewController?.Player?.CurrentItem?.RemoveObserver(statusObserver, "status");
				}
				finally
				{
					statusObserver = null;
				}
			}
		}

		protected virtual void ObserveRate(NSObservedChange e)
		{
			if (Controller is object)
			{
				switch (avPlayerViewController.Player.Rate)
				{
					case 0.0f:
						Controller.CurrentState = MediaElementState.Paused;
						break;

					case 1.0f:
						Controller.CurrentState = MediaElementState.Playing;
						break;
				}

				Controller.Position = Position;
			}
		}

		void ObserveVolume(NSObservedChange e)
		{
			if (Controller == null)
				return;

			Controller.Volume = avPlayerViewController.Player.Volume;
		}

		protected void ObserveStatus(NSObservedChange e)
		{
			Controller.Volume = avPlayerViewController.Player.Volume;

			switch (avPlayerViewController.Player.Status)
			{
				case AVPlayerStatus.Failed:
					Controller.OnMediaFailed();
					break;

				case AVPlayerStatus.ReadyToPlay:
					var duration = avPlayerViewController.Player.CurrentItem.Duration;

					if (duration.IsIndefinite)
						Controller.Duration = TimeSpan.Zero;
					else
						Controller.Duration = TimeSpan.FromSeconds(duration.Seconds);

					Controller.VideoHeight = (int)avPlayerViewController.Player.CurrentItem.Asset.NaturalSize.Height;
					Controller.VideoWidth = (int)avPlayerViewController.Player.CurrentItem.Asset.NaturalSize.Width;
					Controller.OnMediaOpened();
					Controller.Position = Position;
					break;
			}
		}

		TimeSpan Position
		{
			get
			{
				if (avPlayerViewController.Player?.CurrentTime.IsInvalid ?? true)
					return TimeSpan.Zero;

				return TimeSpan.FromSeconds(avPlayerViewController.Player.CurrentTime.Seconds);
			}
		}

		void PlayedToEnd(NSNotification notification)
		{
			if (Element == null)
				return;

			if (Element.IsLooping)
			{
				avPlayerViewController.Player?.Seek(CMTime.Zero);
				Controller.Position = Position;
				avPlayerViewController.Player?.Play();
			}
			else
			{
				SetKeepScreenOn(false);
				Controller.Position = Position;

				try
				{
					Device.BeginInvokeOnMainThread(Controller.OnMediaEnded);
				}
				catch (Exception e)
				{
					Log.Warning("MediaElement", $"Failed to play media to end: {e}");
				}
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(ToolKitMediaElement.Aspect):
					avPlayerViewController.VideoGravity = AspectToGravity(Element.Aspect);
					break;

				case nameof(ToolKitMediaElement.KeepScreenOn):
					if (!Element.KeepScreenOn)
						SetKeepScreenOn(false);
					else if (Element.CurrentState == MediaElementState.Playing)
					{
						// only toggle this on if property is set while video is already running
						SetKeepScreenOn(true);
					}
					break;

				case nameof(ToolKitMediaElement.ShowsPlaybackControls):
					avPlayerViewController.ShowsPlaybackControls = Element.ShowsPlaybackControls;
					break;

				case nameof(ToolKitMediaElement.Source):
					UpdateSource();
					break;

				case nameof(ToolKitMediaElement.Volume):
					if (avPlayerViewController.Player != null)
						avPlayerViewController.Player.Volume = (float)Element.Volume;
					break;
			}
		}

		void MediaElementSeekRequested(object sender, SeekRequested e)
		{
			if (avPlayerViewController.Player?.CurrentItem == null || avPlayerViewController.Player.Status != AVPlayerStatus.ReadyToPlay)
				return;

			var ranges = avPlayerViewController.Player.CurrentItem.SeekableTimeRanges;
			var seekTo = new CMTime(Convert.ToInt64(e.Position.TotalMilliseconds), 1000);
			foreach (var v in ranges)
			{
				if (seekTo >= v.CMTimeRangeValue.Start && seekTo < (v.CMTimeRangeValue.Start + v.CMTimeRangeValue.Duration))
				{
					avPlayerViewController.Player.Seek(seekTo, SeekComplete);
					break;
				}
			}
		}

		protected virtual void Play()
		{
			var audioSession = AVAudioSession.SharedInstance();
			var err = audioSession.SetCategory(AVAudioSession.CategoryPlayback);

			if (!(err is null))
				Log.Warning("MediaElement", "Failed to set AVAudioSession Category {0}", err.Code);

			audioSession.SetMode(AVAudioSession.ModeMoviePlayback, out err);
			if (!(err is null))
				Log.Warning("MediaElement", "Failed to set AVAudioSession Mode {0}", err.Code);

			err = audioSession.SetActive(true);
			if (!(err is null))
				Log.Warning("MediaElement", "Failed to set AVAudioSession Active {0}", err.Code);

			if (avPlayerViewController.Player != null)
			{
				avPlayerViewController.Player.Play();
				Controller.CurrentState = MediaElementState.Playing;
			}

			if (Element.KeepScreenOn)
				SetKeepScreenOn(true);
		}

		void MediaElementStateRequested(object sender, StateRequested e)
		{
			switch (e.State)
			{
				case MediaElementState.Playing:
					Play();
					break;

				case MediaElementState.Paused:
					if (Element.KeepScreenOn)
						SetKeepScreenOn(false);

					if (avPlayerViewController.Player != null)
					{
						avPlayerViewController.Player.Pause();
						Controller.CurrentState = MediaElementState.Paused;
					}
					break;

				case MediaElementState.Stopped:
					if (Element.KeepScreenOn)
						SetKeepScreenOn(false);

					// iOS has no stop...
					avPlayerViewController?.Player.Pause();
					avPlayerViewController?.Player.Seek(CMTime.Zero);
					Controller.CurrentState = MediaElementState.Stopped;

					var err = AVAudioSession.SharedInstance().SetActive(false);

					if (!(err is null))
						Log.Warning("MediaElement", "Failed to set AVAudioSession Inactive {0}", err.Code);

					break;
			}

			Controller.Position = Position;
		}

		static AVLayerVideoGravity AspectToGravity(Aspect aspect) =>
			aspect switch
			{
				Aspect.Fill => AVLayerVideoGravity.Resize,
				Aspect.AspectFill => AVLayerVideoGravity.ResizeAspectFill,
				_ => AVLayerVideoGravity.ResizeAspect,
			};

		void SeekComplete(bool finished)
		{
			if (finished)
				Controller.OnSeekCompleted();
		}

		void MediaElementPositionRequested(object sender, EventArgs e) => Controller.Position = Position;

		protected override void OnElementChanged(ElementChangedEventArgs<MediaElement> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;
				e.OldElement.SeekRequested -= MediaElementSeekRequested;
				e.OldElement.StateRequested -= MediaElementStateRequested;
				e.OldElement.PositionRequested -= MediaElementPositionRequested;

				if (playedToEndObserver != null)
				{
					NSNotificationCenter.DefaultCenter.RemoveObserver(playedToEndObserver);
					playedToEndObserver = null;
				}

				SetKeepScreenOn(false);
				RemoveStatusObserver();
				rateObserver?.Dispose();
				rateObserver = null;
				volumeObserver?.Dispose();
				volumeObserver = null;

				// stop video if playing
				if (avPlayerViewController?.Player?.CurrentItem != null)
				{
					if (avPlayerViewController?.Player?.Rate > 0)
					{
						avPlayerViewController?.Player?.Pause();
					}
					avPlayerViewController?.Player?.ReplaceCurrentItemWithPlayerItem(null);
					AVAudioSession.SharedInstance().SetActive(false);
				}
			}

			if (e.NewElement != null)
			{
				SetNativeControl(avPlayerViewController.View);

				Element.PropertyChanged += OnElementPropertyChanged;
				Element.SeekRequested += MediaElementSeekRequested;
				Element.StateRequested += MediaElementStateRequested;
				Element.PositionRequested += MediaElementPositionRequested;

				avPlayerViewController.ShowsPlaybackControls = Element.ShowsPlaybackControls;
				avPlayerViewController.VideoGravity = AspectToGravity(Element.Aspect);
				if (Element.KeepScreenOn)
					SetKeepScreenOn(true);

				playedToEndObserver = NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, PlayedToEnd);

				UpdateBackgroundColor();
				UpdateSource();
			}
		}

		protected virtual void UpdateBackgroundColor() => BackgroundColor = Element.BackgroundColor.ToUIColor();
	}
}