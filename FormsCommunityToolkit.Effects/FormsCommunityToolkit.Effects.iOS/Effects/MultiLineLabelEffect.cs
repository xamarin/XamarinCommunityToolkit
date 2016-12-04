using System;
using System.Linq;
using FormsCommunityToolkit.Effects.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(MultiLineLabelEffect), nameof(MultiLineLabelEffect))]
namespace FormsCommunityToolkit.Effects.iOS.Effects
{
    public class MultiLineLabelEffect : PlatformEffect
    {
        private nint _initialeLines;

        protected override void OnAttached()
        {
            UILabel control = Control as UILabel;

            if (control != null)
            {
                _initialeLines = control.Lines;

                var effect = (FormsCommunityToolkit.Effects.MultiLineLabelEffect)Element.Effects.FirstOrDefault(item => item is FormsCommunityToolkit.Effects.MultiLineLabelEffect);
                if (effect != null && effect.Lines > 0)
                    control.Lines = effect.Lines;
            }
        }

        protected override void OnDetached()
        {
            UILabel control = Control as UILabel;

            if (control != null)
                control.Lines = _initialeLines;
        }
    }
}