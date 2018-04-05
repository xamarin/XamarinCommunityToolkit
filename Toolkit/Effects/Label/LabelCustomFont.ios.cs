using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PlatformEffects = Xamarin.Toolkit.Effects.iOS;
using RoutingEffects = Xamarin.Toolkit.Effects;

[assembly: ExportEffect(typeof(PlatformEffects.LabelCustomFont), nameof(RoutingEffects.LabelCustomFont))]
namespace Xamarin.Toolkit.Effects.iOS
{
    public class LabelCustomFont : PlatformEffect
    {
        RoutingEffects.LabelCustomFont effect;

        protected override void OnAttached()
        {
            var control = Control as UILabel;

            if (control == null)
                return;

            effect = (RoutingEffects.LabelCustomFont)Element.Effects.FirstOrDefault(item => item is RoutingEffects.LabelCustomFont);
            if (effect != null && !string.IsNullOrWhiteSpace(effect.FontPath))
                control.Font = UIFont.FromName(effect.FontFamilyName, control.Font.PointSize);

            // After one of these properties change, reapply the custom font
            // As per https://bugzilla.xamarin.com/show_bug.cgi?id=33666
            Element.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == Label.TextColorProperty.PropertyName
                    || e.PropertyName == Label.FontProperty.PropertyName
                    || e.PropertyName == Label.TextProperty.PropertyName
                    || e.PropertyName == Label.FormattedTextProperty.PropertyName)
                {
                    control.Font = UIFont.FromName(effect.FontFamilyName, control.Font.PointSize);
                }
            };
        }

        protected override void OnDetached()
        {
        }
    }
}
