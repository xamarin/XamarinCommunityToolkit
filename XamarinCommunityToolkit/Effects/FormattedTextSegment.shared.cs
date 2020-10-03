using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Effects
{
	public class FormattedTextSegment<T>
	{
		public int StartIndex { get; set; }

		public Span Span { get; set; }

		public T[] NativeSpans { get; set; }
	}
}
