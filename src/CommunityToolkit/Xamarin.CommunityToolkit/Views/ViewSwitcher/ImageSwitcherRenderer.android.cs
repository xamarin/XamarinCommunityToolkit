using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;
using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Views;
using AndroidX.Core.View;
using AImageView = Android.Widget.ImageView;
using AView = Android.Views.View;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.FastRenderers;
using Xamarin.CommunityToolkit.Extensions;
using Animation = Android.Views.Animations.Animation;
using AImageSwitcher = Android.Widget.ImageSwitcher;
using AViewSwitcher = Android.Widget.ViewSwitcher;
using Res = Android.Resource;
using Size = Xamarin.Forms.Size;
using System.Collections.Generic;
using Xamarin.Forms.Internals;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.Extensions.Internals;
using System.Threading.Tasks;
using Android.Views.Animations;
using ImageRenderer = Xamarin.Forms.Platform.Android.FastRenderers.ImageRenderer;

// Copied from Xamarin.Forms (ImageRenderer - Fast Renderer)
[assembly: ExportRenderer(typeof(ImageSwitcher), typeof(ImageSwitcherRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable SA1000 // Keywords should be spaced correctly
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8604 // Possible null reference argument.
	public class ImageSwitcherRenderer : AImageSwitcher, IVisualElementRenderer, IViewRenderer, ITabStop, AViewSwitcher.IViewFactory
	{
		bool disposed;
		ImageSwitcher element;
		int? defaultLabelFor;
		VisualElementTracker visualElementTracker;
		VisualElementRenderer visualElementRenderer;
		readonly MotionEventHelper motionEventHelper = new();
		readonly ImageRenderer[] children = new ImageRenderer[2];
		readonly Stack<ImageRenderer> childrenStack = new();

		readonly WeakEventManager<VisualElementChangedEventArgs> elementChangedEventManager = new();
		readonly WeakEventManager<PropertyChangedEventArgs> elementPropertyChangedEventManager = new();

		public ImageSwitcherRenderer(Context context)
			: base(context)
		{
			childrenStack.Push(children[0] = new ImageRenderer(Context));
			childrenStack.Push(children[1] = new ImageRenderer(Context));
			SetFactory(this);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			disposed = true;

			if (disposing)
			{
				if (element != null)
				{
					element.PropertyChanged -= OnElementPropertyChanged;
				}

				BackgroundManager.Dispose(this);

				visualElementTracker?.Dispose();
				visualElementTracker = null;

				visualElementRenderer?.Dispose();
				visualElementRenderer = null;

				children.ForEach(c => c.Dispose());

				Platform.ClearRenderer(this);
			}

			base.Dispose(disposing);
		}

		public override void Draw(Canvas? canvas)
		{
			canvas?.ClipShape(Context, Element);

			base.Draw(canvas);
		}

		protected virtual void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			this.EnsureId();
			elementChangedEventManager.RaiseEvent(this, new VisualElementChangedEventArgs(e.OldElement, e.NewElement), nameof(ElementChanged));

			if (e.OldElement != null)
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;

			if (e.NewElement != null)
			{
				UpdateTransition();
				children.ForEach(c => ((IVisualElementRenderer)c).SetElement(e.NewElement));
			}
		}

		protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.IsDisposed())
				return;

			elementPropertyChangedEventManager.RaiseEvent(this, e, nameof(ElementPropertyChanged));

			if (e.PropertyName == ViewSwitcher.TransitionDurationProperty.PropertyName ||
				e.PropertyName == ViewSwitcher.TransitionTypeProperty.PropertyName)
				UpdateTransition();
		}

		public override bool OnTouchEvent(MotionEvent? e)
		{
			if (visualElementRenderer.OnTouchEvent(e) || base.OnTouchEvent(e))
			{
				return true;
			}

			return motionEventHelper.HandleMotionEvent(Parent, e);
		}

		void UpdateTransition()
		{
			Animation? inAnimation = null;
			Animation? outAnimation = null;

			switch (Element.TransitionType)
			{
				case TransitionType.Fade:
					inAnimation = AnimationUtils.LoadAnimation(Context, Res.Animation.FadeIn);
					outAnimation = AnimationUtils.LoadAnimation(Context, Res.Animation.FadeOut);
					break;
				case TransitionType.MoveInFromLeft:
					inAnimation = AnimationUtils.LoadAnimation(Context, Res.Animation.SlideInLeft);
					outAnimation = AnimationUtils.LoadAnimation(Context, Res.Animation.SlideOutRight);
					break;
			}

			if (inAnimation == null || outAnimation == null)
				return;

			var duration = (int)Element.TransitionDuration;
			inAnimation.Duration = duration;
			outAnimation.Duration = duration;

			InAnimation = inAnimation;
			OutAnimation = outAnimation;
		}

		Size MinimumSize() => default;

		SizeRequest IVisualElementRenderer.GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			if (disposed)
				return default;

			Measure(widthConstraint, heightConstraint);
			return new SizeRequest(new Size(MeasuredWidth, MeasuredHeight), MinimumSize());
		}

		void IVisualElementRenderer.SetElement(VisualElement element)
		{
			if (element == null)
				throw new ArgumentNullException(nameof(element));

			if (element is not ImageSwitcher image)
				throw new ArgumentException("Element is not of type " + typeof(Image), nameof(element));

			var oldElement = this.element;
			this.element = image;

			if (oldElement != null)
				oldElement.PropertyChanged -= OnElementPropertyChanged;

			element.PropertyChanged += OnElementPropertyChanged;

			visualElementTracker ??= new VisualElementTracker(this);

			if (visualElementRenderer == null)
			{
				visualElementRenderer = new VisualElementRenderer(this);
				BackgroundManager.Init(this);
			}

			motionEventHelper.UpdateElement(element);
			OnElementChanged(new ElementChangedEventArgs<Image>(oldElement, this.element));
		}

		void IVisualElementRenderer.SetLabelFor(int? id)
		{
			if (defaultLabelFor == null)
				defaultLabelFor = ViewCompat.GetLabelFor(this);

			ViewCompat.SetLabelFor(this, (int)(id ?? defaultLabelFor));
		}

		void IVisualElementRenderer.UpdateLayout()
			=> visualElementTracker?.UpdateLayout();

		void IViewRenderer.MeasureExactly()
		{
			if (Element == null)
				return;

			var width = Element.Width;
			var height = Element.Height;

			if (width <= 0 || height <= 0)
			{
				return;
			}

			var realWidth = (int)Context.ToPixels(width);
			var realHeight = (int)Context.ToPixels(height);

			var widthMeasureSpec = MeasureSpecFactory.MakeMeasureSpec(realWidth, MeasureSpecMode.Exactly);
			var heightMeasureSpec = MeasureSpecFactory.MakeMeasureSpec(realHeight, MeasureSpecMode.Exactly);

			Measure(widthMeasureSpec, heightMeasureSpec);
		}

		VisualElement IVisualElementRenderer.Element => element;

		VisualElementTracker IVisualElementRenderer.Tracker => visualElementTracker;

		AView IVisualElementRenderer.View => this;

		AView ITabStop.TabStop => this;

		ViewGroup IVisualElementRenderer.ViewGroup => null;

		protected AImageSwitcher Control => this;

		protected ImageSwitcher Element => element;

		public event EventHandler<VisualElementChangedEventArgs> ElementChanged
		{
			add => elementChangedEventManager.AddEventHandler(value);
			remove => elementChangedEventManager.RemoveEventHandler(value);
		}

		public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged
		{
			add => elementPropertyChangedEventManager.AddEventHandler(value);
			remove => elementPropertyChangedEventManager.RemoveEventHandler(value);
		}

		public AView? MakeView()
			=> childrenStack.Pop();
	}
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore SA1000 // Keywords should be spaced correctly
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}