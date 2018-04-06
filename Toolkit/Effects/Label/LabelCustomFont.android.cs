using System.Linq;
using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PlatformEffects = Xamarin.Toolkit.Effects.Droid;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.LabelCustomFont), nameof(RoutingEffects.LabelCustomFont))]
namespace Xamarin.Toolkit.Effects.Droid
{
    public class LabelCustomFont : PlatformEffect
    {
        protected override void OnAttached()
        {
            var control = Control as TextView;

            if (control == null)
                return;

            var effect = (RoutingEffects.LabelCustomFont)Element.Effects.FirstOrDefault(item => item is RoutingEffects.LabelCustomFont);
            if (effect != null && !string.IsNullOrWhiteSpace(effect.FontPath))
            {
#pragma warning disable CS0618 // Type or member is obsolete
                var font = Typeface.CreateFromAsset(Forms.Forms.Context.Assets, effect.FontPath);
#pragma warning restore CS0618 // Type or member is obsolete
                control.Typeface = font;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}
