using System;
using System.IO;
using AppKit;
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.MacOS;
using ToolKitMediaElement = Xamarin.CommunityToolkit.UI.Views.MediaElement;
using ToolKitMediaElementRenderer = Xamarin.CommunityToolkit.UI.Views.MediaElementRenderer;
using XCT = Xamarin.CommunityToolkit.Core;

[assembly: ExportRenderer(typeof(ToolKitMediaElement), typeof(ToolKitMediaElementRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class MediaElementRenderer : ViewRenderer<ToolKitMediaElement, NSView>
	{
		IMediaElementController Controller => Element;

		protected readonly AVPlayerView avPlayerView = new();
		protected NSObject? playedToEndObserver;
		protected IDisposable? statusObserver;
		protected IDisposable? rateObserver;
		protected IDisposable? volumeObserver;
		bool idleTimerDisabled = false;
		AVPlayerItem? playerItem;
		AVPlayerLayer? playerLayer;

		public MediaElementRenderer() => AddPlayedToEndObserver();

		protected virtual void SetKeepScreenOn(bool value)
		{
			if (avPlayerView.Player != null)
			{
				if (value)
				{
					if (!avPlayerView.Player.PreventsDisplaySleepDuringVideoPlayback)
					{
						idleTimerDisabled = true;
						avPlayerView.Player.PreventsDisplaySleepDuringVideoPlayback = true;
					}
				}
				else if (idleTimerDisabled)
				{
					idleTimerDisabled = false;
					avPlayerView.Player.PreventsDisplaySleepDuringVideoPlayback = false;
				}
			}
		}

		protected virtual void UpdateSource()
		{
			if (Element.Source != null)
			{
				AVAsset? asset = null;

				if (Element.Source is XCT.UriMediaSource uriSource)
				{
					if (uriSource.Uri?.Scheme is "ms-appx")
					{
						if (uriSource.Uri.LocalPath.Length <= 1)
							return;

						// used for a file embedded in the application package
						asset = AVAsset.FromUrl(NSUrl.FromFilename(uriSource.Uri.LocalPath.Substring(1)));
					}
					else if (uriSource.Uri?.Scheme == "ms-appdata")
					{
						var filePath = ResolveMsAppDataUri(uriSource.Uri);

						if (string.IsNullOrEmpty(filePath))
							throw new ArgumentException("Invalid Uri", "Source");

						asset = AVAsset.FromUrl(NSUrl.FromFilename(filePath));
					}
					else if (uriSource.Uri != null)
					{
						asset = AVUrlAsset.Create(NSUrl.FromString(uriSource.Uri.AbsoluteUri));
					}
					else
					{
						throw new InvalidOperationException($"{nameof(uriSource.Uri)} is not initialized");
					}
				}
				else
				{
					if (Element.Source is XCT.FileMediaSource fileSource)
						asset = AVAsset.FromUrl(NSUrl.FromFilename(fileSource.File));
				}

				_ = asset ?? throw new NullReferenceException();

				playerItem = new AVPlayerItem(asset);
				AddStatusObserver();

				if (avPlayerView.Player != null)
					avPlayerView.Player.ReplaceCurrentItemWithPlayerItem(playerItem);
				else
				{
					avPlayerView.Player = new AVPlayer(playerItem);
					AddRateObserver();
					AddVolumeObserver();
				}

				UpdateVolume();

				if (Element.AutoPlay)
					Play();
			}
			else
			{
				avPlayerView.Player?.Pause();
				avPlayerView.Player?.ReplaceCurrentItemWithPlayerItem(null);
				DestroyStatusObserver();
				Controller.CurrentState = MediaElementState.Stopped;
			}
		}

		protected string ResolveMsAppDataUri(Uri uri)
		{
			if (uri.Scheme is "ms-appdata")
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

		protected virtual void ObserveRate(NSObservedChange e)
		{
			if (Controller is object)
			{
				switch (avPlayerView.Player?.Rate)
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
			if (Controller == null || avPlayerView.Player == null)
				return;

			Controller.Volume = avPlayerView.Player.Volume;
		}

		protected void ObserveStatus(NSObservedChange e)
		{
			_ = avPlayerView.Player?.CurrentItem ?? throw new NullReferenceException();
			Controller.Volume = avPlayerView.Player.Volume;

			switch (avPlayerView.Player.Status)
			{
				case AVPlayerStatus.Failed:
					Controller.OnMediaFailed();
					break;

				case AVPlayerStatus.ReadyToPlay:
					var duration = avPlayerView.Player.CurrentItem.Duration;

					if (duration.IsIndefinite)
						Controller.Duration = TimeSpan.Zero;
					else
						Controller.Duration = TimeSpan.FromSeconds(duration.Seconds);

					Controller.VideoHeight = (int)avPlayerView.Player.CurrentItem.Asset.NaturalSize.Height;
					Controller.VideoWidth = (int)avPlayerView.Player.CurrentItem.Asset.NaturalSize.Width;
					Controller.OnMediaOpened();
					Controller.Position = Position;
					break;
			}
		}

		TimeSpan Position
		{
			get
			{
				if (avPlayerView?.Player?.CurrentTime.IsInvalid ?? true)
					return TimeSpan.Zero;

				return TimeSpan.FromSeconds(avPlayerView.Player.CurrentTime.Seconds);
			}
		}

		void PlayedToEnd(NSNotification notification)
		{
			if (Element == null || notification.Object != avPlayerView.Player?.CurrentItem)
				return;

			if (Element.IsLooping)
			{
				avPlayerView.Player?.Seek(CMTime.Zero);
				Controller.Position = Position;
				avPlayerView.Player?.Play();
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

		protected override void OnElementPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(ToolKitMediaElement.Aspect):
					if (playerLayer != null)
					{
						playerLayer.VideoGravity = AspectToGravity(Element.Aspect);
					}
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
					avPlayerView.ShowsFullScreenToggleButton = Element.ShowsPlaybackControls;
					break;

				case nameof(ToolKitMediaElement.Source):
					UpdateSource();
					break;

				case nameof(ToolKitMediaElement.Volume):
					UpdateVolume();
					break;
			}
		}

		void MediaElementSeekRequested(object? sender, SeekRequested e)
		{
			if (avPlayerView.Player?.CurrentItem == null || avPlayerView.Player.Status != AVPlayerStatus.ReadyToPlay)
				return;

			var ranges = avPlayerView.Player.CurrentItem.SeekableTimeRanges;
			var seekTo = new CMTime(Convert.ToInt64(e.Position.TotalMilliseconds), 1000);
			foreach (var v in ranges)
			{
				if (seekTo >= v.CMTimeRangeValue.Start && seekTo < (v.CMTimeRangeValue.Start + v.CMTimeRangeValue.Duration))
				{
					avPlayerView.Player.Seek(seekTo, SeekComplete);
					break;
				}
			}
		}

		protected virtual void Play()
		{
			if (avPlayerView.Player != null)
			{
				avPlayerView.Player.Play();
				Controller.CurrentState = MediaElementState.Playing;
			}

			if (Element.KeepScreenOn)
				SetKeepScreenOn(true);
		}

		void UpdateVolume()
		{
			if (avPlayerView.Player != null)
				avPlayerView.Player.Volume = (float)Element.Volume;
		}

		void MediaElementStateRequested(object? sender, StateRequested e)
		{
			switch (e.State)
			{
				case MediaElementState.Playing:
					Play();
					break;

				case MediaElementState.Paused:
					if (Element.KeepScreenOn)
						SetKeepScreenOn(false);

					if (avPlayerView.Player != null)
					{
						avPlayerView.Player.Pause();
						Controller.CurrentState = MediaElementState.Paused;
					}
					break;

				case MediaElementState.Stopped:
					if (Element.KeepScreenOn)
						SetKeepScreenOn(false);

					avPlayerView.Player?.Pause();
					avPlayerView.Player?.Seek(CMTime.Zero);
					Controller.CurrentState = MediaElementState.Stopped;
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
				Controller?.OnSeekCompleted();
		}

		void MediaElementPositionRequested(object? sender, EventArgs e) => Controller.Position = Position;

		protected override void OnElementChanged(ElementChangedEventArgs<MediaElement> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;
				e.OldElement.SeekRequested -= MediaElementSeekRequested;
				e.OldElement.StateRequested -= MediaElementStateRequested;
				e.OldElement.PositionRequested -= MediaElementPositionRequested;
				SetKeepScreenOn(false);

				// stop video if playing
				if (avPlayerView?.Player?.CurrentItem != null)
				{
					if (avPlayerView?.Player?.Rate > 0)
						avPlayerView?.Player?.Pause();

					avPlayerView?.Player?.ReplaceCurrentItemWithPlayerItem(null);
				}

				playerLayer?.Dispose();
				avPlayerView?.Dispose();

				DestroyPlayedToEndObserver();
				DestroyRateObserver();
				DestroyVolumeObserver();
				DestroyStatusObserver();
			}

			if (e.NewElement != null)
			{
				SetNativeControl(avPlayerView ?? throw new NullReferenceException());

				playerLayer = AVPlayerLayer.FromPlayer(avPlayerView.Player);
				avPlayerView.Layer = playerLayer;
				playerLayer.VideoGravity = AspectToGravity(Element.Aspect);

				Element.PropertyChanged += OnElementPropertyChanged;
				Element.SeekRequested += MediaElementSeekRequested;
				Element.StateRequested += MediaElementStateRequested;
				Element.PositionRequested += MediaElementPositionRequested;

				if (Element.KeepScreenOn)
					SetKeepScreenOn(true);

				AddPlayedToEndObserver();
				UpdateSource();
			}
		}

		protected void DisposeObservers(ref IDisposable? disposable)
		{
			disposable?.Dispose();
			disposable = null;
		}

		protected void DisposeObservers(ref NSObject? disposable)
		{
			disposable?.Dispose();
			disposable = null;
		}

		void AddVolumeObserver()
		{
			DestroyVolumeObserver();
			volumeObserver = avPlayerView.Player?.AddObserver("volume", NSKeyValueObservingOptions.New,
					ObserveVolume);
		}

		void AddRateObserver()
		{
			DestroyRateObserver();
			rateObserver = avPlayerView.Player?.AddObserver("rate", NSKeyValueObservingOptions.New,
					ObserveRate);
		}

		void AddStatusObserver()
		{
			DestroyStatusObserver();
			statusObserver = playerItem?.AddObserver("status", NSKeyValueObservingOptions.New, ObserveStatus);
		}

		void AddPlayedToEndObserver()
		{
			DestroyPlayedToEndObserver();
			playedToEndObserver =
				NSNotificationCenter.DefaultCenter.AddObserver(AVPlayerItem.DidPlayToEndTimeNotification, PlayedToEnd);
		}

		void DestroyVolumeObserver() => DisposeObservers(ref volumeObserver);

		void DestroyRateObserver() => DisposeObservers(ref rateObserver);

		void DestroyStatusObserver() => DisposeObservers(ref statusObserver);

		void DestroyPlayedToEndObserver()
		{
			if (playedToEndObserver == null)
			{
				return;
			}

			NSNotificationCenter.DefaultCenter.RemoveObserver(playedToEndObserver);
			DisposeObservers(ref playedToEndObserver);
		}
	}
}