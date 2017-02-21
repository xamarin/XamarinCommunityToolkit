using System;
using System.Linq;
using FormsCommunityToolkit.Effects.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportEffect(typeof(LabelMultiLine), nameof(LabelMultiLine))]
namespace FormsCommunityToolkit.Effects.iOS
{
    public class LabelMultiLine : PlatformEffect
    {
        private nint _initialeLines;

        protected override void OnAttached()
        {
            var control = Control as UILabel;

            if (control == null)
                return;

            _initialeLines = control.Lines;

            var effect = (FormsCommunityToolkit.Effects.LabelMultiLine)Element.Effects.FirstOrDefault(item => item is FormsCommunityToolkit.Effects.LabelMultiLine);
            if (effect != null && effect.Lines > 0)
                control.Lines = effect.Lines;
        }

        protected override void OnDetached()
        {
            var control = Control as UILabel;

            if (control == null)
                return;

            control.Lines = _initialeLines;
        }
    }
}