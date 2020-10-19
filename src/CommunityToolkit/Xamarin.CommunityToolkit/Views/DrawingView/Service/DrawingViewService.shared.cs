using System.Collections.Generic;
using System.IO;
using Color = Xamarin.Forms.Color;
using Point = Xamarin.Forms.Point;
using Size = Xamarin.Forms.Size;

namespace Xamarin.CommunityToolkit.UI.Views
{
#if NETSTANDARD || __WATCHOS__ || __TVOS__
	static class DrawingViewService
	{
		public static Stream GetImageStream(IList<Point> points,
			Size imageSize,
			float lineWidth,
			Color strokeColor,
			Color backgroundColor) =>
			Stream.Null;
	}
#endif
}