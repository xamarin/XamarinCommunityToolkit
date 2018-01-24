using System.Linq;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using RoutingEffects = XamarinCommunityToolkit.Effects;
using PlatformEffects = XamarinCommunityToolkit.Effects.Droid;

[assembly: ExportEffect(typeof(PlatformEffects.LabelMultiLine), nameof(RoutingEffects.LabelMultiLine))]
namespace XamarinCommunityToolkit.Effects.Droid
{
    public class LabelMultiLine : PlatformEffect
    {
        protected override void OnAttached()
        {
            var control = Control as TextView;

            if (control == null)
                return;

            var effect = (RoutingEffects.LabelMultiLine)Element.Effects.FirstOrDefault(item => item is RoutingEffects.LabelMultiLine);
            if (effect != null && effect.Lines > 0)
            {
                control.SetSingleLine(false);
                control.SetLines(effect.Lines);
            }
        }

        protected override void OnDetached()
        {
            //TODO: Glenn - Reset to old amount of Lines and old SingleLine value
        }
    }
}