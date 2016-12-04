using System.Linq;
using Android.Widget;
using FormsCommunityToolkit.Effects.Droid.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(MultiLineLabelEffect), nameof(MultiLineLabelEffect))]
namespace FormsCommunityToolkit.Effects.Droid.Effects
{
    public class MultiLineLabelEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            TextView control = Control as TextView;

            if (control != null)
            {
                var effect = (FormsCommunityToolkit.Effects.MultiLineLabelEffect)Element.Effects.FirstOrDefault(item => item is FormsCommunityToolkit.Effects.MultiLineLabelEffect);
                if (effect != null && effect.Lines > 0)
                {
                    control.SetSingleLine(false);
                    control.SetLines(effect.Lines);
                }
            }
        }

        protected override void OnDetached()
        {
        }
    }
}