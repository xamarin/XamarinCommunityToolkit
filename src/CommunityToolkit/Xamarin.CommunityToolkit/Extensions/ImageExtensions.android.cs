using Android.Widget;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Extensions.Internals
{
	static class ImageExtensions
	{
		static ImageView.ScaleType? fill;
		static ImageView.ScaleType? aspectFill;
		static ImageView.ScaleType? aspectFit;

		public static ImageView.ScaleType? ToScaleType(this Aspect aspect) => aspect switch
		{
			Aspect.Fill => fill ??= ImageView.ScaleType.FitXy,
			Aspect.AspectFill => aspectFill ??= ImageView.ScaleType.CenterCrop,
			_ => aspectFit ??= ImageView.ScaleType.FitCenter,
		};
	}
}