using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using System.Linq;
using RoutingEffects = FormsCommunityToolkit.Effects;
using PlatformEffects = FormsCommunityToolkit.Effects.iOS;

[assembly: ExportEffect (typeof (PlatformEffects.LabelCustomFont), nameof (RoutingEffects.LabelCustomFont))]
namespace FormsCommunityToolkit.Effects.iOS
{
    public class LabelCustomFont : PlatformEffect
    {
        protected override void OnAttached ()
        {
            var control = Control as UILabel;

            if (control == null)
                return;

            var effect = (RoutingEffects.LabelCustomFont)Element.Effects.FirstOrDefault(item => item is RoutingEffects.LabelCustomFont);
            if (effect != null && !string.IsNullOrWhiteSpace(effect.FontPath))
                control.Font = UIFont.FromName(effect.FontFamilyName, control.Font.PointSize);
        }

        protected override void OnDetached ()
        {
        }
    }
}