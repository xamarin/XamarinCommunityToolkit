using System;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PlatformEffects = Xamarin.Toolkit.Effects.iOS;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.LabelMultiLine), nameof(RoutingEffects.LabelMultiLine))]
namespace Xamarin.Toolkit.Effects.iOS
{
    public class LabelMultiLine : PlatformEffect
    {
        private nint _initialeLines;

        protected override void OnAttached()
        {
            var control = Control as UILabel;

            if (control == null)
            {
                return;
            }

            _initialeLines = control.Lines;

            var effect = (RoutingEffects.LabelMultiLine)Element.Effects.FirstOrDefault(item => item is RoutingEffects.LabelMultiLine);
            if (effect != null && effect.Lines > 0)
            {
                control.Lines = effect.Lines;
            }
        }

        protected override void OnDetached()
        {
            var control = Control as UILabel;

            if (control == null)
            {
                return;
            }

            control.Lines = _initialeLines;
        }
    }
}