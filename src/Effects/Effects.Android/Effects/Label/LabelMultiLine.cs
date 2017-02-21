using System.Linq;
using Android.Widget;
using FormsCommunityToolkit.Effects.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(LabelMultiLine), nameof(LabelMultiLine))]
namespace FormsCommunityToolkit.Effects.Droid
{
    public class LabelMultiLine : PlatformEffect
    {
        protected override void OnAttached()
        {
            var control = Control as TextView;

            if (control == null)
                return;

            var effect = (FormsCommunityToolkit.Effects.LabelMultiLine)Element.Effects.FirstOrDefault(item => item is FormsCommunityToolkit.Effects.LabelMultiLine);
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