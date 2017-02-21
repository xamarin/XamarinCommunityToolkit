using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsCommunityToolkit.Effects.iOS;
using UIKit;
using System.Linq;

[assembly: ExportEffect (typeof (LabelCustomFont), nameof (LabelCustomFont))]
namespace FormsCommunityToolkit.Effects.iOS
{
    public class LabelCustomFont : PlatformEffect
    {
        protected override void OnAttached ()
        {
            var control = Control as UILabel;

            if (control == null)
                return;

            var effect = (FormsCommunityToolkit.Effects.LabelCustomFont)Element.Effects.FirstOrDefault(item => item is FormsCommunityToolkit.Effects.LabelCustomFont);
            if (effect != null && !string.IsNullOrWhiteSpace(effect.FontPath))
                control.Font = UIFont.FromName(effect.FontFamilyName, control.Font.PointSize);
        }

        protected override void OnDetached ()
        {
        }
    }
}