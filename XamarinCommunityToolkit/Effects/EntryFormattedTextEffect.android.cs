using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Android.Text;
using Android.Text.Style;
using Android.Widget;
using Xamarin.CommunityToolkit.Android.Effects;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName(nameof(Xamarin.CommunityToolkit))]
[assembly: ExportEffect(typeof(EntryFormattedTextEffect), nameof(EntryFormattedTextEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	/// <summary>
	/// The FormattedText <see cref="PlatformEffect"/> specific
	/// to <see cref="Entry"/> controls.
	/// </summary>
	public class EntryFormattedTextEffect : PlatformEffect, IDisposable
	{
		bool isDisposed;

		protected IList<FormattedTextSegment<Java.Lang.Object>> Segments { get; set; }

		/// <summary>
		/// Gets the native <see cref="FormsEditText"/> control.
		/// </summary>
		public new FormsEditText Control => (FormsEditText)base.Control;

		/// <summary>
		/// Gets the Xamarin.Forms <see cref="Entry"/> element.
		/// </summary>
		public new Entry Element => (Entry)base.Element;

		/// <inheritdoc />
		protected override void OnAttached()
		{
			UpdateFormattedText();

			Control.TextChanged += OnTextChanged;
		}

		/// <inheritdoc />
		protected override void OnDetached()
		{
		}

		/// <inheritdoc />
		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);

			if (args.PropertyName == FormattedTextEffect.FormattedTextProperty.PropertyName)
			{
				UpdateFormattedText();
			}
		}

		void OnTextChanged(object sender, global::Android.Text.TextChangedEventArgs args)
		{
			var spannable = args.Text as SpannableStringBuilder;
			if (spannable == null || Segments.Count == 0)
			{
				return;
			}

			var segment = Segments[args.Start];
			var length = args.AfterCount - args.BeforeCount;
			var startIndex = args.Start - segment.StartIndex;

			if (length > 0)
			{
				segment.Span.Text = segment.Span.Text.Insert(startIndex, Control.Text.Substring(args.Start, length));

				for (var index = 0; index < length; index++)
				{
					Segments.Insert(args.Start + index, segment);
				}
			}
			else if (length < 0)
			{
				var absLength = Math.Abs(length);
				segment.Span.Text = segment.Span.Text.Remove(startIndex, Math.Abs(length));

				for (var index = 0; index < absLength; index++)
				{
					Segments.RemoveAt(args.Start + index);
				}
			}

			var position = 0;
			foreach (var item in Segments.Distinct())
			{
				item.StartIndex = position;
				position += item.Span.Text.Length;
			}

			foreach (var nativeSpan in segment.NativeSpans)
			{
				spannable.RemoveSpan(nativeSpan);
				spannable.SetSpan(nativeSpan, segment.StartIndex, segment.StartIndex + segment.Span.Text.Length, SpanTypes.ExclusiveExclusive);
			}
		}

		/// <summary>
		/// Updates the native <see cref="FormsEditText"/> with the
		/// Text or FormattedText. If the FormattedText is null the
		/// Control defaults to using Text property. Otherwise it uses
		/// the custom Spans in FormattedText.
		/// </summary>
		void UpdateFormattedText()
		{
			var selectionStart = Control.SelectionStart;
			var selectionEnd = Control.SelectionEnd;
			var formattedText = FormattedTextEffect.GetFormattedText(Element);

			if (formattedText == null)
			{
				Control.Text = Element.Text;
			}
			else
			{
				var spannableStringBuilder = new SpannableStringBuilder();
				Segments = new List<FormattedTextSegment<Java.Lang.Object>>();
				var position = 0;
				for (var index = 0; index < formattedText.Spans.Count; index++)
				{
					var currentSpan = formattedText.Spans[index];
					spannableStringBuilder.Append(currentSpan.Text);

					var segment = new FormattedTextSegment<Java.Lang.Object>
					{
						StartIndex = position,
						Span = currentSpan,
						NativeSpans = CreateNativeSpan(currentSpan)
					};

					for (var textIndex = position; textIndex < (currentSpan.Text.Length + position); textIndex++)
						Segments.Add(segment);

					foreach (var nativeSpan in segment.NativeSpans)
					{
						spannableStringBuilder.SetSpan(nativeSpan, position, position + currentSpan.Text.Length, SpanTypes.ExclusiveExclusive);
					}

					position += currentSpan.Text.Length;
				}

				Control.SetText(spannableStringBuilder, TextView.BufferType.Spannable);
			}

			Control.SetSelection(selectionStart, selectionEnd);
		}

		/// <summary>
		/// Create a native Android Span that is used
		/// with the <see cref="Spannable"/> object.
		/// </summary>
		/// <param name="span">
		/// The Xamarin.Forms <see cref="Span"/> to convert to a native span.
		/// </param>
		/// <returns>
		/// The native Android Span.
		/// </returns>
		/// <remarks>
		/// The native Android <see cref="Spannable"/> accepts <see cref="Java.Lang.Object"/>
		/// which allows several different types. Please refer to the Android Documentation
		/// on usage. https://developer.android.com/reference/android/text/Spannable.
		/// </remarks>
		protected virtual Java.Lang.Object[] CreateNativeSpan(Span span)
		{
			var nativeSpans = new List<Java.Lang.Object>();

			if (span.IsSet(Span.TextColorProperty))
			{
				nativeSpans.Add(new ForegroundColorSpan(span.TextColor.ToAndroid()));
			}

			if (span.IsSet(Span.BackgroundColorProperty))
			{
				nativeSpans.Add(new BackgroundColorSpan(span.BackgroundColor.ToAndroid()));
			}

			if (span.IsSet(Span.FontAttributesProperty) && span.FontAttributes != FontAttributes.None)
			{
				if (span.FontAttributes.HasFlag(FontAttributes.Italic) && span.FontAttributes.HasFlag(FontAttributes.Bold))
				{
					nativeSpans.Add(new StyleSpan(global::Android.Graphics.TypefaceStyle.Bold));
				}
				else if (span.FontAttributes.HasFlag(FontAttributes.Italic))
				{
					nativeSpans.Add(new StyleSpan(global::Android.Graphics.TypefaceStyle.Italic));
				}
				else if (span.FontAttributes.HasFlag(FontAttributes.Bold))
				{
					nativeSpans.Add(new StyleSpan(global::Android.Graphics.TypefaceStyle.Bold));
				}
			}

			if (span.IsSet(Span.FontProperty))
			{
				nativeSpans.Add(new TypefaceSpan(span.FontFamily));
			}

			if (span.IsSet(Span.FontSizeProperty))
			{
				nativeSpans.Add(new RelativeSizeSpan((float)span.FontSize));
			}

			if (span.IsSet(Span.TextDecorationsProperty) && span.TextDecorations != TextDecorations.None)
			{
				if (span.TextDecorations.HasFlag(TextDecorations.Underline))
				{
					nativeSpans.Add(new UnderlineSpan());
				}

				if (span.TextDecorations.HasFlag(TextDecorations.Strikethrough))
				{
					nativeSpans.Add(new StrikethroughSpan());
				}
			}

			if (span.IsSet(Span.LineHeightProperty))
			{
				// only for android 10.0
				//nativeSpans.Add(new LineHeightSpanStandard((int)span.LineHeight));
			}

			return nativeSpans.ToArray();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (isDisposed)
			{
				return;
			}

			if (disposing)
			{
				if (Control != null)
				{
					Control.TextChanged -= OnTextChanged;
				}
			}

			isDisposed = true;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
