﻿using System.Collections.Generic;
using Android.Text;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

// Copied from Xamarin.Forms (TextViewExtensions)
namespace Xamarin.CommunityToolkit.Extensions.Internals
{
	static class TextViewExtensions
	{
		public static void SetMaxLines(this TextView textView, Label label)
		{
			var maxLines = label.MaxLines;

			if (maxLines == (int)Label.MaxLinesProperty.DefaultValue)
			{
				// MaxLines is not explicitly set, so just let it be whatever gets set by LineBreakMode
				textView.SetLineBreakMode(label);
				return;
			}

			textView.SetMaxLines(maxLines);
		}

		static void SetMaxLines(this TextView textView, Label label, int lines)
		{
			// If the Label's MaxLines has been explicitly set, we should not set it here
			if (label.MaxLines != (int)Label.MaxLinesProperty.DefaultValue)
			{
				return;
			}

			textView.SetMaxLines(lines);
		}

		public static void SetLineBreakMode(this TextView textView, Label label)
		{
			var lineBreakMode = label.LineBreakMode;

			var maxLines = int.MaxValue;
			var singleLine = false;

			switch (lineBreakMode)
			{
				case LineBreakMode.NoWrap:
					maxLines = 1;
					textView.Ellipsize = null;
					break;
				case LineBreakMode.WordWrap:
					textView.Ellipsize = null;
					break;
				case LineBreakMode.CharacterWrap:
					textView.Ellipsize = null;
					break;
				case LineBreakMode.HeadTruncation:
					maxLines = 1;
					singleLine = true; // Workaround for bug in older Android API versions (https://bugzilla.xamarin.com/show_bug.cgi?id=49069)
					textView.Ellipsize = TextUtils.TruncateAt.Start;
					break;
				case LineBreakMode.TailTruncation:
					maxLines = 1;
					textView.Ellipsize = TextUtils.TruncateAt.End;
					break;
				case LineBreakMode.MiddleTruncation:
					maxLines = 1;
					singleLine = true; // Workaround for bug in older Android API versions (https://bugzilla.xamarin.com/show_bug.cgi?id=49069)
					textView.Ellipsize = TextUtils.TruncateAt.Middle;
					break;
			}

			textView.SetSingleLine(singleLine);
			textView.SetMaxLines(label, maxLines);
		}

		public static void RecalculateSpanPositions(this TextView textView, Label? element, SpannableString? spannableString, SizeRequest finalSize)
		{
			if (element?.FormattedText?.Spans == null || element.FormattedText.Spans.Count == 0)
				return;

			var labelWidth = finalSize.Request.Width;
			if (labelWidth <= 0 || finalSize.Request.Height <= 0)
				return;

			if (spannableString == null || spannableString.IsDisposed())
				return;

			var layout = textView.Layout;
			if (layout == null)
				return;

			var next = 0;
			var count = 0;
			IList<int> totalLineHeights = new List<int>();

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
				if (spans == null)
					break;

				var startSpan = spans[0];
				var endSpan = spans[spans.Length - 1];

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

				((ISpatialElement)span).Region = Region.FromLines(lineHeights, labelWidth, startX, endX, yaxis).Inflate(10);
			}
		}
	}
}