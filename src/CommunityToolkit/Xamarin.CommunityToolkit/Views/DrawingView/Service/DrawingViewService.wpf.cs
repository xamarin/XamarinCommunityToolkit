using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Xamarin.Forms.Platform.WPF;
using Color = Xamarin.Forms.Color;
using Point = Xamarin.Forms.Point;
using Size = Xamarin.Forms.Size;

namespace Xamarin.CommunityToolkit.UI.Views
{
	static class DrawingViewService
	{
		public static Stream GetImageStream(IList<Point> points,
			Size imageSize,
			float lineWidth,
			Color strokeColor,
			Color backgroundColor) => Stream.Null;
	}
}