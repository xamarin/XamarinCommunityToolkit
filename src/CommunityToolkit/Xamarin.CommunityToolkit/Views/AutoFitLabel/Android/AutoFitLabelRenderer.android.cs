using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Util;
using Android.Views;
using AndroidX.Core.View;
using AndroidX.Core.Widget;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.CommunityToolkit.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;
using AndroidAutoSizeTextType = Android.Widget.AutoSizeTextType;
using AView = Android.Views.View;
using Color = Xamarin.Forms.Color;
using FormsTextAlignment = Xamarin.Forms.TextAlignment;
using Size = Xamarin.Forms.Size;
using VisualElement = Xamarin.Forms.VisualElement;
using VisualElementFastRenderer = Xamarin.Forms.Platform.Android.FastRenderers.VisualElementRenderer;

[assembly: ExportRenderer(typeof(AutoFitLabel), typeof(AutoFitLabelRenderer))]

namespace Xamarin.CommunityToolkit.Views
{
	public class AutoFitLabelRenderer : FormsAppCompatTextView, IVisualElementRenderer, IViewRenderer, ITabStop
	{
		static BuildVersionCodes? sdkInt;
		static bool? isLollipopOrNewer;
		static bool? isNougatOrNewer;

		static BuildVersionCodes SdkInt =>
			sdkInt ??= Build.VERSION.SdkInt;

		static bool IsNougatOrNewer =>
			isNougatOrNewer ??= SdkInt >= BuildVersionCodes.N;

		static bool IsLollipopOrNewer =>
			isLollipopOrNewer ??= SdkInt >= BuildVersionCodes.Lollipop;

		readonly ColorStateList? labelTextColorDefault;
		readonly MotionEventHelper motionEventHelper;

		int lastConstraintHeight;
		int lastConstraintWidth;
		int? defaultLabelFor;

		float lineSpacingMultiplierDefault = -1.0f;
		float lineSpacingExtraDefault = -1.0f;
		float lastTextSize = -1f;

		bool hasLayoutOccurred;
		bool wasFormatted;
		bool disposed;

		VisualElementFastRenderer? visualElementRenderer;
		VisualElementTracker? visualElementTracker;
		SpannableString? spannableString;
		SizeRequest? lastSizeRequest;
		Typeface? lastTypeface;
		AutoFitLabel? element;

		Color lastUpdateColor = Color.Default;

		public AutoFitLabelRenderer(Context? context)
			: base(context)
		{
			motionEventHelper = new MotionEventHelper();
			labelTextColorDefault = TextColors;
			visualElementRenderer = new VisualElementFastRenderer(this);
			BackgroundManager.Init(this);
		}

		public event EventHandler<VisualElementChangedEventArgs>? ElementChanged;

		public event EventHandler<PropertyChangedEventArgs>? ElementPropertyChanged;

		VisualElement? IVisualElementRenderer.Element => Element;

		VisualElementTracker? IVisualElementRenderer.Tracker => visualElementTracker;

		AView IVisualElementRenderer.View => this;

		AView ITabStop.TabStop => this;

		ViewGroup? IVisualElementRenderer.ViewGroup => null;

		protected AutoFitLabel? Element
		{
			get => element;
			set
			{
				if (element == value)
					return;

				var oldElement = element;
				element = value;

				if (oldElement != null && element != null)
					OnElementChanged(new ElementChangedEventArgs<AutoFitLabel>(oldElement, element));
			}
		}

		protected global::Android.Widget.TextView Control => this;

		SizeRequest IVisualElementRenderer.GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			if (disposed)
				return default;

			if (lastSizeRequest.HasValue)
			{
				var canRecycleLast = widthConstraint == lastConstraintWidth && heightConstraint == lastConstraintHeight;

				if (!canRecycleLast)
				{
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

			var hint = Control.Hint;
			var setHint = Control.LayoutParameters != null;

			if (!string.IsNullOrEmpty(hint) && setHint)
				Control.Hint = string.Empty;

			Measure(widthConstraint, heightConstraint);
			var result = new SizeRequest(new Size(MeasuredWidth, MeasuredHeight), default);

			if (setHint)
				Control.Hint = hint;

			result.Minimum = new Size(Math.Min(Context.ToPixels(10), result.Request.Width), result.Request.Height);

			var measureIsChanged = !lastSizeRequest.HasValue ||
				(lastSizeRequest.HasValue && (lastSizeRequest.Value.Request.Height != MeasuredHeight || lastSizeRequest.Value.Request.Width != MeasuredWidth));

			if (measureIsChanged)
				MaybeRequestLayout();
			else
				ForceLayout();

			lastConstraintWidth = widthConstraint;
			lastConstraintHeight = heightConstraint;
			lastSizeRequest = result;

			return result;
		}

		void IVisualElementRenderer.SetElement(VisualElement element)
		{
			if (element is not AutoFitLabel label)
				throw new ArgumentException("Element must be of type Label");

			Element = label;
			motionEventHelper.UpdateElement(element);
		}

		void IVisualElementRenderer.SetLabelFor(int? id)
		{
			if (defaultLabelFor == null)
				defaultLabelFor = ViewCompat.GetLabelFor(this);

			ViewCompat.SetLabelFor(this, (int)(id ?? defaultLabelFor));
		}

		void IVisualElementRenderer.UpdateLayout()
		{
			var tracker = visualElementTracker;
			tracker?.UpdateLayout();
		}

		void IViewRenderer.MeasureExactly() =>
			MeasureExactly();

		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout(changed, left, top, right, bottom);

			if (Element is null || spannableString is null)
				return;

			RecalculateSpanPositions(Element, spannableString, new SizeRequest(new Size(right - left, bottom - top)));
			hasLayoutOccurred = true;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposed)
				return;

			disposed = true;

			if (disposing)
			{
				if (Element is not null)
					Element.PropertyChanged -= OnElementPropertyChanged;

				BackgroundManager.Dispose(this);

				if (visualElementTracker is not null)
				{
					visualElementTracker.Dispose();
					visualElementTracker = null;
				}

				if (visualElementRenderer is not null)
				{
					visualElementRenderer.Dispose();
					visualElementRenderer = null;
				}

				spannableString?.Dispose();

				if (Element is not null)
				{
					if (Platform.GetRenderer(Element) == this)
						Platform.ClearRenderer(null);
				}
			}

			base.Dispose(disposing);
		}

		public override bool OnTouchEvent(MotionEvent? e)
		{
			if (visualElementRenderer?.OnTouchEvent(e) == true || base.OnTouchEvent(e))
				return true;

			return motionEventHelper.HandleMotionEvent(Parent, e);
		}

		protected virtual void OnElementChanged(ElementChangedEventArgs<AutoFitLabel>? e)
		{
			ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(e?.OldElement, e?.NewElement));

			if (e?.OldElement is not null)
			{
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;
				MaybeRequestLayout();
			}

			if (e?.NewElement is not null)
			{
				this.EnsureId();

				if (visualElementTracker is null)
					visualElementTracker = new VisualElementTracker(this);

				e.NewElement.PropertyChanged += OnElementPropertyChanged;

				UpdateText();
				UpdateLineHeight();
				UpdateCharacterSpacing();
				UpdateTextDecorations();

				if (e.OldElement?.LineBreakMode != e.NewElement.LineBreakMode)
					UpdateLineBreakMode();
				if (e.OldElement?.HorizontalTextAlignment != e.NewElement.HorizontalTextAlignment || e.OldElement?.VerticalTextAlignment != e.NewElement.VerticalTextAlignment)
					UpdateGravity();
				if (e.OldElement?.MaxLines != e.NewElement.MaxLines)
					UpdateMaxLines();

				UpdatePadding();
				UpdateAutoFitMode();

				ElevationHelper.SetElevation(this, e.NewElement);
			}
		}

		protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.IsDisposed())
				return;

			ElementPropertyChanged?.Invoke(this, e);

			if (Control?.LayoutParameters is null && hasLayoutOccurred)
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
			else if (e.PropertyName == Label.TextProperty.PropertyName || e.PropertyName == Label.FormattedTextProperty.PropertyName)
				UpdateText();
			else if (e.PropertyName == Label.LineHeightProperty.PropertyName)
				UpdateLineHeight();
			else if (e.PropertyName == Label.MaxLinesProperty.PropertyName)
				UpdateMaxLines();
			else if (e.PropertyName == Label.PaddingProperty.PropertyName)
				UpdatePadding();
			else if (e.PropertyName == AutoFitLabel.AutoFitModeProperty.PropertyName)
				UpdateAutoFitMode();
		}

		void UpdateColor()
		{
			var color = Element?.TextColor;
			if (color == lastUpdateColor || color is null)
				return;

			lastUpdateColor = color.Value;

			if (lastUpdateColor.IsDefault)
				SetTextColor(labelTextColorDefault);
			else
				SetTextColor(lastUpdateColor.ToAndroid());
		}

		void UpdateFont()
		{
			if (Element is null)
				return;

#pragma warning disable 618 // We will need to update this when .Font goes away
			var font = Element.Font;
#pragma warning restore 618

			var newTypeface = font.ToTypeface();
			if (newTypeface != lastTypeface)
			{
				Typeface = newTypeface;
				lastTypeface = newTypeface;
			}

			var newTextSize = font.ToScaledPixel();
			if (newTextSize != lastTextSize)
			{
				SetTextSize(ComplexUnitType.Sp, newTextSize);
				lastTextSize = newTextSize;
			}
		}

		void UpdateTextDecorations()
		{
			if (Element is null)
				return;

			if (!Element.IsSet(Label.TextDecorationsProperty))
				return;

			var textDecorations = Element.TextDecorations;

			if ((textDecorations & TextDecorations.Strikethrough) == 0)
				PaintFlags &= ~PaintFlags.StrikeThruText;
			else
				PaintFlags |= PaintFlags.StrikeThruText;

			if ((textDecorations & TextDecorations.Underline) == 0)
				PaintFlags &= ~PaintFlags.UnderlineText;
			else
				PaintFlags |= PaintFlags.UnderlineText;
		}

		void UpdateGravity()
		{
			Gravity = GetHorizontalGravityFlags() | GetVerticalGravityFlags();
			lastSizeRequest = null;
		}

		void UpdateCharacterSpacing()
		{
			if (IsLollipopOrNewer && Element is not null)
				LetterSpacing = (float)Element.CharacterSpacing * 0.0624f;
		}

		void UpdateLineBreakMode()
		{
			if (Element is null)
				return;

			SetLineBreakMode(Element);
			lastSizeRequest = null;
		}

		void UpdateMaxLines()
		{
			if (Element is null)
				return;

			SetMaxLines(Element);
			lastSizeRequest = null;
		}

		void UpdateText()
		{
			if (Element is null)
				return;

			if (Element.FormattedText != null)
			{
				var formattedText = Element.FormattedText ?? Element.Text;
#pragma warning disable 618 // We will need to update this when .Font goes away
				TextFormatted = spannableString = formattedText.ToAttributed(Element.Font, Element.TextColor, this);
#pragma warning restore 618
				wasFormatted = true;
			}
			else
			{
				if (wasFormatted)
				{
					SetTextColor(labelTextColorDefault);
					lastUpdateColor = Color.Default;
				}

				switch (Element.TextType)
				{
					case TextType.Html:
						if (IsNougatOrNewer)
							Control.SetText(Html.FromHtml(Element.Text ?? string.Empty, FromHtmlOptions.ModeCompact), BufferType.Spannable);
						else
#pragma warning disable CS0618 // Type or member is obsolete
							Control.SetText(Html.FromHtml(Element.Text ?? string.Empty), BufferType.Spannable);
#pragma warning restore CS0618 // Type or member is obsolete
						break;

					default:
						Text = Element.Text;
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
			if (Element is null)
				return;

			if (lineSpacingExtraDefault < 0)
				lineSpacingExtraDefault = LineSpacingExtra;
			if (lineSpacingMultiplierDefault < 0)
				lineSpacingMultiplierDefault = LineSpacingMultiplier;

			if (Element.LineHeight == -1)
				SetLineSpacing(lineSpacingExtraDefault, lineSpacingMultiplierDefault);
			else if (Element.LineHeight >= 0)
				SetLineSpacing(0, (float)Element.LineHeight);

			lastSizeRequest = null;
		}

		void UpdatePadding()
		{
			if (Element is null)
				return;

			SetPadding(
				(int)Context.ToPixels(Element.Padding.Left),
				(int)Context.ToPixels(Element.Padding.Top),
				(int)Context.ToPixels(Element.Padding.Right),
				(int)Context.ToPixels(Element.Padding.Bottom));

			lastSizeRequest = null;
		}

		void UpdateAutoFitMode()
		{
			if (Element is null)
				return;

			SetAutoFitMode(Element);
		}

		void MaybeRequestLayout()
		{
			var isInLayout = false;
			if ((int)Build.VERSION.SdkInt >= 18)
				isInLayout = IsInLayout;

			if (!isInLayout && !IsLayoutRequested)
				RequestLayout();
		}

		GravityFlags GetHorizontalGravityFlags()
		{
			var alignment = Element?.HorizontalTextAlignment ?? FormsTextAlignment.Start;
			return alignment switch
			{
				FormsTextAlignment.Center => GravityFlags.CenterHorizontal,
				FormsTextAlignment.End => GravityFlags.End,
				_ => GravityFlags.Start,
			};
		}

		GravityFlags GetVerticalGravityFlags()
		{
			var alignment = Element?.VerticalTextAlignment ?? FormsTextAlignment.Start;
			return alignment switch
			{
				FormsTextAlignment.Start => GravityFlags.Top,
				FormsTextAlignment.End => GravityFlags.Bottom,
				_ => GravityFlags.CenterVertical,
			};
		}

		void MeasureExactly()
		{
			if (Control is null || Element is null)
				return;

			var width = Element.Width;
			var height = Element.Height;

			if (width <= 0 || height <= 0)
				return;

			var realWidth = (int)Context.ToPixels(width);
			var realHeight = (int)Context.ToPixels(height);

			var widthMeasureSpec = MeasureSpecFactory.MakeMeasureSpec(realWidth, MeasureSpecMode.Exactly);
			var heightMeasureSpec = MeasureSpecFactory.MakeMeasureSpec(realHeight, MeasureSpecMode.Exactly);

			Control.Measure(widthMeasureSpec, heightMeasureSpec);
		}

		public void RecalculateSpanPositions(Label element, SpannableString spannableString, SizeRequest finalSize)
		{
			if (element?.FormattedText?.Spans == null || element.FormattedText.Spans.Count == 0)
				return;

			var labelWidth = finalSize.Request.Width;
			if (labelWidth <= 0 || finalSize.Request.Height <= 0)
				return;

			if (spannableString == null || spannableString.IsDisposed())
				return;

			var layout = Layout;
			if (layout == null)
				return;

			var next = 0;
			var count = 0;
			var totalLineHeights = new List<int>();

			for (var i = 0; i < spannableString.Length(); i = next)
			{
				var type = Java.Lang.Class.FromType(typeof(Java.Lang.Object));

				var span = element.FormattedText.Spans[count];

				count++;

				if (string.IsNullOrEmpty(span.Text))
					continue;

				// find the next span
				next = spannableString.NextSpanTransition(i, spannableString.Length(), type);

				// get all spans in the range - Android can have overlapping spans
				var spans = spannableString.GetSpans(i, next, type);

				if (spans is null)
					continue;

				var startSpan = spans[0];
				var endSpan = spans[^1];

				var startSpanOffset = spannableString.GetSpanStart(startSpan);
				var endSpanOffset = spannableString.GetSpanEnd(endSpan);

				var thisLine = layout.GetLineForOffset(endSpanOffset);
				var lineStart = layout.GetLineStart(thisLine);
				var lineEnd = layout.GetLineEnd(thisLine);

				// If this is true, endSpanOffset has the value for another line that belong to the next span and not it self.
				// So it should be rearranged to value not pass the lineEnd.
				if (endSpanOffset > (lineEnd - lineStart))
					endSpanOffset = lineEnd;

				var startX = layout.GetPrimaryHorizontal(startSpanOffset);
				var endX = layout.GetPrimaryHorizontal(endSpanOffset);

				var startLine = layout.GetLineForOffset(startSpanOffset);
				var endLine = layout.GetLineForOffset(endSpanOffset);

				var lineHeights = new double[endLine - startLine + 1];

				// calculate all the different line heights
				for (var lineCount = startLine; lineCount <= endLine; lineCount++)
				{
					var lineHeight = layout.GetLineBottom(lineCount) - layout.GetLineTop(lineCount);
					lineHeights[lineCount - startLine] = lineHeight;

					if (totalLineHeights.Count <= lineCount)
						totalLineHeights.Add(lineHeight);
				}

				var yaxis = 0.0;
				for (var line = startLine; line > 0; line--)
					yaxis += totalLineHeights[line];

				((ISpatialElement)span).Region = Forms.Region.FromLines(lineHeights, labelWidth, startX, endX, yaxis).Inflate(10);
			}
		}

		void SetLineBreakMode(Label label)
		{
			var lineBreakMode = label.LineBreakMode;
			var maxLines = int.MaxValue;
			var singleLine = false;

			switch (lineBreakMode)
			{
				case LineBreakMode.NoWrap:
					maxLines = 1;
					Ellipsize = null;
					break;
				case LineBreakMode.WordWrap:
					Ellipsize = null;
					break;
				case LineBreakMode.CharacterWrap:
					Ellipsize = null;
					break;
				case LineBreakMode.HeadTruncation:
					maxLines = 1;
					singleLine = true; // Workaround for bug in older Android API versions (https://bugzilla.xamarin.com/show_bug.cgi?id=49069)
					Ellipsize = TextUtils.TruncateAt.Start;
					break;
				case LineBreakMode.TailTruncation:
					maxLines = 1;
					Ellipsize = TextUtils.TruncateAt.End;
					break;
				case LineBreakMode.MiddleTruncation:
					maxLines = 1;
					singleLine = true; // Workaround for bug in older Android API versions (https://bugzilla.xamarin.com/show_bug.cgi?id=49069)
					Ellipsize = TextUtils.TruncateAt.Middle;
					break;
			}

			SetSingleLine(singleLine);
			SetMaxLines(label, maxLines);
		}

		void SetMaxLines(Label label, int lines)
		{
			// If the Label's MaxLines has been explicitly set, we should not set it here
			if (label.MaxLines != (int)Label.MaxLinesProperty.DefaultValue)
				return;

			SetMaxLines(lines);
		}

		void SetMaxLines(Label label)
		{
			var maxLines = label.MaxLines;

			if (maxLines == (int)Label.MaxLinesProperty.DefaultValue)
			{
				// MaxLines is not explicitly set, so just let it be whatever gets set by LineBreakMode
				SetLineBreakMode(label);
				return;
			}

			SetMaxLines(maxLines);
		}

		void SetAutoFitMode(AutoFitLabel label)
		{
			if ((int)SdkInt < 14)
				return;

			switch (label.AutoFitMode)
			{
				case AutoFitTextMode.FitToWidth:
					label.SetValueFromRenderer(Label.MaxLinesProperty, 1);
					TextViewCompat.SetAutoSizeTextTypeUniformWithConfiguration(
						this, label.MinAutoFitFontSize, label.MaxAutoFitFontSize, 1, (int)ComplexUnitType.Sp);
					break;
				case AutoFitTextMode.None:
				default:
					TextViewCompat.SetAutoSizeTextTypeWithDefaults(this, (int)AndroidAutoSizeTextType.None);
					label.SetValueFromRenderer(Label.LineBreakModeProperty, label.LineBreakMode);

#pragma warning disable 618 // We will need to update this when .Font goes away
					var f = label.Font;
#pragma warning restore 618

					var newTextSize = f.ToScaledPixel();
					SetTextSize(ComplexUnitType.Sp, newTextSize);
					break;
			}
		}
	}

	static class BackgroundManager
	{
		public static void Init(IVisualElementRenderer renderer)
		{
			_ = renderer ?? throw new ArgumentNullException($"{nameof(BackgroundManager)}.{nameof(Init)} {nameof(renderer)} cannot be null");

			renderer.ElementPropertyChanged += OnElementPropertyChanged;
			renderer.ElementChanged += OnElementChanged;
		}

		public static void Dispose(IVisualElementRenderer renderer)
		{
			_ = renderer ?? throw new ArgumentNullException($"{nameof(BackgroundManager)}.{nameof(Init)} {nameof(renderer)} cannot be null");

			renderer.ElementPropertyChanged -= OnElementPropertyChanged;
			renderer.ElementChanged -= OnElementChanged;

			if (renderer.Element != null)
			{
				renderer.Element.PropertyChanged -= OnElementPropertyChanged;
			}
		}

		static void UpdateBackgroundColor(AView control, VisualElement element, Color? color = null)
		{
			if (element == null || control == null)
				return;

			var finalColor = color ?? element.BackgroundColor;
			if (finalColor.IsDefault)
				control.SetBackground(null);
			else
				control.SetBackgroundColor(finalColor.ToAndroid());
		}

		static void OnElementChanged(object sender, VisualElementChangedEventArgs e)
		{
			Performance.Start(out var reference);
			if (e.OldElement != null)
			{
				e.OldElement.PropertyChanged -= OnElementPropertyChanged;
			}

			if (e.NewElement != null)
			{
				var renderer = sender as IVisualElementRenderer;
				var control = renderer?.View;
				var element = renderer?.Element;
				e.NewElement.PropertyChanged += OnElementPropertyChanged;

				if (control != null && element != null)
					UpdateBackgroundColor(control, element);
			}

			Performance.Stop(reference);
		}

		static void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
			{
				var renderer = sender as IVisualElementRenderer;

				var control = renderer?.View;
				var element = renderer?.Element;

				if (control != null && element != null)
					UpdateBackgroundColor(control, element);
			}
		}
	}
}
