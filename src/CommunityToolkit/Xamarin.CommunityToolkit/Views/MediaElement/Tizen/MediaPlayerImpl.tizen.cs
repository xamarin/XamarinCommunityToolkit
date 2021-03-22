using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Tizen.Multimedia;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Tizen;
using MSize = Tizen.Multimedia.Size;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class MediaPlayerImpl : IPlatformMediaPlayer
	{
		bool disposed = false;
		bool cancelToStart;
		DisplayAspectMode aspectMode = DisplayAspectMode.AspectFit;
		Player player;
		Task? taskPrepare;
		TaskCompletionSource<bool>? tcsForStreamInfo;
		IVideoOutput? videoOutput;
		Core.MediaSource? source;

		public MediaPlayerImpl()
		{
			player = new Player();
			player.PlaybackCompleted += OnPlaybackCompleted;
			player.BufferingProgressChanged += OnBufferingProgressChanged;
			player.ErrorOccurred += OnErrorOccurred;
		}

		~MediaPlayerImpl()
		{
			Dispose(false);
		}

		public bool UsesEmbeddingControls { get; set; }

		public bool AutoPlay { get; set; }

		public bool AutoStop { get; set; }

		public double Volume
		{
			get => player.Volume;
			set => player.Volume = (float)value;
		}

		public bool IsMuted
		{
			get => player.Muted;
			set => player.Muted = value;
		}

		public bool IsLooping
		{
			get => player.IsLooping;
			set => player.IsLooping = value;
		}

		public int Duration => player.StreamInfo.GetDuration();

		public int Position
		{
			get
			{
				if (player.State == PlayerState.Idle || player.State == PlayerState.Preparing)
					return 0;
				return player.GetPlayPosition();
			}
		}

		public DisplayAspectMode AspectMode
		{
			get { return aspectMode; }
			set
			{
				aspectMode = value;
				ApplyAspectMode();
			}
		}

		bool HasSource => source != null;

		IVideoOutput? VideoOutput
		{
			get { return videoOutput; }
			set
			{
				if (TargetView != null)
					TargetView.PropertyChanged -= OnTargetViewPropertyChanged;

				videoOutput = value;

				if (TargetView != null)
				{
					TargetView.PropertyChanged += OnTargetViewPropertyChanged;
				}
			}
		}

		VisualElement? TargetView => VideoOutput?.MediaView;

		Task TaskPrepare
		{
			get => taskPrepare ?? Task.CompletedTask;
			set => taskPrepare = value;
		}

		public event EventHandler? UpdateStreamInfo;
		public event EventHandler? PlaybackCompleted;
		public event EventHandler? PlaybackStarted;
		public event EventHandler<BufferingProgressUpdatedEventArgs>? BufferingProgressUpdated;
		public event EventHandler? PlaybackStopped;
		public event EventHandler? PlaybackPaused;
		public event EventHandler? ErrorOccurred;

		public async Task<bool> Start()
		{
			cancelToStart = false;
			if (!HasSource)
				return false;

			if (player.State == PlayerState.Idle)
			{
				await Prepare();
			}

			if (cancelToStart)
				return false;

			try
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					player.Start();
					PlaybackStarted?.Invoke(this, EventArgs.Empty);
				});
			}
			catch (Exception e)
			{
				Log.Error($"Error On Start : {e.Message}");
				return false;
			}
			return true;
		}

		public void Pause()
		{
			try
			{
				player.Pause();
				PlaybackPaused?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception e)
			{
				Log.Error($"Error on Pause : {e.Message}");
			}
		}

		public void Stop()
		{
			cancelToStart = true;
			_ = ChangeToIdleState();
			PlaybackStopped?.Invoke(this, EventArgs.Empty);
		}

		public void SetDisplay(IVideoOutput? output)
		{
			VideoOutput = output;
		}

		public async Task<int> Seek(int ms)
		{
			try
			{
				await player.SetPlayPositionAsync(ms, true);
			}
			catch (Exception e)
			{
				Log.Error($"Fail to seek : {e.Message}");
			}
			return Position;
		}

		public void SetSource(Core.MediaSource source)
		{
			this.source = source;
			if (HasSource && AutoPlay)
			{
				_ = Start();
			}
			else if (!HasSource)
			{
				Stop();
			}
		}

		public async Task<Stream?> GetAlbumArts()
		{
			if (player.State == PlayerState.Idle)
			{
				if (tcsForStreamInfo == null || tcsForStreamInfo.Task.IsCompleted)
				{
					tcsForStreamInfo = new TaskCompletionSource<bool>();
				}
				await tcsForStreamInfo.Task;
			}
			await TaskPrepare;

			var imageData = player.StreamInfo.GetAlbumArt();
			if (imageData == null)
				return null;
			return new MemoryStream(imageData);
		}

		public async Task<IDictionary<string, string>> GetMetadata()
		{
			if (player.State == PlayerState.Idle)
			{
				if (tcsForStreamInfo == null || tcsForStreamInfo.Task.IsCompleted)
				{
					tcsForStreamInfo = new TaskCompletionSource<bool>();
				}
				await tcsForStreamInfo.Task;
			}
			await TaskPrepare;

			var metadata = new Dictionary<string, string>
			{
				[nameof(StreamMetadataKey.Album)] = player.StreamInfo.GetMetadata(StreamMetadataKey.Album),
				[nameof(StreamMetadataKey.Artist)] = player.StreamInfo.GetMetadata(StreamMetadataKey.Artist),
				[nameof(StreamMetadataKey.Author)] = player.StreamInfo.GetMetadata(StreamMetadataKey.Author),
				[nameof(StreamMetadataKey.Genre)] = player.StreamInfo.GetMetadata(StreamMetadataKey.Genre),
				[nameof(StreamMetadataKey.Title)] = player.StreamInfo.GetMetadata(StreamMetadataKey.Title),
				[nameof(StreamMetadataKey.Year)] = player.StreamInfo.GetMetadata(StreamMetadataKey.Year)
			};
			return metadata;
		}

		public async Task<MSize> GetVideoSize()
		{
			if (player.State == PlayerState.Idle)
			{
				if (tcsForStreamInfo == null || tcsForStreamInfo.Task.IsCompleted)
				{
					tcsForStreamInfo = new TaskCompletionSource<bool>();
				}
				await tcsForStreamInfo.Task;
			}
			await TaskPrepare;

			var videoSize = player.StreamInfo.GetVideoProperties().Size;
			return new MSize(videoSize.Width, videoSize.Height);
		}

		public View GetEmbeddingControlView(IMediaPlayer player)
		{
			return new EmbeddingControls
			{
				BindingContext = player
			};
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				player.PlaybackCompleted -= OnPlaybackCompleted;
				player.BufferingProgressChanged -= OnBufferingProgressChanged;
				player.ErrorOccurred -= OnErrorOccurred;
				player.Dispose();
			}

			disposed = true;
		}

		void ApplyDisplay()
		{
			if (VideoOutput == null)
			{
				player.Display = null;
			}
			else
			{
				if (player.Display != null)
					return;

				var renderer = Platform.GetRenderer(TargetView);
				if (renderer is IMediaViewProvider provider && provider.GetMediaView() != null)
				{
					try
					{
						var display = new Display(provider.GetMediaView());
						player.Display = display;
						player.DisplaySettings.Mode = aspectMode.ToNative();
					}
					catch
					{
						Log.Error("Error on MediaView");
					}
				}
			}
		}

		void ApplySource()
		{
			if (source == null)
			{
				return;
			}

			if (source is UriMediaSource uriSource)
			{
				var uri = uriSource.Uri;
				if (uri != null)
					player.SetSource(new MediaUriSource(uri.IsFile ? uri.LocalPath : uri.AbsoluteUri));
			}
			else if (source is FileMediaSource fileSource)
			{
				player.SetSource(new MediaUriSource(ResourcePath.GetPath(fileSource.File)));
			}
		}

		void OnTargetViewPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Renderer")
			{
				if (Platform.GetRenderer(sender as BindableObject) != null && HasSource && AutoPlay)
				{
					Device.BeginInvokeOnMainThread(async () =>
					{
						await Start();
					});
				}
				else if (Platform.GetRenderer(sender as BindableObject) == null && AutoStop)
				{
					Stop();
				}
			}
		}

		async Task Prepare()
		{
			var tcs = new TaskCompletionSource<bool>();
			var prevTask = TaskPrepare;
			TaskPrepare = tcs.Task;
			await prevTask;

			if (player.State == PlayerState.Ready)
				return;

			ApplyDisplay();
			ApplySource();

			try
			{
				await player.PrepareAsync();
				UpdateStreamInfo?.Invoke(this, EventArgs.Empty);
				tcsForStreamInfo?.TrySetResult(true);
			}
			catch (Exception e)
			{
				Log.Error($"Error on prepare : {e.Message}");
				cancelToStart = true;
			}
			tcs.SetResult(true);
		}

		async void ApplyAspectMode()
		{
			if (player.State == PlayerState.Preparing)
			{
				await TaskPrepare;
			}
			player.DisplaySettings.Mode = AspectMode.ToNative();
		}

		void OnBufferingProgressChanged(object sender, BufferingProgressChangedEventArgs e)
		{
			BufferingProgressUpdated?.Invoke(this, new BufferingProgressUpdatedEventArgs { Progress = e.Percent / 100.0 });
		}

		void OnPlaybackCompleted(object sender, EventArgs e)
		{
			PlaybackCompleted?.Invoke(this, EventArgs.Empty);
		}

		void OnErrorOccurred(object sender, PlayerErrorOccurredEventArgs e)
		{
			Log.Error($"Playback Error Occurred (code:{e.Error})-{e.ToString()}");
			ErrorOccurred?.Invoke(this, EventArgs.Empty);
		}

		async Task ChangeToIdleState()
		{
			switch (player.State)
			{
				case PlayerState.Playing:
				case PlayerState.Paused:
					player.Stop();
					player.Unprepare();
					break;
				case PlayerState.Ready:
					player.Unprepare();
					break;
				case PlayerState.Preparing:
					await TaskPrepare;
					player.Unprepare();
					break;
			}
		}
	}
}
