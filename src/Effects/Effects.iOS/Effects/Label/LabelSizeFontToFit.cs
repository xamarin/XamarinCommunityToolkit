using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsCommunityToolkit.Effects.iOS;
using UIKit;
using Foundation;

[assembly: ExportEffect(typeof(LabelSizeFontToFit), nameof(LabelSizeFontToFit))]

namespace FormsCommunityToolkit.Effects.iOS
{
    [Preserve(AllMembers = true)]
    public class LabelSizeFontToFit : PlatformEffect
    {
        protected override void OnAttached()
        {
            var label = Control as UILabel;
            if (label == null)
                return;

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
