using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PlatformEffects = Xamarin.Toolkit.Effects.iOS;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.LabelSizeFontToFit), nameof(RoutingEffects.LabelSizeFontToFit))]

namespace Xamarin.Toolkit.Effects.iOS
{
    [Preserve(AllMembers = true)]
    public class LabelSizeFontToFit : PlatformEffect
    {
        protected override void OnAttached()
        {
            var label = Control as UILabel;
            if (label == null)
            {
                return;
            }

            label.AdjustsFontSizeToFitWidth = true;
            label.Lines = 1;
            label.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
            label.LineBreakMode = UILineBreakMode.Clip;
        }

        protected override void OnDetached()
        {
        }
    }
}
