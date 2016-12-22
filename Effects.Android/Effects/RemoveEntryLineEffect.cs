using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Runtime;
using FormsCommunityToolkit.Effects.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(RemoveEntryLineEffect), nameof(RemoveEntryLineEffect))]
namespace FormsCommunityToolkit.Effects.Droid
{
    [Preserve(AllMembers = true)]
    public class RemoveEntryLineEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            var shape = new ShapeDrawable(new RectShape());
            shape.Paint.Color = Android.Graphics.Color.Transparent;
            shape.Paint.StrokeWidth = 0;
            shape.Paint.SetStyle(Paint.Style.Stroke);
            Control.Background = shape;
        }

        protected override void OnDetached()
        {
        }
    }
}