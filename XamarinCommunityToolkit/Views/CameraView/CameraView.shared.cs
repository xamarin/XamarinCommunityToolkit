using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class CameraView : View
	{
		public event EventHandler<MediaCapturedEventArgs> MediaCaptured;

		public event EventHandler<string> MediaCaptureFailed;

		public event EventHandler<bool> OnAvailable;

		internal event EventHandler ShutterClicked;

		public static readonly BindableProperty ShutterCommandProperty = new BindablePropertyBuilder()
			.SetPropertyName(nameof(ShutterCommand))
			.SetReturnType(typeof(ICommand))
			.SetDeclaringType(typeof(CameraView))
			.Build();

		public ICommand ShutterCommand
		{
			get => (ICommand)GetValue(ShutterCommandProperty);
			set => SetValue(ShutterCommandProperty, value);
		}

		public static readonly BindableProperty ShutterCommandParameterProperty = new BindablePropertyBuilder()
			.SetPropertyName(nameof(ShutterCommandParameter))
			.SetReturnType(typeof(object))
			.SetDeclaringType(typeof(CameraView))
			.Build();

		public object ShutterCommandParameter
		{
			get => GetValue(ShutterCommandParameterProperty);
			set => SetValue(ShutterCommandParameterProperty, value);
		}

		public static readonly BindableProperty IsBusyProperty = new BindablePropertyBuilder()
			.SetPropertyName(nameof(IsBusy))
			.SetReturnType(typeof(bool))
			.SetDeclaringType(typeof(CameraView))
			.SetDefaultValue(false)
			.Build();

		public bool IsBusy
		{
			get => (bool)GetValue(IsBusyProperty);
			set => SetValue(IsBusyProperty, value);
		}

		public static readonly BindableProperty IsAvailableProperty = new BindablePropertyBuilder()
			.SetPropertyName(nameof(IsAvailable))
			.SetReturnType(typeof(bool))
			.SetDeclaringType(typeof(CameraView))
			.SetDefaultValue(false)
			.SetPropertyChangedDelegate((b, o, n) => ((CameraView)b).OnAvailable?.Invoke(b, (bool)n))
			.Build();

		public bool IsAvailable
		{
			get => (bool)GetValue(IsAvailableProperty);
			set => SetValue(IsAvailableProperty, value);
		}

		public static readonly BindableProperty CameraOptionsProperty = new BindablePropertyBuilder()
			.SetPropertyName(nameof(CameraOptions))
			.SetReturnType(typeof(CameraOptions))
			.SetDeclaringType(typeof(CameraView))
			.SetDefaultValue(CameraOptions.Default)
			.Build();

		public CameraOptions CameraOptions
		{
			get => (CameraOptions)GetValue(CameraOptionsProperty);
			set => SetValue(CameraOptionsProperty, value);
		}

		public static readonly BindableProperty SavePhotoToFileProperty = new BindablePropertyBuilder()
			.SetPropertyName(nameof(SavePhotoToFile))
			.SetReturnType(typeof(bool))
			.SetDeclaringType(typeof(CameraView))
			.SetDefaultValue(false)
			.Build();

		public bool SavePhotoToFile
		{
			get => (bool)GetValue(SavePhotoToFileProperty);
			set => SetValue(SavePhotoToFileProperty, value);
		}

		public static readonly BindableProperty CaptureOptionsProperty = new BindablePropertyBuilder()
			.SetPropertyName(nameof(CaptureOptions))
			.SetReturnType(typeof(CameraCaptureOptions))
			.SetDeclaringType(typeof(CameraView))
			.SetDefaultValue(CameraCaptureOptions.Default)
			.Build();

		public CameraCaptureOptions CaptureOptions
		{
			get => (CameraCaptureOptions)GetValue(CaptureOptionsProperty);
			set => SetValue(CaptureOptionsProperty, value);
		}

		public static readonly BindableProperty VideoStabilizationProperty = new BindablePropertyBuilder()
			.SetPropertyName(nameof(VideoStabilization))
			.SetReturnType(typeof(bool))
			.SetDeclaringType(typeof(CameraView))
			.SetDefaultValue(false)
			.Build();

		public bool VideoStabilization
		{
			get => (bool)GetValue(VideoStabilizationProperty);
			set => SetValue(VideoStabilizationProperty, value);
		}

		public static readonly BindableProperty FlashModeProperty = new BindablePropertyBuilder()
			.SetPropertyName(nameof(FlashMode))
			.SetReturnType(typeof(CameraFlashMode))
			.SetDeclaringType(typeof(CameraView))
			.SetDefaultValue(CameraFlashMode.Off)
			.Build();

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

		public static readonly BindableProperty ZoomProperty = new BindablePropertyBuilder()
			.SetPropertyName(nameof(Zoom))
			.SetReturnType(typeof(double))
			.SetDeclaringType(typeof(CameraView))
			.SetDefaultValue(1d)
			.Build();

		public double Zoom
		{
			get => (double)GetValue(ZoomProperty);
			set => SetValue(ZoomProperty, value);
		}

		public static readonly BindableProperty MaxZoomProperty = new BindablePropertyBuilder()
			.SetPropertyName(nameof(MaxZoom))
			.SetReturnType(typeof(double))
			.SetDeclaringType(typeof(CameraView))
			.SetDefaultValue(1d)
			.Build();

		public double MaxZoom
		{
			get => (double)GetValue(MaxZoomProperty);
			set => SetValue(MaxZoomProperty, value);
		}

		internal void RaiseMediaCaptured(MediaCapturedEventArgs args) => MediaCaptured?.Invoke(this, args);

		internal void RaiseMediaCaptureFailed(string message) => MediaCaptureFailed?.Invoke(this, message);

		public void Shutter()
		{
			ShutterClicked?.Invoke(this, EventArgs.Empty);
			ShutterCommand?.Execute(ShutterCommandParameter);
		}
	}
}