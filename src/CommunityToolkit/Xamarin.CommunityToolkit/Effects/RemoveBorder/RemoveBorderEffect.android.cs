using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Runtime;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Effects = Xamarin.CommunityToolkit.Android.Effects;

[assembly: ExportEffect(typeof(Effects.RemoveBorderEffect), nameof(RemoveBorderEffect))]

namespace Xamarin.CommunityToolkit.Android.Effects
{
	public class RemoveBorderEffect : PlatformEffect
	{
		Drawable originalBackground;

		protected override void OnAttached()
		{
			originalBackground = Control.Background;

			var shape = new ShapeDrawable(new RectShape());
			shape.Paint.Color = global::Android.Graphics.Color.Transparent;
			shape.Paint.StrokeWidth = 0;
			shape.Paint.SetStyle(Paint.Style.Stroke);
			Control.Background = shape;
		}

		protected override void OnDetached() => Control.Background = originalBackground;
	}
}