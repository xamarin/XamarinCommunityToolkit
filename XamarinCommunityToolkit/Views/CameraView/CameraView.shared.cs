using System;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class CameraView : View
	{
		readonly WeakEventManager<MediaCapturedEventArgs> mediaCapturedEventManager = new WeakEventManager<MediaCapturedEventArgs>();
		readonly WeakEventManager<string> mediaCaptureFailedEventManager = new WeakEventManager<string>();
		readonly WeakEventManager<bool> onAvailableEventManager = new WeakEventManager<bool>();
		readonly WeakEventManager shutterClickedEventManager = new WeakEventManager();

		public event EventHandler<MediaCapturedEventArgs> MediaCaptured
		{
			add => mediaCapturedEventManager.AddEventHandler(value);
			remove => mediaCapturedEventManager.RemoveEventHandler(value);
		}

		public event EventHandler<string> MediaCaptureFailed
		{
			add => mediaCaptureFailedEventManager.AddEventHandler(value);
			remove => mediaCaptureFailedEventManager.RemoveEventHandler(value);
		}

		public event EventHandler<bool> OnAvailable
		{
			add => onAvailableEventManager.AddEventHandler(value);
			remove => onAvailableEventManager.RemoveEventHandler(value);
		}

		internal event EventHandler ShutterClicked
		{
			add => shutterClickedEventManager.AddEventHandler(value);
			remove => shutterClickedEventManager.RemoveEventHandler(value);
		}

		public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(CameraView), false);

		public bool IsBusy
		{
			get => (bool)GetValue(IsBusyProperty);
			set => SetValue(IsBusyProperty, value);
		}

		public static readonly BindableProperty IsAvailableProperty = BindableProperty.Create(nameof(IsAvailable), typeof(bool), typeof(CameraView), false, propertyChanged: (b, o, n) => ((CameraView)b).RaiseAvailable((bool)n));

		public bool IsAvailable
		{
			get => (bool)GetValue(IsAvailableProperty);
			set => SetValue(IsAvailableProperty, value);
		}

		public static readonly BindableProperty CameraOptionsProperty = BindableProperty.Create(nameof(CameraOptions), typeof(CameraOptions), typeof(CameraView), CameraOptions.Default);

		public CameraOptions CameraOptions
		{
			get => (CameraOptions)GetValue(CameraOptionsProperty);
			set => SetValue(CameraOptionsProperty, value);
		}

		public static readonly BindableProperty SavePhotoToFileProperty = BindableProperty.Create(nameof(SavePhotoToFile), typeof(bool), typeof(CameraView), false);

		public bool SavePhotoToFile
		{
			get => (bool)GetValue(SavePhotoToFileProperty);
			set => SetValue(SavePhotoToFileProperty, value);
		}

		public static readonly BindableProperty CaptureOptionsProperty = BindableProperty.Create(nameof(CaptureOptions), typeof(CameraCaptureOptions), typeof(CameraView), CameraCaptureOptions.Default);

		public CameraCaptureOptions CaptureOptions
		{
			get => (CameraCaptureOptions)GetValue(CaptureOptionsProperty);
			set => SetValue(CaptureOptionsProperty, value);
		}

		public static readonly BindableProperty VideoStabilizationProperty = BindableProperty.Create(nameof(VideoStabilization), typeof(bool), typeof(CameraView), false);

		public bool VideoStabilization
		{
			get => (bool)GetValue(VideoStabilizationProperty);
			set => SetValue(VideoStabilizationProperty, value);
		}

		public static readonly BindableProperty FlashModeProperty = BindableProperty.Create(nameof(FlashMode), typeof(CameraFlashMode), typeof(CameraView), CameraFlashMode.Off);

		public CameraFlashMode FlashMode
		{
			get => (CameraFlashMode)GetValue(FlashModeProperty);
			set => SetValue(FlashModeProperty, value);
		}

		// Only supported by Android, removed until we have platform specifics
		//public static readonly BindableProperty PreviewAspectProperty = BindableProperty.Create(nameof(PreviewAspect), typeof(Aspect), typeof(CameraView), Aspect.AspectFit);

		//public Aspect PreviewAspect
		//{
		//	get => (Aspect)GetValue(PreviewAspectProperty);
		//	set => SetValue(PreviewAspectProperty, value);
		//}

		public static readonly BindableProperty ZoomProperty = BindableProperty.Create(nameof(Zoom), typeof(double), typeof(CameraView), 1d);

		public double Zoom
		{
			get => (double)GetValue(ZoomProperty);
			set => SetValue(ZoomProperty, value);
		}

		public static readonly BindableProperty MaxZoomProperty = BindableProperty.Create(nameof(MaxZoom), typeof(double), typeof(CameraView), 1d);

		public double MaxZoom
		{
			get => (double)GetValue(MaxZoomProperty);
			set => SetValue(MaxZoomProperty, value);
		}

		internal void RaiseMediaCaptured(MediaCapturedEventArgs args) => mediaCapturedEventManager.RaiseEvent(this, args, nameof(MediaCaptured));

		internal void RaiseMediaCaptureFailed(string message) => mediaCaptureFailedEventManager.RaiseEvent(this, message, nameof(MediaCaptureFailed));

		public void Shutter() => shutterClickedEventManager.RaiseEvent(this, EventArgs.Empty, nameof(ShutterClicked));

		void RaiseAvailable(bool isAvailable) => onAvailableEventManager.RaiseEvent(this, isAvailable, nameof(OnAvailable));
	}
}