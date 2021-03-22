using System;
using System.Threading;
using System.Threading.Tasks;
using Tizen.Multimedia;
using Xamarin.CommunityToolkit.Core;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using MediaSource = Xamarin.CommunityToolkit.Core.MediaSource;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class MediaPlayer : Element, IMediaPlayer, IDisposable
	{
		public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(MediaSource), typeof(MediaPlayer), default(MediaSource), propertyChanged: OnSourceChanged);

		public static readonly BindableProperty VideoOutputProperty = BindableProperty.Create(nameof(VideoOutput), typeof(IVideoOutput), typeof(MediaPlayer), null, propertyChanging: null, propertyChanged: (b, o, n) => ((MediaPlayer)b).OnVideoOutputChanged());

		public static readonly BindableProperty UsesEmbeddingControlsProperty = BindableProperty.Create(nameof(UsesEmbeddingControls), typeof(bool), typeof(MediaPlayer), true, propertyChanged: (b, o, n) => ((MediaPlayer)b).OnUsesEmbeddingControlsChanged());

		public static readonly BindableProperty VolumeProperty = BindableProperty.Create(nameof(Volume), typeof(double), typeof(MediaPlayer), 1d, coerceValue: (bindable, value) => ((double)value).Clamp(0, 1), propertyChanged: (b, o, n) => ((MediaPlayer)b).OnVolumeChanged());

		public static readonly BindableProperty IsMutedProperty = BindableProperty.Create(nameof(IsMuted), typeof(bool), typeof(MediaPlayer), false, propertyChanged: (b, o, n) => ((MediaPlayer)b).UpdateIsMuted());

		public static readonly BindableProperty AspectModeProperty = BindableProperty.Create(nameof(AspectMode), typeof(DisplayAspectMode), typeof(MediaPlayer), DisplayAspectMode.AspectFit, propertyChanged: (b, o, n) => ((MediaPlayer)b).OnAspectModeChanged());

		public static readonly BindableProperty AutoPlayProperty = BindableProperty.Create(nameof(AutoPlay), typeof(bool), typeof(MediaPlayer), false, propertyChanged: (b, o, n) => ((MediaPlayer)b).UpdateAutoPlay());

		public static readonly BindableProperty AutoStopProperty = BindableProperty.Create(nameof(AutoStop), typeof(bool), typeof(MediaPlayer), true, propertyChanged: (b, o, n) => ((MediaPlayer)b).UpdateAutoStop());

		public static readonly BindableProperty IsLoopingProperty = BindableProperty.Create(nameof(IsLooping), typeof(bool), typeof(MediaPlayer), false, propertyChanged: (b, o, n) => ((MediaPlayer)b).UpdateIsLooping());

		static readonly BindablePropertyKey DurationPropertyKey = BindableProperty.CreateReadOnly(nameof(Duration), typeof(int), typeof(MediaPlayer), 0);

		public static readonly BindableProperty DurationProperty = DurationPropertyKey.BindableProperty;

		static readonly BindablePropertyKey BufferingProgressPropertyKey = BindableProperty.CreateReadOnly(nameof(BufferingProgress), typeof(double), typeof(MediaPlayer), 0d);

		public static readonly BindableProperty BufferingProgressProperty = BufferingProgressPropertyKey.BindableProperty;

		static readonly BindablePropertyKey PositionPropertyKey = BindableProperty.CreateReadOnly(nameof(Position), typeof(int), typeof(MediaPlayer), 0);

		public static readonly BindableProperty PositionProperty = PositionPropertyKey.BindableProperty;

		static readonly BindablePropertyKey StatePropertyKey = BindableProperty.CreateReadOnly(nameof(State), typeof(PlaybackState), typeof(MediaPlayer), PlaybackState.Stopped);

		public static readonly BindableProperty StateProperty = StatePropertyKey.BindableProperty;

		public static readonly BindableProperty PositionUpdateIntervalProperty = BindableProperty.Create(nameof(PositionUpdateInterval), typeof(int), typeof(MediaPlayer), 200);

		static readonly BindablePropertyKey IsBufferingPropertyKey = BindableProperty.CreateReadOnly(nameof(IsBuffering), typeof(bool), typeof(MediaPlayer), false);

		public static readonly BindableProperty IsBufferingProperty = IsBufferingPropertyKey.BindableProperty;

		bool disposed = false;
		bool isDisposing = false;
		bool isPlaying;
		bool controlsAlwaysVisible;
		IPlatformMediaPlayer impl;
		CancellationTokenSource hideTimerCTS = new CancellationTokenSource();
		Lazy<View> controls;

		public MediaPlayer()
		{
			impl = new MediaPlayerImpl();
			impl.UpdateStreamInfo += OnUpdateStreamInfo;
			impl.PlaybackCompleted += SendPlaybackCompleted;
			impl.PlaybackStarted += SendPlaybackStarted;
			impl.PlaybackPaused += SendPlaybackPaused;
			impl.PlaybackStopped += SendPlaybackStopped;
			impl.BufferingProgressUpdated += OnUpdateBufferingProgress;
			impl.ErrorOccurred += OnErrorOccurred;
			impl.UsesEmbeddingControls = true;
			impl.Volume = 1d;
			impl.AspectMode = DisplayAspectMode.AspectFit;
			impl.AutoPlay = false;
			impl.AutoStop = true;

			controlsAlwaysVisible = false;
			controls = new Lazy<View>(() =>
			{
				return impl.GetEmbeddingControlView(this);
			});
		}

		~MediaPlayer()
		{
			Dispose(false);
		}

		public DisplayAspectMode AspectMode
		{
			get { return (DisplayAspectMode)GetValue(AspectModeProperty); }
			set { SetValue(AspectModeProperty, value); }
		}

		public bool AutoPlay
		{
			get
			{
				return (bool)GetValue(AutoPlayProperty);
			}
			set
			{
				SetValue(AutoPlayProperty, value);
			}
		}

		public bool AutoStop
		{
			get
			{
				return (bool)GetValue(AutoStopProperty);
			}
			set
			{
				SetValue(AutoStopProperty, value);
			}
		}

		public bool IsLooping
		{
			get
			{
				return (bool)GetValue(IsLoopingProperty);
			}
			set
			{
				SetValue(IsLoopingProperty, value);
			}
		}

		public double BufferingProgress
		{
			get
			{
				return (double)GetValue(BufferingProgressProperty);
			}
			private set
			{
				SetValue(BufferingProgressPropertyKey, value);
			}
		}

		public int Duration
		{
			get
			{
				return (int)GetValue(DurationProperty);
			}
			private set
			{
				SetValue(DurationPropertyKey, value);
			}
		}

		[Xamarin.Forms.TypeConverter(typeof(MediaSourceConverter))]
		public MediaSource Source
		{
			get { return (MediaSource)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		public IVideoOutput VideoOutput
		{
			get { return (IVideoOutput)GetValue(VideoOutputProperty); }
			set { SetValue(VideoOutputProperty, value); }
		}

		public double Volume
		{
			get { return (double)GetValue(VolumeProperty); }
			set { SetValue(VolumeProperty, value); }
		}

		public bool IsMuted
		{
			get { return (bool)GetValue(IsMutedProperty); }
			set { SetValue(IsMutedProperty, value); }
		}

		public int PositionUpdateInterval
		{
			get { return (int)GetValue(PositionUpdateIntervalProperty); }
			set { SetValue(PositionUpdateIntervalProperty, value); }
		}

		public bool UsesEmbeddingControls
		{
			get
			{
				return (bool)GetValue(UsesEmbeddingControlsProperty);
			}
			set
			{
				SetValue(UsesEmbeddingControlsProperty, value);
				impl.UsesEmbeddingControls = value;
			}
		}

		public int Position
		{
			get
			{
				return impl.Position;
			}
			private set
			{
				SetValue(PositionPropertyKey, value);
				OnPropertyChanged(nameof(Progress));
			}
		}

		public PlaybackState State
		{
			get
			{
				return (PlaybackState)GetValue(StateProperty);
			}
			private set
			{
				SetValue(StatePropertyKey, value);
			}
		}

		public bool IsBuffering
		{
			get
			{
				return (bool)GetValue(IsBufferingProperty);
			}
			private set
			{
				SetValue(IsBufferingPropertyKey, value);
			}
		}

		public double Progress
		{
			get
			{
				return Position / (double)Math.Max(Position, Duration);
			}
		}

		public Command StartCommand => new Command(() =>
		{
			if (State == PlaybackState.Playing)
			{
				Pause();
			}
			else
			{
				Start();
			}
		});

		public Command FastForwardCommand => new Command(() =>
		{
			if (State != PlaybackState.Stopped)
			{
				Seek(Math.Min(Position + 5000, Duration));
			}
		}, () => State != PlaybackState.Stopped);

		public Command RewindCommand => new Command(() =>
		{
			if (State != PlaybackState.Stopped)
			{
				Seek(Math.Max(Position - 5000, 0));
			}
		}, () => State != PlaybackState.Stopped);

		public event EventHandler? PlaybackCompleted;
		public event EventHandler? PlaybackStarted;
		public event EventHandler? PlaybackPaused;
		public event EventHandler? PlaybackStopped;
		public event EventHandler<BufferingProgressUpdatedEventArgs>? BufferingProgressUpdated;
		public event EventHandler? BufferingStarted;
		public event EventHandler? BufferingCompleted;
		public event EventHandler? ErrorOccurred;
		public event EventHandler? MediaPrepared;

		public void Pause()
		{
			impl.Pause();
		}

		public Task<int> Seek(int ms)
		{
			ShowController();
			var task = impl.Seek(ms).ContinueWith((t) => Position = impl.Position);

			return task;
		}

		public Task<bool> Start()
		{
			return impl.Start();
		}

		public void Stop()
		{
			impl.Stop();
		}

		public Task<global::Tizen.Multimedia.Size> GetVideoSize()
		{
			return impl.GetVideoSize();
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

			isDisposing = disposing;

			if (disposing)
			{
				isPlaying = false;
				impl.UpdateStreamInfo -= OnUpdateStreamInfo;
				impl.PlaybackCompleted -= SendPlaybackCompleted;
				impl.PlaybackStarted -= SendPlaybackStarted;
				impl.PlaybackPaused -= SendPlaybackPaused;
				impl.PlaybackStopped -= SendPlaybackStopped;
				impl.BufferingProgressUpdated -= OnUpdateBufferingProgress;
				impl.ErrorOccurred -= OnErrorOccurred;
				impl.Dispose();
			}

			disposed = true;
		}

		void UpdateAutoPlay()
		{
			impl.AutoPlay = AutoPlay;
		}

		void UpdateAutoStop()
		{
			impl.AutoStop = AutoStop;
		}

		void UpdateIsMuted()
		{
			impl.IsMuted = IsMuted;
		}

		void UpdateIsLooping()
		{
			impl.IsLooping = IsLooping;
		}

		void OnUpdateStreamInfo(object sender, EventArgs e)
		{
			Duration = impl.Duration;
			MediaPrepared?.Invoke(this, EventArgs.Empty);
		}

		void SendPlaybackCompleted(object sender, EventArgs e)
		{
			PlaybackCompleted?.Invoke(this, EventArgs.Empty);
		}

		void SendPlaybackStarted(object sender, EventArgs e)
		{
			isPlaying = true;
			State = PlaybackState.Playing;
			StartPostionPollingTimer();
			PlaybackStarted?.Invoke(this, EventArgs.Empty);
			controlsAlwaysVisible = false;
			ShowController();
		}

		void SendPlaybackPaused(object sender, EventArgs e)
		{
			isPlaying = false;
			State = PlaybackState.Paused;
			PlaybackPaused?.Invoke(this, EventArgs.Empty);
			controlsAlwaysVisible = true;
			ShowController();
		}

		void SendPlaybackStopped(object sender, EventArgs e)
		{
			isPlaying = false;
			State = PlaybackState.Stopped;
			Position = 0;
			PlaybackStopped?.Invoke(this, EventArgs.Empty);
			controlsAlwaysVisible = true;
			ShowController();
		}

		void StartPostionPollingTimer()
		{
			Device.StartTimer(TimeSpan.FromMilliseconds(PositionUpdateInterval), () =>
			{
				if (isDisposing)
				{
					return false;
				}
				Position = impl.Position;
				return isPlaying;
			});
		}

		void OnSourceChanged(object sender, EventArgs e)
		{
			impl.SetSource(Source);
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
			impl.SetDisplay(VideoOutput);
		}

		void OnOutputTapped(object sender, EventArgs e)
		{
			if (!UsesEmbeddingControls)
				return;
			if (!controls.Value.IsVisible)
			{
				ShowController();
			}
		}

		async void OnUsesEmbeddingControlsChanged()
		{
			if (UsesEmbeddingControls)
			{
				if (VideoOutput != null)
				{
					VideoOutput.Controller = controls.Value;
					ShowController();
				}
			}
			else
			{
				if (VideoOutput != null)
				{
					HideController(0);
					await Task.Delay(200);
					VideoOutput.Controller = null;
				}
			}
		}

		void OnVideoOutputFocused(object sender, FocusEventArgs e)
		{
			if (UsesEmbeddingControls)
			{
				ShowController();
			}
		}

		void OnVolumeChanged()
		{
			impl.Volume = Volume;
		}

		void OnAspectModeChanged()
		{
			impl.AspectMode = AspectMode;
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

		void OnErrorOccurred(object sender, EventArgs e)
		{
			ErrorOccurred?.Invoke(this, EventArgs.Empty);
		}

		async void HideController(int after)
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
			catch (Exception)
			{
				//Exception when canceled
			}
		}

		void ShowController()
		{
			if (controls.IsValueCreated)
			{
				controls.Value.IsVisible = true;
				controls.Value.FadeTo(1.0, 200);
				HideController(5000);
			}
		}

		static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			(bindable as MediaPlayer)?.OnSourceChanged(bindable, EventArgs.Empty);
		}
	}
}