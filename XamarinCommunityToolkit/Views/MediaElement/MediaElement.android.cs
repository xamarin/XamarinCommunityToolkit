using Android.Content;
using Android.Media;
using Android.Views;
using Android.Widget;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;
using AView = Android.Views.View;
using CommunityToolkit = Xamarin.CommunityToolkit.UI.Views;
using MediaElementRenderer = Xamarin.CommunityToolkit.Android.UI.Views.MediaElementRenderer;

[assembly: ExportRenderer(typeof(CommunityToolkit::MediaElement), typeof(MediaElementRenderer))]
namespace Xamarin.CommunityToolkit.Android.UI.Views
{
	[Preserve(AllMembers = true)]
	public sealed class MediaElementRenderer : FrameLayout, IVisualElementRenderer, IViewRenderer, MediaPlayer.IOnCompletionListener, MediaPlayer.IOnInfoListener, MediaPlayer.IOnPreparedListener, MediaPlayer.IOnErrorListener
	{
		bool isDisposed;
		int? defaultLabelFor;
		CommunityToolkit::MediaElement MediaElement { get; set; }
		CommunityToolkit::IMediaElementController Controller => MediaElement;

		VisualElementTracker tracker;

		MediaController controller;
		MediaPlayer mediaPlayer;
		FormsVideoView view;

		public MediaElementRenderer(Context context) : base(context)
		{
			view = new FormsVideoView(Context);
			view.SetZOrderMediaOverlay(true);
			view.SetOnCompletionListener(this);
			view.SetOnInfoListener(this);
			view.SetOnPreparedListener(this);
			view.SetOnErrorListener(this);
			view.MetadataRetrieved += MetadataRetrieved;

			AddView(view, -1, -1);

			controller = new MediaController(Context);
			controller.SetAnchorView(this);
			view.SetMediaController(controller);
		}

		public VisualElement Element => MediaElement;

		VisualElementTracker IVisualElementRenderer.Tracker => tracker;

		ViewGroup IVisualElementRenderer.ViewGroup => null;

		AView IVisualElementRenderer.View => this;

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
		public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

		SizeRequest IVisualElementRenderer.GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			AView view = this;
			view.Measure(widthConstraint, heightConstraint);

			return new SizeRequest(new Size(MeasuredWidth, MeasuredHeight), new Size());
		}

		void IViewRenderer.MeasureExactly()
		{
			if (Controller == null || Element == null)
			{
				return;
			}

			var width = Element.Width;
			var height = Element.Height;

			if (width <= 0 || height <= 0)
			{
				return;
			}

			var realWidth = (int)Context.ToPixels(width);
			var realHeight = (int)Context.ToPixels(height);

			var widthMeasureSpec = MeasureSpec.MakeMeasureSpec(realWidth, MeasureSpecMode.Exactly);
			var heightMeasureSpec = MeasureSpec.MakeMeasureSpec(realHeight, MeasureSpecMode.Exactly);

			Measure(widthMeasureSpec, heightMeasureSpec);
		}

		void UnsubscribeFromEvents(CommunityToolkit::MediaElement element)
		{
			if (element == null)
				return;

			element.PropertyChanged -= OnElementPropertyChanged;
			element.SeekRequested -= SeekRequested;
			element.StateRequested -= StateRequested;
			element.PositionRequested -= OnPositionRequested;
		}

		void IVisualElementRenderer.SetElement(VisualElement element)
		{
			if (element is null)
				throw new ArgumentNullException(nameof(element));

			if (!(element is CommunityToolkit::MediaElement))
				throw new ArgumentException($"{nameof(element)} must be of type {nameof(MediaElement)}");

			var oldElement = MediaElement;
			MediaElement = (CommunityToolkit::MediaElement)element;

			Performance.Start(out var reference);

			if (oldElement != null)
			{
				UnsubscribeFromEvents(oldElement);
			}

			var currentColor = oldElement?.BackgroundColor ?? Color.Default;
			if (element.BackgroundColor != currentColor)
			{
				UpdateBackgroundColor();
			}

			if (MediaElement != null)
			{
				MediaElement.PropertyChanged += OnElementPropertyChanged;
				MediaElement.SeekRequested += SeekRequested;
				MediaElement.StateRequested += StateRequested;
				MediaElement.PositionRequested += OnPositionRequested;
			}

			if (tracker is null)
			{
				// Can't set up the tracker in the constructor because it access the Element (for now)
				SetTracker(new VisualElementTracker(this));
			}

			OnElementChanged(new ElementChangedEventArgs<CommunityToolkit::MediaElement>(oldElement as CommunityToolkit::MediaElement, MediaElement));

			Performance.Stop(reference);
		}

		void StateRequested(object sender, CommunityToolkit::StateRequested e)
		{
			if (view == null)
				return;

			switch (e.State)
			{
				case CommunityToolkit::MediaElementState.Playing:
					view.Start();
					Controller.CurrentState = view.IsPlaying ? CommunityToolkit::MediaElementState.Playing : CommunityToolkit::MediaElementState.Stopped;
					break;

				case CommunityToolkit::MediaElementState.Paused:
					if (view.CanPause())
					{
						view.Pause();
						Controller.CurrentState = CommunityToolkit::MediaElementState.Paused;
					}
					break;

				case CommunityToolkit::MediaElementState.Stopped:
					view.Pause();
					view.SeekTo(0);

					Controller.CurrentState = view.IsPlaying ? CommunityToolkit::MediaElementState.Playing : CommunityToolkit::MediaElementState.Stopped;
					break;
			}

			UpdateLayoutParameters();
			Controller.Position = view.Position;
		}

		void OnPositionRequested(object sender, EventArgs e)
		{
			if (view == null)
				return;

			Controller.Position = view.Position;
		}

		void SeekRequested(object sender, CommunityToolkit::SeekRequested e)
		{
			if (view == null)
				return;

			Controller.Position = view.Position;
		}

		void IVisualElementRenderer.SetLabelFor(int? id)
		{
			if (defaultLabelFor is null)
			{
				defaultLabelFor = LabelFor;
			}

			LabelFor = (int)(id ?? defaultLabelFor);
		}

		void SetTracker(VisualElementTracker tracker) => this.tracker = tracker;

		void UpdateBackgroundColor() => SetBackgroundColor(Element.BackgroundColor.ToAndroid());

		void IVisualElementRenderer.UpdateLayout() => tracker?.UpdateLayout();

		protected override void Dispose(bool disposing)
		{
			if (isDisposed)
			{
				return;
			}

			isDisposed = true;

			ReleaseControl();

			if (disposing)
			{
				SetOnClickListener(null);
				SetOnTouchListener(null);

				tracker?.Dispose();

				if (Element != null)
				{
					UnsubscribeFromEvents(Element as CommunityToolkit::MediaElement);
				}
			}

			base.Dispose(disposing);
		}

		// TODO: Make virtual when unsealed
		void OnElementChanged(ElementChangedEventArgs<CommunityToolkit::MediaElement> e)
		{
			if (e.OldElement != null)
			{

			}

			if (e.NewElement != null)
			{
				this.EnsureId();

				UpdateKeepScreenOn();
				UpdateLayoutParameters();
				UpdateShowPlaybackControls();
				UpdateSource();
				UpdateBackgroundColor();

				ElevationHelper.SetElevation(this, e.NewElement);
			}

			ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(e.OldElement, e.NewElement));
		}

		void MetadataRetrieved(object sender, EventArgs e)
		{
			if (view == null)
				return;

			Controller.Duration = view.DurationTimeSpan;
			Controller.VideoHeight = view.VideoHeight;
			Controller.VideoWidth = view.VideoWidth;

			Device.BeginInvokeOnMainThread(UpdateLayoutParameters);
		}

		// TODO: Make virtual when unsealed
		void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(MediaElement.Aspect):
					UpdateLayoutParameters();
					break;

				case nameof(MediaElement.IsLooping):
					if (mediaPlayer != null)
					{
						mediaPlayer.Looping = MediaElement.IsLooping;
					}
					break;

				case nameof(MediaElement.KeepScreenOn):
					UpdateKeepScreenOn();
					break;

				case nameof(MediaElement.ShowsPlaybackControls):
					UpdateShowPlaybackControls();
					break;

				case nameof(MediaElement.Source):
					UpdateSource();
					break;

				case nameof(MediaElement.Volume):
					mediaPlayer?.SetVolume((float)MediaElement.Volume, (float)MediaElement.Volume);
					break;
			}

			ElementPropertyChanged?.Invoke(this, e);
		}

		void UpdateKeepScreenOn()
		{
			if (view == null)
				return;

			view.KeepScreenOn = MediaElement.KeepScreenOn;
		}

		void UpdateShowPlaybackControls()
		{
			if (controller == null)
				return;

			controller.Visibility = MediaElement.ShowsPlaybackControls ? ViewStates.Visible : ViewStates.Gone;
		}

		void UpdateSource()
		{
			if (view == null)
				return;

			if (MediaElement.Source != null)
			{
				if (MediaElement.Source is CommunityToolkit::UriMediaSource uriSource)
				{
					if (uriSource.Uri.Scheme == "ms-appx")
					{
						if (uriSource.Uri.LocalPath.Length <= 1)
							return;

						// video resources should be in the raw folder with Build Action set to AndroidResource
						var uri = "android.resource://" + Context.PackageName + "/raw/" + uriSource.Uri.LocalPath[1..uriSource.Uri.LocalPath.LastIndexOf('.')].ToLower();
						view.SetVideoURI(global::Android.Net.Uri.Parse(uri));
					}
					else if (uriSource.Uri.Scheme == "ms-appdata")
					{
						var filePath = ResolveMsAppDataUri(uriSource.Uri);

						if (string.IsNullOrEmpty(filePath))
							throw new ArgumentException("Invalid Uri", "Source");

						view.SetVideoPath(filePath);

					}
					else
					{
						if (uriSource.Uri.IsFile)
						{
							view.SetVideoPath(uriSource.Uri.AbsolutePath);
						}
						else
						{
							view.SetVideoURI(global::Android.Net.Uri.Parse(uriSource.Uri.AbsoluteUri));
						}
					}
				}
				else if (MediaElement.Source is CommunityToolkit::FileMediaSource fileSource)
				{
					view.SetVideoPath(fileSource.File);
				}

				if (MediaElement.AutoPlay)
				{
					view.Start();
					Controller.CurrentState = view.IsPlaying ? CommunityToolkit::MediaElementState.Playing : CommunityToolkit::MediaElementState.Stopped;
				}

			}
			else if (view.IsPlaying)
			{
				view.StopPlayback();
				Controller.CurrentState = CommunityToolkit::MediaElementState.Stopped;
			}
		}

		string ResolveMsAppDataUri(Uri uri)
		{
			if (uri.Scheme == "ms-appdata")
			{
				string filePath;

				if (uri.LocalPath.StartsWith("/local"))
				{
					filePath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), uri.LocalPath.Substring(7));
				}
				else if (uri.LocalPath.StartsWith("/temp"))
				{
					filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), uri.LocalPath.Substring(6));
				}
				else
				{
					throw new ArgumentException("Invalid Uri", "Source");
				}

				return filePath;
			}
			else
			{
				throw new ArgumentException("uri");
			}
		}

		void MediaPlayer.IOnCompletionListener.OnCompletion(MediaPlayer mp)
		{
			if (Controller == null)
				return;

			Controller.Position = TimeSpan.FromMilliseconds(mediaPlayer.CurrentPosition);
			Controller.OnMediaEnded();
		}

		void MediaPlayer.IOnPreparedListener.OnPrepared(MediaPlayer mp)
		{
			if (Controller == null)
				return;

			Controller.OnMediaOpened();

			UpdateLayoutParameters();

			mediaPlayer = mp;
			mp.Looping = MediaElement.IsLooping;
			mp.SeekTo(0);

			if (MediaElement.AutoPlay)
			{
				mediaPlayer.Start();
				Controller.CurrentState = CommunityToolkit::MediaElementState.Playing;
			}
			else
			{
				Controller.CurrentState = CommunityToolkit::MediaElementState.Paused;
			}
		}

		void UpdateLayoutParameters()
		{
			if (view == null)
				return;

			if (view.VideoWidth == 0 || view.VideoHeight == 0)
			{
				view.LayoutParameters = new FrameLayout.LayoutParams(Width, Height, GravityFlags.Fill);
				return;
			}

			var ratio = (float)view.VideoWidth / (float)view.VideoHeight;
			var controlRatio = (float)Width / Height;

			switch (MediaElement.Aspect)
			{
				case Aspect.Fill:
					// TODO: this doesn't stretch like other platforms...
					view.LayoutParameters = new FrameLayout.LayoutParams(Width, Height, GravityFlags.Fill) { LeftMargin = 0, RightMargin = 0, TopMargin = 0, BottomMargin = 0 };
					break;

				case Aspect.AspectFit:
					if (ratio > controlRatio)
					{
						var requiredHeight = (int)(Width / ratio);
						var vertMargin = (Height - requiredHeight) / 2;
						view.LayoutParameters = new FrameLayout.LayoutParams(Width, requiredHeight, GravityFlags.FillHorizontal | GravityFlags.CenterVertical) { LeftMargin = 0, RightMargin = 0, TopMargin = vertMargin, BottomMargin = vertMargin };
					}
					else
					{
						var requiredWidth = (int)(Height * ratio);
						var horizMargin = (Width - requiredWidth) / 2;
						view.LayoutParameters = new FrameLayout.LayoutParams(requiredWidth, Height, GravityFlags.CenterHorizontal | GravityFlags.FillVertical) { LeftMargin = horizMargin, RightMargin = horizMargin, TopMargin = 0, BottomMargin = 0 };
					}
					break;

				case Aspect.AspectFill:
					if (ratio > controlRatio)
					{
						var requiredWidth = (int)(Height * ratio);
						var horizMargin = (Width - requiredWidth) / 2;
						view.LayoutParameters = new FrameLayout.LayoutParams((int)(Height * ratio), Height, GravityFlags.CenterHorizontal | GravityFlags.FillVertical) { TopMargin = 0, BottomMargin = 0, LeftMargin = horizMargin, RightMargin = horizMargin };
					}
					else
					{
						var requiredHeight = (int)(Width / ratio);
						var vertMargin = (Height - requiredHeight) / 2;
						view.LayoutParameters = new FrameLayout.LayoutParams(Width, requiredHeight, GravityFlags.FillHorizontal | GravityFlags.CenterVertical) { LeftMargin = 0, RightMargin = 0, TopMargin = vertMargin, BottomMargin = vertMargin };
					}

					break;
			}
		}

		void ReleaseControl()
		{
			if (view != null)
			{
				view.MetadataRetrieved -= MetadataRetrieved;
				RemoveView(view);
				view.SetOnPreparedListener(null);
				view.SetOnCompletionListener(null);
				view.Dispose();
				view = null;
			}

			if (controller != null)
			{
				controller.Dispose();
				controller = null;
			}

			if (mediaPlayer != null)
			{
				mediaPlayer.Dispose();
				mediaPlayer = null;
			}
		}

		bool MediaPlayer.IOnErrorListener.OnError(MediaPlayer mp, MediaError what, int extra)
		{
			if (Controller == null)
				return false;

			Controller.OnMediaFailed();
			return false;
		}

		bool MediaPlayer.IOnInfoListener.OnInfo(MediaPlayer mp, MediaInfo what, int extra)
		{
			if (view == null)
				return false;

			switch (what)
			{
				case MediaInfo.BufferingStart:
					Controller.CurrentState = CommunityToolkit::MediaElementState.Buffering;
					mp.BufferingUpdate += Mp_BufferingUpdate;
					break;

				case MediaInfo.BufferingEnd:
					mp.BufferingUpdate -= Mp_BufferingUpdate;
					Controller.CurrentState = CommunityToolkit::MediaElementState.Paused;
					break;

				case MediaInfo.VideoRenderingStart:
					view.SetBackground(null);
					Controller.CurrentState = CommunityToolkit::MediaElementState.Playing;
					break;
			}

			mediaPlayer = mp;

			return true;
		}

		void Mp_BufferingUpdate(object sender, MediaPlayer.BufferingUpdateEventArgs e)
		{
			Controller.BufferingProgress = e.Percent / 100f;
		}
	}
}