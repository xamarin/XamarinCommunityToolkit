using System;using Microsoft.Extensions.Logging;
using System.Windows.Input;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Internals;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class CameraView : View
	{
		[Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
		public event EventHandler<MediaCapturedEventArgs>? MediaCaptured;

		[Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
		public event EventHandler<string>? MediaCaptureFailed;

		public event EventHandler<bool>? OnAvailable;

		internal event EventHandler? ShutterClicked;

		internal static readonly BindablePropertyKey ShutterCommandPropertyKey =
			BindableProperty.CreateReadOnly(nameof(ShutterCommand),
				typeof(ICommand),
				typeof(CameraView),
				default,
				BindingMode.OneWayToSource,
				defaultValueCreator: ShutterCommandValueCreator);

		public static readonly BindableProperty ShutterCommandProperty = ShutterCommandPropertyKey.BindableProperty;

		[Microsoft.Maui.Controls.Internals.Preserve(Conditional = true)]
		public ICommand? ShutterCommand => (ICommand?)GetValue(ShutterCommandProperty);

		public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(nameof(IsBusy), typeof(bool), typeof(CameraView), false);

		public bool IsBusy
		{
			get => (bool)GetValue(IsBusyProperty);
			set => SetValue(IsBusyProperty, value);
		}

		public static readonly BindableProperty IsAvailableProperty = BindableProperty.Create(nameof(IsAvailable), typeof(bool), typeof(CameraView), false, propertyChanged: (b, o, n) => ((CameraView)b).OnAvailable?.Invoke(b, (bool)n));

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

		// TODO: See if we re-add this and implement it in a sane way
		// Right now this doesn't work on all platforms and the user has no control over where things are saved
		/*public static readonly BindableProperty SavePhotoToFileProperty = BindableProperty.Create(nameof(SavePhotoToFile), typeof(bool), typeof(CameraView), false);

		public bool SavePhotoToFile
		{
			get => (bool)GetValue(SavePhotoToFileProperty);
			set => SetValue(SavePhotoToFileProperty, value);
		}*/

		public static readonly BindableProperty CaptureModeProperty = BindableProperty.Create(nameof(CaptureMode), typeof(CameraCaptureMode), typeof(CameraView), CameraCaptureMode.Default);

		public CameraCaptureMode CaptureMode
		{
			get => (CameraCaptureMode)GetValue(CaptureModeProperty);
			set => SetValue(CaptureModeProperty, value);
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
		// public static readonly BindableProperty PreviewAspectProperty = BindableProperty.Create(nameof(PreviewAspect), typeof(Aspect), typeof(CameraView), Aspect.AspectFit);

		// public Aspect PreviewAspect
		// {
		// get => (Aspect)GetValue(PreviewAspectProperty);
		// set => SetValue(PreviewAspectProperty, value);
		// }

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

		internal void RaiseMediaCaptured(MediaCapturedEventArgs args) => MediaCaptured?.Invoke(this, args);

		internal void RaiseMediaCaptureFailed(string message) => MediaCaptureFailed?.Invoke(this, message);

		public void Shutter() => ShutterClicked?.Invoke(this, EventArgs.Empty);

		static object? ShutterCommandValueCreator(BindableObject? b)
		{
			if (b is CameraView camera)
				return new Command(camera.Shutter);

			return null;
		}
	}
}