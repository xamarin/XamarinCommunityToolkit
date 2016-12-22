using System;
using System.Linq;
using FormsCommunityToolkit.Effects.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(MultiLineLabelEffect), nameof(MultiLineLabelEffect))]
namespace FormsCommunityToolkit.Effects.iOS
{
    public class MultiLineLabelEffect : PlatformEffect
    {
        nint initialeLines;

        protected override void OnAttached()
        {
            var control = Control as UILabel;

            if (control != null)
            {
                initialeLines = control.Lines;

                var effect = (FormsCommunityToolkit.Effects.MultiLineLabelEffect)Element.Effects.FirstOrDefault(item => item is FormsCommunityToolkit.Effects.MultiLineLabelEffect);
                if (effect != null && effect.Lines > 0)
                    control.Lines = effect.Lines;
            }
        }

        protected override void OnDetached()
        {
            var control = Control as UILabel;

            if (control != null)
                control.Lines = initialeLines;
        }
    }
}