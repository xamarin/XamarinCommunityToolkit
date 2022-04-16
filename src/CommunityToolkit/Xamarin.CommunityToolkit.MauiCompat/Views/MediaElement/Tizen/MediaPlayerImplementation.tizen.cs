using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Tizen.Multimedia;
using Xamarin.CommunityToolkit.Core;
using Xamarin.CommunityToolkit.Helpers;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Tizen;
using MSize = Tizen.Multimedia.Size;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class MediaPlayerImplementation : IPlatformMediaPlayer
	{
		readonly Player player;

		bool disposed = false;
		bool cancelToStart;
		DisplayAspectMode aspectMode = DisplayAspectMode.AspectFit;
		Task? taskPrepare;
		TaskCompletionSource<bool>? tcsForStreamInfo;
		IVideoOutput? videoOutput;
		Core.MediaSource? source;

		public MediaPlayerImplementation()
		{
			player = new Player();
			player.PlaybackCompleted += OnPlaybackCompleted;
			player.BufferingProgressChanged += OnBufferingProgressChanged;
			player.ErrorOccurred += OnErrorOccurred;
		}

		~MediaPlayerImplementation()
		{
			Dispose(false);
		}

		public bool UsesEmbeddingControls { get; set; }

		public bool AutoPlay { get; set; }

		public bool AutoStop { get; set; }

		public float Volume
		{
			get => player.Volume;
			set => player.Volume = value;
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
			get => aspectMode;

			set
			{
				aspectMode = value;
				ApplyAspectMode().SafeFireAndForget();
			}
		}

		bool HasSource => source != null;

		IVideoOutput? VideoOutput
		{
			get => videoOutput;

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
				await Device.InvokeOnMainThreadAsync(() =>
				{
					player.Start();
					PlaybackStarted?.Invoke(this, EventArgs.Empty);
				});
			}
			catch (Exception e)
			{
				(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetService(typeof(ILogger)) as ILogger)?.LogError($"Error On Start : {e.Message}");
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
				(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetService(typeof(ILogger)) as ILogger)?.LogError($"Error on Pause : {e.Message}");
			}
		}

		public async Task Stop()
		{
			cancelToStart = true;
			await ChangeToIdleState();
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
				if (player.State == PlayerState.Preparing || player.State == PlayerState.Idle)
					return 0;
				await player.SetPlayPositionAsync(ms, true);
			}
			catch (Exception e)
			{
				(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetService(typeof(ILogger)) as ILogger)?.LogError($"Failed to seek: \n{e}");
			}
			return Position;
		}

		public async ValueTask SetSource(Core.MediaSource? source)
		{
			this.source = source;

			if (HasSource && AutoPlay)
			{
				await Start();
			}
			else if (!HasSource)
			{
				await Stop();
			}
		}

		public async ValueTask<Stream?> GetAlbumArtsAsync()
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

		public async Task<IReadOnlyDictionary<string, string>> GetMetadata()
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

		public View GetEmbeddingControlView(IMediaPlayer player) => new EmbeddingControls
		{
			BindingContext = player
		};

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
					catch (Exception e)
					{
						(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetService(typeof(ILogger)) as ILogger)?.LogError($"Error on MediaView: \n{e}");
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

		async void OnTargetViewPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Renderer")
			{
				var renderer = Platform.GetRenderer(sender as BindableObject);
				if (renderer != null && HasSource && AutoPlay)
				{
					await Device.InvokeOnMainThreadAsync(Start);
				}
				else if (renderer == null && AutoStop && !disposed)
				{
					await Stop();
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
				(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetService(typeof(ILogger)) as ILogger)?.LogError($"Error on prepare: \n{e}");
				cancelToStart = true;
			}
			tcs.SetResult(true);
		}

		async ValueTask ApplyAspectMode()
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
			(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetService(typeof(ILogger)) as ILogger)?.LogError($"Playback Error Occurred (code:{e.Error})-{e}");
			ErrorOccurred?.Invoke(this, EventArgs.Empty);
		}

		async ValueTask ChangeToIdleState()
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