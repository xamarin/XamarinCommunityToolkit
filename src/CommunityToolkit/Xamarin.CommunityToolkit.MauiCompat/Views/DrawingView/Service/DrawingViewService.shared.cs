using System.Collections.Generic;
using System.IO;
using Color = Microsoft.Maui.Graphics.Color;
using Point = Microsoft.Maui.Graphics.Point;
using Size = Microsoft.Maui.Graphics.Size;

namespace Xamarin.CommunityToolkit.UI.Views
{
#if NETSTANDARD || __WATCHOS__ || __TVOS__ || WINDOWS
	static class DrawingViewService
	{
		/// <summary>
		/// Get image stream from points
		/// </summary>
		/// <param name="points">Drawing points</param>
		/// <param name="imageSize">Image size</param>
		/// <param name="lineWidth">Line Width</param>
		/// <param name="strokeColor">Line color</param>
		/// <param name="backgroundColor">Image background color</param>
		/// <returns>Image stream</returns>
		public static Stream GetImageStream(IList<Point> points,
			Size imageSize,
			float lineWidth,
			Color strokeColor,
			Color backgroundColor) =>
			Stream.Null;

		/// <summary>
		/// Get image stream from lines
		/// </summary>
		/// <param name="points">Drawing lines</param>
		/// <param name="imageSize">Image size</param>
		/// <param name="backgroundColor">Image background color</param>
		/// <returns>Image stream</returns>
		public static Stream GetImageStream(IList<Line> lines,
			Size imageSize,
			Color backgroundColor) =>
			Stream.Null;
	}
#endif
}