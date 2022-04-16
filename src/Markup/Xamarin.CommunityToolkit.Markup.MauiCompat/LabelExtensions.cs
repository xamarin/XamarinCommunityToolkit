using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Markup
{
	public static class LabelExtensions
	{
		public static TLabel TextStart<TLabel>(this TLabel label) where TLabel : Label
		{ label.HorizontalTextAlignment = TextAlignment.Start; return label; }

		public static TLabel TextCenterHorizontal<TLabel>(this TLabel label) where TLabel : Label
		{ label.HorizontalTextAlignment = TextAlignment.Center; return label; }

		public static TLabel TextEnd<TLabel>(this TLabel label) where TLabel : Label
		{ label.HorizontalTextAlignment = TextAlignment.End; return label; }

		public static TLabel TextTop<TLabel>(this TLabel label) where TLabel : Label
		{ label.VerticalTextAlignment = TextAlignment.Start; return label; }

		public static TLabel TextCenterVertical<TLabel>(this TLabel label) where TLabel : Label
		{ label.VerticalTextAlignment = TextAlignment.Center; return label; }

		public static TLabel TextBottom<TLabel>(this TLabel label) where TLabel : Label
		{ label.VerticalTextAlignment = TextAlignment.End; return label; }

		public static TLabel TextCenter<TLabel>(this TLabel label) where TLabel : Label
			=> label.TextCenterHorizontal().TextCenterVertical();

		public static TLabel FormattedText<TLabel>(this TLabel label, params Span[] spans) where TLabel : Label
		{
			label.FormattedText = new FormattedString();
			foreach (var span in spans)
				label.FormattedText.Spans.Add(span);
			return label;
		}
	}

#pragma warning disable SA1403 // File may only contain a single namespace

	// The extensions in these sub-namespaces are designed to be used together with the extensions in the parent namespace.
	// Keep them in a single file for better maintainability
	namespace LeftToRight
	{
		public static class LabelExtensions
		{
			public static TLabel TextLeft<TLabel>(this TLabel label) where TLabel : Label
			{ label.HorizontalTextAlignment = TextAlignment.Start; return label; }

			public static TLabel TextRight<TLabel>(this TLabel label) where TLabel : Label
			{ label.HorizontalTextAlignment = TextAlignment.End; return label; }
		}
	}

	namespace RightToLeft
	{
		public static class LabelExtensions
		{
			public static TLabel TextLeft<TLabel>(this TLabel label) where TLabel : Label
			{ label.HorizontalTextAlignment = TextAlignment.End; return label; }

			public static TLabel TextRight<TLabel>(this TLabel label) where TLabel : Label
			{ label.HorizontalTextAlignment = TextAlignment.Start; return label; }
		}
	}
#pragma warning restore SA1403 // File may only contain a single namespace
}