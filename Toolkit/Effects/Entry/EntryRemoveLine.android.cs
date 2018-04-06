using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Runtime;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PlatformEffects = Xamarin.Toolkit.Effects.Droid;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.EntryRemoveLine), nameof(RoutingEffects.EntryRemoveLine))]
namespace Xamarin.Toolkit.Effects.Droid
{
    [Preserve(AllMembers = true)]
    public class EntryRemoveLine : PlatformEffect
    {
        protected override void OnAttached()
        {
            var shape = new ShapeDrawable(new RectShape());
            shape.Paint.Color = global::Android.Graphics.Color.Transparent;
            shape.Paint.StrokeWidth = 0;
            shape.Paint.SetStyle(Paint.Style.Stroke);
            Control.Background = shape;
        }

        protected override void OnDetached()
        {
        }
    }
}
