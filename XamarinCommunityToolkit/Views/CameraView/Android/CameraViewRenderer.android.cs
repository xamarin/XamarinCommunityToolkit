using System;
using System.ComponentModel;

#if __ANDROID_29__
using AndroidX.Fragment.App;
#else
using Android.Support.V4.App;
#endif

using Android.Content;
using Android.Views;
using Android.Widget;
using AView = Android.Views.View;

using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android.FastRenderers;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class CameraViewRenderer : FrameLayout, IVisualElementRenderer, IViewRenderer
	{
		int? defaultLabelFor;
		bool disposed;
		CameraView element;
		VisualElementTracker visualElementTracker;
		VisualElementRenderer visualElementRenderer;
		readonly MotionEventHelper motionEventHelper;
		FragmentManager fragmentManager;

		FragmentManager FragmentManager => fragmentManager ?? (fragmentManager = Context.GetFragmentManager());

		CameraFragment camerafragment;

		public CameraViewRenderer(Context context) : base(context)
		{
			motionEventHelper = new MotionEventHelper();
			visualElementRenderer = new VisualElementRenderer(this);
		}

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

		public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

		async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			ElementPropertyChanged?.Invoke(this, e);

			switch (e.PropertyName)
			{
				case nameof(CameraView.CameraOptions):
					await camerafragment.RetrieveCameraDevice();
					break;
				case nameof(CameraView.CaptureOptions):
					camerafragment.UpdateCaptureOptions();
					await camerafragment.RetrieveCameraDevice();
					break;
				case nameof(CameraView.FlashMode):
					camerafragment.SetFlash();
					if (Element.CaptureOptions == CameraCaptureOptions.Video)
						camerafragment.UpdateRepeatingRequest();
					break;
				case nameof(CameraView.Zoom):
					camerafragment.ApplyZoom();
					camerafragment.UpdateRepeatingRequest();
					break;
				case nameof(CameraView.VideoStabilization):
					camerafragment.SetVideoStabilization();
					if (Element.CaptureOptions == CameraCaptureOptions.Video)
						camerafragment.UpdateRepeatingRequest();
					break;
				case nameof(CameraView.PreviewAspect):
				case "MirrorFrontPreview":
					camerafragment?.ConfigureTransform();
					break;
				case nameof(CameraView.KeepScreenOn):
					if (camerafragment != null)
						camerafragment.KeepScreenOn = Element.KeepScreenOn;
					break;
			}
		}

		void OnElementChanged(ElementChangedEventArgs<CameraView> e)
		{
			CameraFragment newfragment = null;

			if (e.OldElement != null)
			{
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;
				e.OldElement.ShutterClicked -= OnShutterClicked;
				camerafragment.Dispose();
			}

			if (e.NewElement != null)
			{
				this.EnsureId();

				e.NewElement.PropertyChanged += OnElementPropertyChanged;
				e.NewElement.ShutterClicked += OnShutterClicked;

				ElevationHelper.SetElevation(this, e.NewElement);
				newfragment = new CameraFragment() { Element = element };
			}

			FragmentManager.BeginTransaction()
				.Replace(Id, camerafragment = newfragment, "camera")
				.Commit();

			ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(e.OldElement, e.NewElement));
		}

		CameraView Element
		{
			get => element;
			set
			{
				if (element == value)
					return;

				var oldElement = element;
				element = value;

				OnElementChanged(new ElementChangedEventArgs<CameraView>(oldElement, element));
				//element?.SendViewInitialized(this);
			}
		}

		public override bool OnTouchEvent(MotionEvent e)
		{
			if (visualElementRenderer.OnTouchEvent(e) || base.OnTouchEvent(e))
				return true;

			return motionEventHelper.HandleMotionEvent(Parent, e);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			camerafragment.Dispose();

			disposed = true;

			if (disposing)
			{
				SetOnClickListener(null);
				SetOnTouchListener(null);
				if (visualElementTracker != null)
				{
					visualElementTracker.Dispose();
					visualElementTracker = null;
				}

				if (visualElementRenderer != null)
				{
					visualElementRenderer.Dispose();
					visualElementRenderer = null;
				}

				if (Element != null)
				{
					Element.PropertyChanged -= OnElementPropertyChanged;
					Element.ShutterClicked -= OnShutterClicked;

					if (Platform.GetRenderer(Element) == this)
						Platform.SetRenderer(Element, null);
				}
			}

			base.Dispose(disposing);
		}

		void OnShutterClicked(object sender, EventArgs e)
		{
			switch (Element.CaptureOptions)
			{
				default:
				case CameraCaptureOptions.Default:
				case CameraCaptureOptions.Photo:
					camerafragment.TakePhoto();
					break;
				case CameraCaptureOptions.Video:
					if (!camerafragment.IsRecordingVideo)
						camerafragment.StartRecord();
					else
						camerafragment.StopRecord();
					break;
			}
		}

		void IViewRenderer.MeasureExactly() => this.Invalidate();

		#region IVisualElementRenderer
		VisualElement IVisualElementRenderer.Element => Element;

		ViewGroup IVisualElementRenderer.ViewGroup => null;

		VisualElementTracker IVisualElementRenderer.Tracker => visualElementTracker;

		AView IVisualElementRenderer.View => this;

		SizeRequest IVisualElementRenderer.GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			Measure(widthConstraint, heightConstraint);
			var result = new SizeRequest(new Size(MeasuredWidth, MeasuredHeight), new Size(Context.ToPixels(20), Context.ToPixels(20)));
			return result;
		}

		void IVisualElementRenderer.SetElement(VisualElement element)
		{
			if (!(element is CameraView camera))
				throw new ArgumentException($"{nameof(element)} must be of type {nameof(CameraView)}");

			Performance.Start(out var reference);

			motionEventHelper.UpdateElement(element);

			if (visualElementTracker == null)
				visualElementTracker = new VisualElementTracker(this);

			Element = camera;

			Performance.Stop(reference);
		}

		void IVisualElementRenderer.SetLabelFor(int? id)
		{
			if (defaultLabelFor == null)
				defaultLabelFor = LabelFor;

			LabelFor = (int)(id ?? defaultLabelFor);
		}

		void IVisualElementRenderer.UpdateLayout()
		{
			visualElementTracker?.UpdateLayout();
		}
		#endregion
	}
}