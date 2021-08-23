using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using AndroidX.Core.View;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.FastRenderers;
using static Android.Widget.TextView;
using Animation = Android.Views.Animations.Animation;
using ATextSwitcher = Android.Widget.TextSwitcher;
using AView = Android.Views.View;
using AViewSwitcher = Android.Widget.ViewSwitcher;
using Color = Xamarin.Forms.Color;
using Res = Android.Resource;
using Size = Xamarin.Forms.Size;

// Copied from Xamarin.Forms (LabelRenderer - Fast Renderer)
[assembly: ExportRenderer(typeof(TextSwitcher), typeof(TextSwitcherRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class TextSwitcherRenderer : ATextSwitcher, IVisualElementRenderer, IViewRenderer, ITabStop, AViewSwitcher.IViewFactory
	{
		int? defaultLabelFor;
		bool disposed;
		TextSwitcher? element;

		// Do not dispose labelTextColorDefault
		readonly ColorStateList? labelTextColorDefault;
		int lastConstraintHeight;
		int lastConstraintWidth;
		SizeRequest? lastSizeRequest;
		float lastTextSize = -1f;
		Typeface? lastTypeface;
		Color lastUpdateColor = Color.Default;
		float lineSpacingExtraDefault = -1.0f;
		float lineSpacingMultiplierDefault = -1.0f;
		VisualElementTracker? visualElementTracker;
		VisualElementRenderer? visualElementRenderer;

		readonly FormsTextView[] children = new FormsTextView[2];

		readonly WeakEventManager<VisualElementChangedEventArgs> elementChangedEventManager = new();
		readonly WeakEventManager<PropertyChangedEventArgs> elementPropertyChangedEventManager = new();
		readonly Stack<FormsTextView> childrenStack = new();
		readonly MotionEventHelper motionEventHelper = new();
		SpannableString? spannableString;
		bool hasLayoutOccurred;
		bool wasFormatted;

		public TextSwitcherRenderer(Context context)
			: base(context)
		{
			childrenStack.Push(children[0] = new FormsTextView(Context));
			childrenStack.Push(children[1] = new FormsTextView(Context));

			SetFactory(this);
			labelTextColorDefault = children[0].TextColors;
			visualElementRenderer = new VisualElementRenderer(this);
			BackgroundManager.Init(this);
		}

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

		VisualElement? IVisualElementRenderer.Element => Element;

		VisualElementTracker? IVisualElementRenderer.Tracker => visualElementTracker;

		AView IVisualElementRenderer.View => this;

		AView ITabStop.TabStop => this;

		ViewGroup? IVisualElementRenderer.ViewGroup => null;

		protected TextSwitcher? Element
		{
			get => element;
			set
			{
				if (element == value)
					return;

				var oldElement = element;
				element = value;
				OnElementChanged(new ElementChangedEventArgs<Label?>(oldElement, element));
			}
		}

		protected ATextSwitcher Control => this;

		string? Hint
		{
			get => children[0].Hint;
			set
			{
				foreach (var child in children)
					child.Hint = value;
			}
		}

		SizeRequest IVisualElementRenderer.GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			if (disposed)
			{
				return default;
			}

			if (lastSizeRequest.HasValue)
			{
				// if we are measuring the same thing, no need to waste the time
				var canRecycleLast = widthConstraint == lastConstraintWidth && heightConstraint == lastConstraintHeight;

				if (!canRecycleLast)
				{
					// if the last time we measured the returned size was all around smaller than the passed constraint
					// and the constraint is bigger than the last size request, we can assume the newly measured size request
					// will not change either.
					var lastConstraintWidthSize = MeasureSpecFactory.GetSize(lastConstraintWidth);
					var lastConstraintHeightSize = MeasureSpecFactory.GetSize(lastConstraintHeight);

					var currentConstraintWidthSize = MeasureSpecFactory.GetSize(widthConstraint);
					var currentConstraintHeightSize = MeasureSpecFactory.GetSize(heightConstraint);

					var lastWasSmallerThanConstraints = lastSizeRequest.Value.Request.Width < lastConstraintWidthSize && lastSizeRequest.Value.Request.Height < lastConstraintHeightSize;

					var currentConstraintsBiggerThanLastRequest = currentConstraintWidthSize >= lastSizeRequest.Value.Request.Width && currentConstraintHeightSize >= lastSizeRequest.Value.Request.Height;

					canRecycleLast = lastWasSmallerThanConstraints && currentConstraintsBiggerThanLastRequest;
				}

				if (canRecycleLast)
					return lastSizeRequest.Value;
			}

			// We need to clear the Hint or else it will interfere with the sizing of the Label
			var hint = Hint;
			var setHint = Control.LayoutParameters != null;
			if (!string.IsNullOrEmpty(hint) && setHint)
				Hint = string.Empty;

			_ = MeasureSpec.GetSize(heightConstraint);

			Measure(widthConstraint, heightConstraint);
			var result = new SizeRequest(new Size(MeasuredWidth, MeasuredHeight), default);

			// Set Hint back after sizing
			if (setHint)
				Hint = hint;

			result.Minimum = new Size(Math.Min(Context.ToPixels(10), result.Request.Width), result.Request.Height);

			// if the measure of the view has changed then trigger a request for layout
			// if the measure hasn't changed then force a layout of the label
			var measureIsChanged = !lastSizeRequest.HasValue ||
				(lastSizeRequest.HasValue && (lastSizeRequest.Value.Request.Height != MeasuredHeight || lastSizeRequest.Value.Request.Width != MeasuredWidth));
			if (measureIsChanged)
				this.MaybeRequestLayout();
			else
				ForceLayout();

			lastConstraintWidth = widthConstraint;
			lastConstraintHeight = heightConstraint;
			lastSizeRequest = result;

			return result;
		}

		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout(changed, left, top, right, bottom);

			foreach (var child in children)
				child.RecalculateSpanPositions(Element, spannableString, new SizeRequest(new Size(right - left, bottom - top)));

			hasLayoutOccurred = true;
		}

		void IVisualElementRenderer.SetElement(VisualElement element)
		{
			if (element is not TextSwitcher label)
				throw new ArgumentException("Element must be of type TextSwitcher.");

			Element = label;
			motionEventHelper.UpdateElement(element);
		}

		void IVisualElementRenderer.SetLabelFor(int? id)
		{
			defaultLabelFor ??= ViewCompat.GetLabelFor(this);
			ViewCompat.SetLabelFor(this, (int)(id ?? defaultLabelFor));
		}

		void IVisualElementRenderer.UpdateLayout()
		{
			var tracker = visualElementTracker;
			tracker?.UpdateLayout();
		}

		void IViewRenderer.MeasureExactly()
		{
			if (Element == null)
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

			var widthMeasureSpec = MeasureSpecFactory.MakeMeasureSpec(realWidth, MeasureSpecMode.Exactly);
			var heightMeasureSpec = MeasureSpecFactory.MakeMeasureSpec(realHeight, MeasureSpecMode.Exactly);

			Measure(widthMeasureSpec, heightMeasureSpec);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			disposed = true;

			if (disposing)
			{
				if (Element != null)
				{
					Element.PropertyChanged -= OnElementPropertyChanged;
				}

				BackgroundManager.Dispose(this);

				visualElementTracker?.Dispose();
				visualElementTracker = null;

				visualElementRenderer?.Dispose();
				visualElementRenderer = null;

				foreach (var child in children)
					child.Dispose();

				spannableString?.Dispose();
				Platform.ClearRenderer(this);
			}

			base.Dispose(disposing);
		}

		public override bool OnTouchEvent(MotionEvent? e)
		{
			if ((visualElementRenderer?.OnTouchEvent(e) ?? false) || base.OnTouchEvent(e))
			{
				return true;
			}

			return motionEventHelper.HandleMotionEvent(Parent, e);
		}

		protected virtual void OnElementChanged(ElementChangedEventArgs<Label?> e)
		{
			elementChangedEventManager.RaiseEvent(this, new VisualElementChangedEventArgs(e.OldElement, e.NewElement), nameof(ElementChanged));

			if (e.OldElement != null)
			{
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;
				this.MaybeRequestLayout();
			}

			if (e.NewElement != null)
			{
				this.EnsureId();

				visualElementTracker ??= new VisualElementTracker(this);

				e.NewElement.PropertyChanged += OnElementPropertyChanged;

				UpdateText();
				UpdateLineHeight();
				UpdateCharacterSpacing();
				UpdateTextDecorations();
				UpdateTransition();
				if (e.OldElement?.LineBreakMode != e.NewElement.LineBreakMode)
					UpdateLineBreakMode();
				if (e.OldElement?.HorizontalTextAlignment != e.NewElement.HorizontalTextAlignment || e.OldElement?.VerticalTextAlignment != e.NewElement.VerticalTextAlignment)
					UpdateGravity();
				if (e.OldElement?.MaxLines != e.NewElement.MaxLines)
					UpdateMaxLines();

				UpdatePadding();

				ElevationHelper.SetElevation(this, e.NewElement);
			}
		}

		protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.IsDisposed())
				return;

			elementPropertyChangedEventManager.RaiseEvent(this, e, nameof(ElementPropertyChanged));

			if (Control?.LayoutParameters == null && hasLayoutOccurred)
				return;

			if (e.PropertyName == Label.HorizontalTextAlignmentProperty.PropertyName || e.PropertyName == Label.VerticalTextAlignmentProperty.PropertyName)
				UpdateGravity();
			else if (e.PropertyName == Label.TextColorProperty.PropertyName ||
				e.PropertyName == Label.TextTypeProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == Label.FontProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == Label.LineBreakModeProperty.PropertyName)
				UpdateLineBreakMode();
			else if (e.PropertyName == Label.CharacterSpacingProperty.PropertyName)
				UpdateCharacterSpacing();
			else if (e.PropertyName == Label.TextDecorationsProperty.PropertyName)
				UpdateTextDecorations();
			else if (e.PropertyName == Label.TextProperty.PropertyName ||
				e.PropertyName == Label.FormattedTextProperty.PropertyName ||
				e.PropertyName == Label.TextTransformProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == Label.LineHeightProperty.PropertyName)
				UpdateLineHeight();
			else if (e.PropertyName == Label.MaxLinesProperty.PropertyName)
				UpdateMaxLines();
			else if (e.PropertyName == Label.PaddingProperty.PropertyName)
				UpdatePadding();
			else if (e.PropertyName == ViewSwitcher.TransitionDurationProperty.PropertyName ||
				e.PropertyName == ViewSwitcher.TransitionTypeProperty.PropertyName)
				UpdateTransition();
		}

		void UpdateColor()
		{
			if (Element == null)
				return;

			var color = Element.TextColor;
			if (color == lastUpdateColor)
				return;
			lastUpdateColor = color;

			if (color.IsDefault)
			{
				foreach (var child in children)
					child.SetTextColor(labelTextColorDefault);
			}
			else
			{
				var androidColor = color.ToAndroid();
				foreach (var child in children)
					child.SetTextColor(androidColor);
			}
		}

		void UpdateFont()
		{
			if (Element == null)
				return;

#pragma warning disable 618 // We will need to update this when .Font goes away
			var f = Element.Font;
#pragma warning restore 618

			var newTypeface = f.ToTypeface();
			if (newTypeface != lastTypeface)
			{
				foreach (var child in children)
					child.Typeface = newTypeface;

				lastTypeface = newTypeface;
			}

			var newTextSize = f.ToScaledPixel();
			if (newTextSize != lastTextSize)
			{
				foreach (var child in children)
					child.SetTextSize(ComplexUnitType.Sp, newTextSize);

				lastTextSize = newTextSize;
			}
		}

		void UpdateTextDecorations()
		{
			if (Element == null || !Element.IsSet(Label.TextDecorationsProperty))
				return;

			var textDecorations = Element.TextDecorations;

			if ((textDecorations & TextDecorations.Strikethrough) == 0)
			{
				foreach (var child in children)
					child.PaintFlags &= ~PaintFlags.StrikeThruText;
			}
			else
			{
				foreach (var child in children)
					child.PaintFlags |= PaintFlags.StrikeThruText;
			}

			if ((textDecorations & TextDecorations.Underline) == 0)
			{
				foreach (var child in children)
					child.PaintFlags &= ~PaintFlags.UnderlineText;
			}
			else
			{
				foreach (var child in children)
					child.PaintFlags |= PaintFlags.UnderlineText;
			}
		}

		void UpdateGravity()
		{
			if (Element == null)
				return;

			Label label = Element;

			var gravity = label.HorizontalTextAlignment.ToHorizontalGravityFlags() | label.VerticalTextAlignment.ToVerticalGravityFlags();
			foreach (var child in children)
				child.Gravity = gravity;

			lastSizeRequest = null;
		}

		void UpdateCharacterSpacing()
		{
			if (Element == null)
				return;

			if (XCT.SdkInt >= 21)
			{
				// 0.0624 - Coefficient for converting Pt to Em
				var characterSpacing = (float)(Element.CharacterSpacing * 0.0624);
				foreach (var child in children)
					child.LetterSpacing = characterSpacing;
			}
		}

		void UpdateLineBreakMode()
		{
			if (Element == null)
				return;

			foreach (var child in children)
				child.SetLineBreakMode(Element);

			lastSizeRequest = null;
		}

		void UpdateMaxLines()
		{
			if (Element == null)
				return;

			foreach (var child in children)
				child.SetMaxLines(Element);

			lastSizeRequest = null;
		}

		void UpdateText()
		{
			if (Element == null)
				return;

			if (NextView is not FormsTextView nextView)
				return;

			if (Element.FormattedText != null)
			{
				var formattedText = Element.FormattedText ?? Element.Text;
#pragma warning disable 618 // We will need to update this when .Font goes away
				nextView.TextFormatted = spannableString = formattedText.ToAttributed(Element.Font, Element.TextColor, nextView);
				ShowNext();
#pragma warning restore 618
				wasFormatted = true;
			}
			else
			{
				if (wasFormatted)
				{
					foreach (var child in children)
						child.SetTextColor(labelTextColorDefault);

					lastUpdateColor = Color.Default;
				}

				switch (Element.TextType)
				{
					case TextType.Html:
						if (XCT.SdkInt >= 24)
						{
							nextView.SetText(Html.FromHtml(Element.Text ?? string.Empty, FromHtmlOptions.ModeCompact), BufferType.Spannable);
							ShowNext();
						}
						else
						{
#pragma warning disable CS0618 // Type or member is obsolete
							nextView.SetText(Html.FromHtml(Element.Text ?? string.Empty), BufferType.Spannable);
#pragma warning restore CS0618 // Type or member is obsolete
							ShowNext();
						}
						break;

					default:
						nextView.Text = Element.UpdateFormsText(Element.Text, Element.TextTransform);
						ShowNext();
						break;
				}

				UpdateColor();
				UpdateFont();

				wasFormatted = false;
			}

			lastSizeRequest = null;
		}

		void UpdateLineHeight()
		{
			if (Element == null)
				return;

			if (lineSpacingExtraDefault < 0)
				lineSpacingExtraDefault = children[0].LineSpacingExtra;
			if (lineSpacingMultiplierDefault < 0)
				lineSpacingMultiplierDefault = children[0].LineSpacingMultiplier;

			if (Element.LineHeight == -1)
			{
				foreach (var child in children)
					child.SetLineSpacing(lineSpacingExtraDefault, lineSpacingMultiplierDefault);
			}
			else if (Element.LineHeight >= 0)
			{
				foreach (var child in children)
					child.SetLineSpacing(0, (float)Element.LineHeight);
			}

			lastSizeRequest = null;
		}

		void UpdatePadding()
		{
			if (Element == null)
				return;

			SetPadding(
				(int)Context.ToPixels(Element.Padding.Left),
				(int)Context.ToPixels(Element.Padding.Top),
				(int)Context.ToPixels(Element.Padding.Right),
				(int)Context.ToPixels(Element.Padding.Bottom));

			lastSizeRequest = null;
		}

		void UpdateTransition()
		{
			if (Element == null)
				return;

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

		public AView? MakeView()
			=> childrenStack.Pop();
	}
}
