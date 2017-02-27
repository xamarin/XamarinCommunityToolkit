using System.Linq;
using Android.Graphics;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using RoutingEffects = FormsCommunityToolkit.Effects;
using PlatformEffects = FormsCommunityToolkit.Effects.Droid;

[assembly: ExportEffect(typeof(PlatformEffects.LabelCustomFont), nameof(RoutingEffects.LabelCustomFont))]
namespace FormsCommunityToolkit.Effects.Droid
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
                var font = Typeface.CreateFromAsset(Forms.Context.Assets, effect.FontPath);
                control.Typeface = font;
            }
        }

        protected override void OnDetached()
        {
        }
    }
}