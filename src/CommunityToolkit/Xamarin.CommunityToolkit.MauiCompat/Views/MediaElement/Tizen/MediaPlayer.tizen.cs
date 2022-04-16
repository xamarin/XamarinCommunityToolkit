using System;using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Tizen;
using Xamarin.CommunityToolkit.Core;
using Xamarin.CommunityToolkit.ObjectModel;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Internals;
using MediaSource = Xamarin.CommunityToolkit.Core.MediaSource;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class MediaPlayer : Element, IMediaPlayer, IDisposable
	{
		public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(MediaSource), typeof(MediaPlayer), default(MediaSource), propertyChanged: OnSourceChanged);

		public static readonly BindableProperty VideoOutputProperty = BindableProperty.Create(nameof(VideoOutput), typeof(IVideoOutput), typeof(MediaPlayer), null, propertyChanging: null, propertyChanged: (b, o, n) => ((MediaPlayer)b).OnVideoOutputChanged());

		public static readonly BindableProperty UsesEmbeddingControlsProperty = BindableProperty.Create(nameof(UsesEmbeddingControls), typeof(bool), typeof(MediaPlayer), true, propertyChanged: async (b, o, n) => await ((MediaPlayer)b).OnUsesEmbeddingControlsChanged());

		public static readonly BindableProperty VolumeProperty = BindableProperty.Create(nameof(Volume), typeof(double), typeof(MediaPlayer), 1d, coerceValue: (bindable, value) => ((double)value).Clamp(0, 1), propertyChanged: (b, o, n) => ((MediaPlayer)b).OnVolumeChanged());

		public static readonly BindableProperty IsMutedProperty = BindableProperty.Create(nameof(IsMuted), typeof(bool), typeof(MediaPlayer), false, propertyChanged: (b, o, n) => ((MediaPlayer)b).UpdateIsMuted());

		public static readonly BindableProperty AspectModeProperty = BindableProperty.Create(nameof(AspectMode), typeof(DisplayAspectMode), typeof(MediaPlayer), DisplayAspectMode.AspectFit, propertyChanged: (b, o, n) => ((MediaPlayer)b).OnAspectModeChanged());

		public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create(nameof(AutoPlay), typeof(bool), typeof(MediaPlayer), false, propertyChanged: (b, o, n) => ((MediaPlayer)b).UpdateAutoPlay());

		public static readonly BindableProperty AutoStopProperty = BindableProperty.Create(nameof(AutoStop), typeof(bool), typeof(MediaPlayer), true, propertyChanged: (b, o, n) => ((MediaPlayer)b).UpdateAutoStop());

		public static readonly BindableProperty IsLoopingProperty = BindableProperty.Create(nameof(IsLooping), typeof(bool), typeof(MediaPlayer), false, propertyChanged: (b, o, n) => ((MediaPlayer)b).UpdateIsLooping());

		static readonly BindablePropertyKey durationPropertyKey = BindableProperty.CreateReadOnly(nameof(Duration), typeof(int), typeof(MediaPlayer), 0);

		public static readonly BindableProperty DurationProperty = durationPropertyKey.BindableProperty;

		static readonly BindablePropertyKey bufferingProgressPropertyKey = BindableProperty.CreateReadOnly(nameof(BufferingProgress), typeof(double), typeof(MediaPlayer), 0d);

		public static readonly BindableProperty BufferingProgressProperty = bufferingProgressPropertyKey.BindableProperty;

		static readonly BindablePropertyKey positionPropertyKey = BindableProperty.CreateReadOnly(nameof(Position), typeof(int), typeof(MediaPlayer), 0);

		public static readonly BindableProperty PositionProperty = positionPropertyKey.BindableProperty;

		static readonly BindablePropertyKey statePropertyKey = BindableProperty.CreateReadOnly(nameof(State), typeof(PlaybackState), typeof(MediaPlayer), PlaybackState.Stopped);

		public static readonly BindableProperty StateProperty = statePropertyKey.BindableProperty;

		public static readonly BindableProperty PositionUpdateIntervalProperty = BindableProperty.Create(nameof(PositionUpdateInterval), typeof(int), typeof(MediaPlayer), 200);

		static readonly BindablePropertyKey isBufferingPropertyKey = BindableProperty.CreateReadOnly(nameof(IsBuffering), typeof(bool), typeof(MediaPlayer), false);

		public static readonly BindableProperty IsBufferingProperty = isBufferingPropertyKey.BindableProperty;

		readonly IPlatformMediaPlayer mediaPlayer;
		readonly Lazy<View> controls;

		bool disposed = false;
		bool isDisposing = false;
		bool isPlaying;
		bool controlsAlwaysVisible;
		CancellationTokenSource hideTimerCTS = new CancellationTokenSource();

		public MediaPlayer()
		{
			StartCommand = new AsyncValueCommand(async () =>
			{
				if (State == PlaybackState.Playing)
				{
					Pause();
				}
				else
				{
					await Start();
				}
			}, allowsMultipleExecutions: false);

			FastForwardCommand = new AsyncCommand(async () =>
			{
				if (State != PlaybackState.Stopped)
				{
					await Seek(Math.Min(Position + 5000, Duration));
				}
			}, _ => State != PlaybackState.Stopped, allowsMultipleExecutions: false);

			RewindCommand = new AsyncCommand(async () =>
			{
				if (State != PlaybackState.Stopped)
				{
					await Seek(Math.Max(Position - 5000, 0));
				}
			}, _ => State != PlaybackState.Stopped, allowsMultipleExecutions: false);

			mediaPlayer = new MediaPlayerImplementation();
			mediaPlayer.UpdateStreamInfo += OnUpdateStreamInfo;
			mediaPlayer.PlaybackCompleted += SendPlaybackCompleted;
			mediaPlayer.PlaybackStarted += SendPlaybackStarted;
			mediaPlayer.PlaybackPaused += SendPlaybackPaused;
			mediaPlayer.PlaybackStopped += SendPlaybackStopped;
			mediaPlayer.BufferingProgressUpdated += OnUpdateBufferingProgress;
			mediaPlayer.ErrorOccurred += OnErrorOccurred;
			mediaPlayer.UsesEmbeddingControls = true;
			mediaPlayer.Volume = 1f;
			mediaPlayer.AspectMode = DisplayAspectMode.AspectFit;
			mediaPlayer.AutoPlay = false;
			mediaPlayer.AutoStop = true;

			controlsAlwaysVisible = false;
			controls = new Lazy<View>(() => mediaPlayer.GetEmbeddingControlView(this));
		}

		~MediaPlayer()
		{
			Dispose(false);
		}

		public DisplayAspectMode AspectMode
		{
			get => (DisplayAspectMode)GetValue(AspectModeProperty);
			set => SetValue(AspectModeProperty, value);
		}

		public bool AutoPlay
		{
			get => (bool)GetValue(AutoPlayProperty);
			set => SetValue(AutoPlayProperty, value);
		}

		public bool AutoStop
		{
			get => (bool)GetValue(AutoStopProperty);
			set => SetValue(AutoStopProperty, value);
		}

		public bool IsLooping
		{
			get => (bool)GetValue(IsLoopingProperty);
			set => SetValue(IsLoopingProperty, value);
		}

		public double BufferingProgress
		{
			get => (double)GetValue(BufferingProgressProperty);
			private set => SetValue(bufferingProgressPropertyKey, value);
		}

		public int Duration
		{
			get => (int)GetValue(DurationProperty);
			private set => SetValue(durationPropertyKey, value);
		}

		[System.ComponentModel.TypeConverter(typeof(MediaSourceConverter))]
		public MediaSource? Source
		{
			get => (MediaSource?)GetValue(SourceProperty);
			set => SetValue(SourceProperty, value);
		}

		public IVideoOutput? VideoOutput
		{
			get => (IVideoOutput?)GetValue(VideoOutputProperty);
			set => SetValue(VideoOutputProperty, value);
		}

		public double Volume
		{
			get => (double)GetValue(VolumeProperty);
			set => SetValue(VolumeProperty, value);
		}

		public bool IsMuted
		{
			get => (bool)GetValue(IsMutedProperty);
			set => SetValue(IsMutedProperty, value);
		}

		public int PositionUpdateInterval
		{
			get => (int)GetValue(PositionUpdateIntervalProperty);
			set => SetValue(PositionUpdateIntervalProperty, value);
		}

		public bool UsesEmbeddingControls
		{
			get => (bool)GetValue(UsesEmbeddingControlsProperty);
			set
			{
				SetValue(UsesEmbeddingControlsProperty, value);
				mediaPlayer.UsesEmbeddingControls = value;
			}
		}

		public int Position
		{
			get => mediaPlayer.Position;
			private set
			{
				SetValue(positionPropertyKey, value);
				OnPropertyChanged(nameof(Progress));
			}
		}

		public PlaybackState State
		{
			get => (PlaybackState)GetValue(StateProperty);
			private set => SetValue(statePropertyKey, value);
		}

		public bool IsBuffering
		{
			get => (bool)GetValue(IsBufferingProperty);
			private set => SetValue(isBufferingPropertyKey, value);
		}

		public double Progress => Position / (double)Math.Max(Position, Duration);

		public ICommand StartCommand { get; }

		public ICommand FastForwardCommand { get; }

		public ICommand RewindCommand { get; }

		public event EventHandler? PlaybackCompleted;

		public event EventHandler? PlaybackStarted;

		public event EventHandler? PlaybackPaused;

		public event EventHandler? PlaybackStopped;

		public event EventHandler<BufferingProgressUpdatedEventArgs>? BufferingProgressUpdated;

		public event EventHandler? BufferingStarted;

		public event EventHandler? BufferingCompleted;

		public event EventHandler? ErrorOccurred;

		public event EventHandler? MediaPrepared;

		public void Pause() => mediaPlayer.Pause();

		public async Task<int> Seek(int ms)
		{
			var finalPosition = await mediaPlayer.Seek(ms);
			Position = mediaPlayer.Position;

			return finalPosition;
		}

		public Task<bool> Start() => mediaPlayer.Start();

		public Task Stop() => mediaPlayer.Stop();

		public Task<global::Tizen.Multimedia.Size> GetVideoSize() => mediaPlayer.GetVideoSize();

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			isDisposing = disposing;

			if (disposing)
			{
				isPlaying = false;
				mediaPlayer.UpdateStreamInfo -= OnUpdateStreamInfo;
				mediaPlayer.PlaybackCompleted -= SendPlaybackCompleted;
				mediaPlayer.PlaybackStarted -= SendPlaybackStarted;
				mediaPlayer.PlaybackPaused -= SendPlaybackPaused;
				mediaPlayer.PlaybackStopped -= SendPlaybackStopped;
				mediaPlayer.BufferingProgressUpdated -= OnUpdateBufferingProgress;
				mediaPlayer.ErrorOccurred -= OnErrorOccurred;
				mediaPlayer.Dispose();
			}

			disposed = true;
		}

		void UpdateAutoPlay()
		{
			mediaPlayer.AutoPlay = AutoPlay;
		}

		void UpdateAutoStop()
		{
			mediaPlayer.AutoStop = AutoStop;
		}

		void UpdateIsMuted()
		{
			mediaPlayer.IsMuted = IsMuted;
		}

		void UpdateIsLooping()
		{
			mediaPlayer.IsLooping = IsLooping;
		}

		void OnUpdateStreamInfo(object sender, EventArgs e)
		{
			Duration = mediaPlayer.Duration;
			MediaPrepared?.Invoke(this, EventArgs.Empty);
		}

		void SendPlaybackCompleted(object sender, EventArgs e) => PlaybackCompleted?.Invoke(this, EventArgs.Empty);

		async void SendPlaybackStarted(object sender, EventArgs e)
		{
			isPlaying = true;
			State = PlaybackState.Playing;
			StartPostionPollingTimer();
			PlaybackStarted?.Invoke(this, EventArgs.Empty);
			controlsAlwaysVisible = false;
			await ShowController();
		}

		async void SendPlaybackPaused(object sender, EventArgs e)
		{
			isPlaying = false;
			State = PlaybackState.Paused;
			PlaybackPaused?.Invoke(this, EventArgs.Empty);
			controlsAlwaysVisible = true;
			await ShowController();
		}

		async void SendPlaybackStopped(object sender, EventArgs e)
		{
			isPlaying = false;
			State = PlaybackState.Stopped;
			Position = 0;
			PlaybackStopped?.Invoke(this, EventArgs.Empty);
			controlsAlwaysVisible = true;
			await ShowController();
		}

		void StartPostionPollingTimer()
		{
			Device.StartTimer(TimeSpan.FromMilliseconds(PositionUpdateInterval), () =>
			{
				if (isDisposing)
				{
					return false;
				}
				Position = mediaPlayer.Position;
				return isPlaying;
			});
		}

		void OnSourceChanged(object sender, EventArgs e)
		{
			if (Source != null)
				mediaPlayer.SetSource(Source);
		}

		void OnVideoOutputChanged()
		{
			if (VideoOutput != null)
			{
				if (UsesEmbeddingControls)
				{
					VideoOutput.Controller = controls.Value;
				}
				VideoOutput.MediaView.Focused += OnVideoOutputFocused;
				if (VideoOutput.MediaView is View outputView)
				{
					var tapGesture = new TapGestureRecognizer();
					tapGesture.Tapped += OnOutputTapped;
					outputView.GestureRecognizers.Add(tapGesture);
				}
			}
			mediaPlayer.SetDisplay(VideoOutput);
		}

		async void OnOutputTapped(object sender, EventArgs e)
		{
			if (!UsesEmbeddingControls)
				return;
			if (!controls.Value.IsVisible)
			{
				await ShowController();
			}
		}

		async Task OnUsesEmbeddingControlsChanged()
		{
			if (UsesEmbeddingControls)
			{
				if (VideoOutput != null)
				{
					VideoOutput.Controller = controls.Value;
					await ShowController();
				}
			}
			else
			{
				if (VideoOutput != null)
				{
					await HideController(0);
					await Task.Delay(200);
					VideoOutput.Controller = null;
				}
			}
		}

		async void OnVideoOutputFocused(object sender, FocusEventArgs e)
		{
			if (UsesEmbeddingControls)
			{
				await ShowController();
			}
		}

		void OnVolumeChanged()
		{
			mediaPlayer.Volume = (float)Volume;
		}

		void OnAspectModeChanged()
		{
			mediaPlayer.AspectMode = AspectMode;
		}

		void OnUpdateBufferingProgress(object sender, BufferingProgressUpdatedEventArgs e)
		{
			if (!IsBuffering && e.Progress >= 0)
			{
				IsBuffering = true;
				BufferingStarted?.Invoke(this, EventArgs.Empty);
			}
			else if (IsBuffering && e.Progress == 1.0)
			{
				IsBuffering = false;
				BufferingCompleted?.Invoke(this, EventArgs.Empty);
			}

			BufferingProgress = e.Progress;
			BufferingProgressUpdated?.Invoke(this, new BufferingProgressUpdatedEventArgs { Progress = BufferingProgress });
		}

		void OnErrorOccurred(object sender, EventArgs e) => ErrorOccurred?.Invoke(this, EventArgs.Empty);

		async Task HideController(int after)
		{
			if (!controls.IsValueCreated)
				return;

			hideTimerCTS?.Cancel();
			hideTimerCTS?.Dispose();
			hideTimerCTS = new CancellationTokenSource();
			try
			{
				await Task.Delay(after, hideTimerCTS.Token);

				if (!controlsAlwaysVisible)
				{
					await controls.Value.FadeTo(0, 200);
					controls.Value.IsVisible = false;
				}
			}
			catch (Exception e)
			{
				Forms.Platform.Tizen.(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetService(typeof(ILogger)) as ILogger)?.LogError($"HideController failed: \n{e}");
			}
		}

		async ValueTask ShowController()
		{
			if (controls.IsValueCreated)
			{
				controls.Value.IsVisible = true;
				await controls.Value.FadeTo(1.0, 200);
				await HideController(5000);
			}
		}

		static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var mediaPlayer = (MediaPlayer)bindable;
			mediaPlayer.OnSourceChanged(bindable, EventArgs.Empty);
		}
	}
}